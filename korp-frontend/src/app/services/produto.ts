import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Produto {
  codigo: number;
  descricao: string;
  saldo: number;
}

@Injectable({
  providedIn: 'root',
})
export class ProdutoService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5001/api/produtos';

  getProdutos(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.apiUrl);
  }

  criarProduto(produto: Omit<Produto, 'codigo'>): Observable<Produto> {
    return this.http.post<Produto>(this.apiUrl, produto);
  }
}

