# Desafio Técnico Full Stack - KORP

## 1. Título e Visão Geral da Solução

### Visão Geral da Arquitetura
Esta solução foi desenvolvida seguindo uma arquitetura distribuída composta por dois microsserviços backend independentes em **.NET 8** e uma aplicação frontend Single Page Application (SPA) em **Angular Standalone**. A estrutura garante uma rigorosa separação de responsabilidades e independência entre o domínio de controle de estoque e o domínio contábil/fiscal de faturamento.

```
┌─────────────────────────────────────────────────────────────┐
│                   Frontend Angular (SPA)                    │
│                    http://localhost:4200                    │
└───────────────┬─────────────────────────────┬───────────────┘
                │                             │
                ▼                             ▼
┌───────────────────────────────┐   ┌───────────────────────────────┐
│     FaturamentoService        │   │        EstoqueService         │
│     http://localhost:7264     │──>│     http://localhost:7252     │
│   (Emissão e Fechamento NF)   │   │     (Produtos e Saldos)       │
└───────────────────────────────┘   └───────────────────────────────┘
```

* **EstoqueService (Porta 7252)**: Microsserviço responsável pela gestão dos produtos e seus respectivos saldos em estoque.
* **FaturamentoService (Porta 7264)**: Microsserviço responsável pelo ciclo de vida das Notas Fiscais (criação, adição de itens, cancelamento e fechamento), comunicando-se de forma resiliente com o `EstoqueService` para realizar a baixa dos itens no momento do fechamento da nota fiscal.
* **Frontend SPA (Porta 4200)**: Interface web moderna construída com componentes standalone do Angular, proporcionando uma experiência de usuário responsiva e dinâmica.

---

## 2. Demonstração em Vídeo

