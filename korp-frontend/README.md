# 🚀 KORP ERP - Frontend Application

Uma aplicação web moderna, responsiva e resiliente desenvolvida em **Angular** para a gestão de produtos e emissão de notas fiscais, integrada a arquiteturas de microserviços.

---

## 📌 Sumário
- [Visão Geral](#-visão-geral)
- [Funcionalidades Principais](#-funcionalidades-principais)
- [Tecnologias & Stacks Utilizadas](#-tecnologias--stacks-utilizadas)
- [Padrões de Código & Boas Práticas](#-padrões-de-código--boas-práticas)
- [Arquitetura de Pastas](#-arquitetura-de-pastas)
- [Integração com APIs / Microserviços](#-integração-com-apis--microserviços)
- [Como Executar o Projeto](#-como-executar-o-projeto)

---

## 🔍 Visão Geral

O **KORP ERP Frontend** é uma SPA (Single Page Application) focada em fornecer uma interface de usuário elegante e intuitiva para o gerenciamento de estoque e faturamento. O sistema se comunica com dois microserviços distintos backend RESTful:
1. **Microserviço de Produtos (Estoque)**
2. **Microserviço de Notas Fiscais (Faturamento)**

---

## ⚡ Funcionalidades Principais

### 📦 Módulo de Produtos (`/produtos`)
- **Cadastro de Produtos:** Formulário para inserção de descrição e saldo inicial de produtos.
- **Listagem de Estoque:** Tabela informativa exibindo código, descrição e saldo, com tratamento para saldos zerados ou negativos.
- **Validação de Entrada:** Validação de campos obrigatórios antes do envio da requisição.

### 📄 Módulo de Notas Fiscais (`/notas-fiscais`)
- **Emissão de Notas Fiscais:** Formulário para emissão de notas com associação de código de produto e quantidade.
- **Listagem com Badges Dinâmicas:** Exibição do status das notas com marcadores visuais diferenciados (`Aberta` vs `Fechada`).
- **Detalhamento de Itens:** Exibição dos itens da nota em chips visuais compactos.
- **Ação de Impressão:** Botão de impressão com controle de estado assíncrono por nota (*loading spinner*), prevenindo cliques múltiplos e desabilitado quando a nota estiver finalizada/fechada.
- **Tratamento Resiliente de Erros HTTP 503:** Exibição de banner de alerta em destaque no topo da aplicação quando o serviço de estoque estiver temporariamente indisponível.

### 🧭 Navegação & Layout
- **Barra de Navegação Topo (Navbar):** Menu de navegação superior moderno com indicação da rota ativa (`RouterLinkActive`).
- **Interface Responsiva & Polida:** Design baseado em cartões (*cards*), sombras suaves, micro-animações e estados vazios (*empty states*) amigáveis.

---

## 🛠️ Tecnologias & Stacks Utilizadas

| Categoria | Tecnologia / Ferramenta | Descrição |
| :--- | :--- | :--- |
| **Core Framework** | **Angular 21+** | Componentes Standalone, Injeção Funcional (`inject()`), Control Flow & Diretivas |
| **Linguagem** | **TypeScript 5+** | Tipagem estrita, Interfaces, Generics e Utility Types (`Omit`, `Partial`) |
| **Comunicação Async** | **RxJS & HttpClient** | Programação reativa com Observables e suporte a Fetch (`withFetch()`) |
| **Roteamento** | **Angular Router** | Navegação Single Page com `RouterOutlet`, `RouterLink` e `RouterLinkActive` |
| **Estilização** | **CSS3 (Vanilla)** | CSS Grid, Flexbox, Animações Keyframes e Design System customizado |
| **Build & Tooling** | **Angular CLI & Vitest** | Compilação otimizada, bundling e suporte a testes unitários |

---

## 📐 Padrões de Código & Boas Práticas

- **Angular Style Guide:** Estrutura de arquivos e nomenclatura padronizada (`.component.ts`, `.service.ts`, `.spec.ts`).
- **Injeção de Dependência Moderna:** Utilização de `inject(HttpClient)` e `inject(ProdutoService)` ao invés da injeção via construtor legado.
- **Clean Code & Tipagem Forte:** Definição de contratos claros com interfaces TypeScript (`Produto`, `NotaFiscal`, `ItemNotaFiscal`, `CriarNotaFiscalPayload`).
- **Componentes Standalone:** Arquitetura limpa sem necessidade de módulos pesados (`NgModule`).
- **Tratamento de Erro Resiliente:** Tratamento declarativo de falhas HTTP de rede e códigos de status (HTTP 503 Service Unavailable).

---

## 📁 Arquitetura de Pastas

```text
src/app/
├── components/
│   ├── produtos/
│   │   ├── produtos.ts          # Lógica do componente de produtos
│   │   ├── produtos.html        # Template HTML do formulário e tabela
│   │   ├── produtos.css         # Estilização CSS do módulo de produtos
│   │   └── produtos.spec.ts     # Testes unitários
│   └── notas-fiscais/
│       ├── notas-fiscais.ts     # Lógica do componente de notas fiscais (com tratamento 503)
│       ├── notas-fiscais.html   # Template HTML com formulário, tabela e banner de alerta
│       ├── notas-fiscais.css    # Estilização dos componentes e badges
│       └── notas-fiscais.spec.ts# Testes unitários
├── services/
│   ├── produto.ts               # Serviço HTTP e interface de Produtos
│   ├── produto.spec.ts          # Teste do serviço de produtos
│   ├── nota-fiscal.ts           # Serviço HTTP e contratos de Notas Fiscais
│   └── nota-fiscal.spec.ts      # Teste do serviço de notas fiscais
├── app.routes.ts                # Definição de rotas da aplicação
├── app.config.ts                # Configuração do Angular (Router + HttpClient withFetch)
├── app.ts                       # Componente raiz da aplicação
├── app.html                     # Navbar de navegação topo e RouterOutlet
└── app.css                      # Estilos globais e da Navbar
```

---

## 🌐 Integração com APIs / Microserviços

Os serviços Angular estão configurados para se comunicar com as seguintes APIs REST backend:

- **Serviço de Produtos (`ProdutoService`):**
  - Base URL: `https://localhost:7252/api/produtos`
  - `GET /api/produtos` - Listar produtos
  - `POST /api/produtos` - Criar novo produto

- **Serviço de Notas Fiscais (`NotaFiscalService`):**
  - Base URL: `https://localhost:7264/api/notasFiscais`
  - `GET /api/notasFiscais` - Listar notas fiscais
  - `POST /api/notasFiscais` - Emitir nova nota fiscal
  - `POST /api/notasFiscais/{numeroSequencial}/imprimir` - Disparar impressão de nota fiscal

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
- **Node.js**: v18.x ou superior (LTS recomendado)
- **npm**: v9.x ou superior
- **Angular CLI**: instalado globalmente ou via `npx`

### 1. Clonar o Repositório e Instalar Dependências
```bash
# Clone o repositório
git clone https://github.com/IsaqueR02/Korp_Teste_Isaque.git

# Acesse a pasta do frontend
cd Korp_Teste_Isaque/korp-frontend

# Instale as dependências
npm install
```

### 2. Executar o Servidor de Desenvolvimento
```bash
# Iniciar o servidor local
npm run dev
# ou
ng serve
```

Acesse no navegador: **`http://localhost:4200`**

### 3. Executar Testes Unitários
```bash
ng test
```

### 4. Compilar para Produção
```bash
npm run build
```

---
*Desenvolvido por Isaque para o teste prático de avaliação técnica Korp ERP.*
