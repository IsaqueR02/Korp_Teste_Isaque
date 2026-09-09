import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Produto, ProdutoService } from '../../services/produto';

@Component({
  selector: 'app-produtos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './produtos.html',
  styleUrl: './produtos.css',
})
export class ProdutosComponent implements OnInit {
  private readonly produtoService = inject(ProdutoService);

  produtos: Produto[] = [];
  novaDescricao: string = '';
  novoSaldo: number | null = null;

  ngOnInit(): void {
    this.carregarProdutos();
  }

  carregarProdutos(): void {
    this.produtoService.getProdutos().subscribe({
      next: (data) => {
        this.produtos = data;
      },
      error: (err) => {
        console.error('Erro ao carregar produtos:', err);
      },
    });
  }

  salvarProduto(): void {
    if (!this.novaDescricao.trim() || this.novoSaldo === null) {
      alert('Por favor, preencha a descrição e o saldo do produto.');
      return;
    }

    const novoProduto = {
      descricao: this.novaDescricao.trim(),
      saldo: Number(this.novoSaldo),
    };

    this.produtoService.criarProduto(novoProduto).subscribe({
      next: () => {
        this.novaDescricao = '';
        this.novoSaldo = null;
        this.carregarProdutos();
      },
      error: (err) => {
        console.error('Erro ao cadastrar produto:', err);
      },
    });
  }
}