[![Assistir Demonstração em Vídeo no YouTube](https://img.youtube.com/vi/6zZkjyMyxnM/hqdefault.jpg)](https://youtu.be/6zZkjyMyxnM)

👉 **[Assistir Demonstração em Vídeo no YouTube](https://youtu.be/6zZkjyMyxnM)**

### Cenários Validados no Vídeo
1. **Cadastro e Gestão de Produtos**: Inclusão de produtos no `EstoqueService` com visualização de saldo inicial.
2. **Emissão e Fechamento de Nota Fiscal**: Criação de nota fiscal no status "Aberta", inclusão de itens e acionamento do processo de fechamento com baixa automática e síncrona no saldo do estoque.
3. **Simulação de Resiliência e Tolerância a Falhas**: Simulação de indisponibilidade do `EstoqueService` durante a tentativa de fechamento de uma nota fiscal. Demonstração de que o sistema intercepta a falha, preserva a nota com status "Aberta" (evitando inconsistência de faturamento) e retorna o status **HTTP 503 (Service Unavailable)** com mensagem amigável ao usuário.

---

## 3. Tecnologias Utilizadas

![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC292B?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Angular](https://img.shields.io/badge/Angular%2017+-DD0031?style=for-the-badge&logo=angular&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
![RxJS](https://img.shields.io/badge/RxJS-B7178C?style=for-the-badge&logo=reactivex&logoColor=white)

### Backend
* **.NET 8 / ASP.NET Core Web API**: Framework principal para construção dos microsserviços RESTful.
* **Entity Framework Core**: ORM para mapeamento objeto-relacional e persistência de dados.
* **SQL Server**: Banco de dados relacional.
* **IHttpClientFactory**: Gerenciamento eficiente e resiliente de conexões HTTP entre os microsserviços.

### Frontend
* **Angular 17+**: Arquitetura modular utilizando **Standalone Components**.
* **TypeScript**: Linguagem base tipada para garantir robustez no desenvolvimento.
* **RxJS**: Manipulação de fluxos de dados reativos e operações assíncronas.
* **`provideHttpClient(withFetch())`**: Configuração moderna de cliente HTTP para consumo das APIs backend.

---

## 4. Resiliência e Comunicação entre Microsserviços (Destaque Avaliativo)

A comunicação entre o **FaturamentoService** e o **EstoqueService** ocorre no momento do fechamento da Nota Fiscal. Para garantir a integridade dos dados e prevenir inconsistências contábeis, foi implementado um mecanismo de resiliência e tratamento de exceções robusto.

### Orquestração da Baixa de Estoque
1. O usuário solicita o fechamento de uma Nota Fiscal no status "Aberta".
2. O `FaturamentoService` inicia a transação e, através do `IHttpClientFactory`, realiza a requisição ao `EstoqueService` na porta `7252` para decrementar as quantidades dos produtos constantes na nota fiscal.
3. Se a comunicação for bem-sucedida e houver saldo disponível, o estoque é atualizado e a Nota Fiscal transita para o status "Fechada".

### Contingência e Tratamento de Exceções
* **Interceptação de Indisponibilidade**: Em caso de falha de rede ou indisponibilidade do serviço de estoque (`HttpRequestException`), o backend intercepta a exceção antes de concluir a alteração de status da nota.
* **Preservação da Integridade Contábil**: A nota fiscal **permanece no status "Aberta"**, impedindo que um faturamento seja concluído sem a garantia da baixa no estoque.
* **Resposta Amigável ao Cliente**: O `FaturamentoService` responde à requisição com o código **HTTP 503 (Service Unavailable)**, enviando um payload detalhado e uma mensagem amigável para exibição no frontend.

---

## 5. Conceitos Teóricos do Angular (Exigência do Edital)

### Ciclos de Vida (`ngOnInit`)
Emprego do hook de ciclo de vida `ngOnInit` nos componentes Angular para inicializar o carregamento reativo das tabelas de produtos e notas fiscais assim que o componente é renderizado na DOM.

### Reatividade com RxJS
Adoção do paradigma de programação reativa utilizando **Observables** para gerenciar as chamadas HTTP assíncronas. O operador `.subscribe()` é utilizado para tratar a resposta dos dados e capturar eventuais falhas de rede (`HttpErrorResponse`), permitindo a exibição imediata de feedbacks na interface.

### Injeção de Dependência Moderna
Utilização da função moderna `inject()` em substituição à injeção via construtor tradicional:
```typescript
private httpClient = inject(HttpClient);
private produtoService = inject(ProdutoService);
```
Esta abordagem alinha-se aos padrões recomendados a partir do Angular 14+, simplificando a herança de classes e tornando os **Standalone Components** mais limpos e modulares.

### Feedback Visual e UX (User Experience)
* **Estado de Carregamento**: Controle visual de estado durante requisições assíncronas, exibindo spinners e legendas como `"Processando..."` para evitar cliques duplicados.
* **Bloqueio Condicional**: Aplicação da diretiva `[disabled]` em botões de ação (ex.: botão de impressão ou fechamento), desabilitando interações quando a nota fiscal já se encontra no status `"Fechada"` ou quando o formulário está inválido.

---

## 6. Instruções de Execução Local

### Pré-requisitos
* **.NET 8 SDK** instalado.
* **Node.js LTS** (v18+) e **npm** instalados.
* **SQL Server** em execução (LocalDB ou instância completa).

---

### Passo 1: Inicialização do EstoqueService (Porta 7252)
No terminal, navegue até o diretório do microsserviço de Estoque e execute:

```bash
cd src/EstoqueService
dotnet run --urls "http://localhost:7252"
```

---

### Passo 2: Inicialização do FaturamentoService (Porta 7264)
Em outro terminal, navegue até o diretório do microsserviço de Faturamento e execute:

```bash
cd src/FaturamentoService
dotnet run --urls "http://localhost:7264"
```

---

### Passo 3: Inicialização do Frontend Angular (Porta 4200)
Em um terceiro terminal, navegue até o diretório da aplicação frontend, instale as dependências e inicie o servidor de desenvolvimento:

```bash
cd korp-frontend
npm install
ng serve
```

Acesse a aplicação no navegador em: **[http://localhost:4200](http://localhost:4200)**