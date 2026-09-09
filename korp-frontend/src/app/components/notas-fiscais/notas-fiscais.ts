import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NotaFiscal, NotaFiscalService } from '../../services/nota-fiscal';

@Component({
  selector: 'app-notas-fiscais',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './notas-fiscais.html',
  styleUrl: './notas-fiscais.css',
})
export class NotasFiscaisComponent implements OnInit {
  private readonly notaFiscalService = inject(NotaFiscalService);

  notas: NotaFiscal[] = [];
  produtoCodigo: number | null = null;
  quantidade: number | null = null;
  erroServicoEstoque: boolean = false;
  imprimindoIds: Set<number> = new Set<number>();
  carregando: boolean = false;

  ngOnInit(): void {
    this.carregarNotas();
  }

  carregarNotas(): void {
    this.carregando = true;
    this.notaFiscalService.getNotas().subscribe({
      next: (data) => {
        this.notas = data;
        this.erroServicoEstoque = false;
        this.carregando = false;
      },
      error: (err) => {
        this.carregando = false;
        this.tratarErro(err);
      },
    });
  }

  criarNota(): void {
    if (!this.produtoCodigo || !this.quantidade || this.quantidade <= 0) {
      alert('Por favor, informe um código de produto válido e uma quantidade maior que zero.');
      return;
    }

    const payload = {
      itens: [
        {
          produtoCodigo: Number(this.produtoCodigo),
          quantidade: Number(this.quantidade),
        },
      ],
    };

    this.notaFiscalService.criarNota(payload).subscribe({
      next: () => {
        this.produtoCodigo = null;
        this.quantidade = null;
        this.erroServicoEstoque = false;
        this.carregarNotas();
      },
      error: (err) => {
        this.tratarErro(err);
      },
    });
  }

  imprimirNota(numeroSequencial: number): void {
    if (this.imprimindoIds.has(numeroSequencial)) {
      return;
    }

    this.imprimindoIds.add(numeroSequencial);

    this.notaFiscalService.imprimirNota(numeroSequencial).subscribe({
      next: () => {
        this.imprimindoIds.delete(numeroSequencial);
        this.erroServicoEstoque = false;
        this.carregarNotas();
      },
      error: (err) => {
        this.imprimindoIds.delete(numeroSequencial);
        this.tratarErro(err);
      },
    });
  }

  isNotaFechada(status: string): boolean {
    if (!status) return false;
    const statusLower = status.toLowerCase();
    return statusLower === 'fechada' || statusLower === 'fechado' || statusLower === 'emitida' || statusLower === 'cancelada';
  }

  fecharAlertaErro(): void {
    this.erroServicoEstoque = false;
  }

  private tratarErro(err: any): void {
    console.error('Erro na requisição:', err);
    if (err?.status === 503) {
      this.erroServicoEstoque = true;
    }
  }
}

