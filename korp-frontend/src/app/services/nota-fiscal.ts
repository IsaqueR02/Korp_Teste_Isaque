import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ItemNotaFiscal {
  produtoCodigo: number;
  quantidade: number;
}

export interface NotaFiscal {
  numeroSequencial: number;
  status: string;
  itens: ItemNotaFiscal[];
}

export type CriarNotaFiscalPayload = Omit<NotaFiscal, 'numeroSequencial' | 'status'> & { status?: string };

@Injectable({
  providedIn: 'root',
})
export class NotaFiscalService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7264/api/notasFiscais';

  getNotas(): Observable<NotaFiscal[]> {
    return this.http.get<NotaFiscal[]>(this.apiUrl);
  }

  criarNota(payload: CriarNotaFiscalPayload): Observable<NotaFiscal> {
    return this.http.post<NotaFiscal>(this.apiUrl, payload);
  }

  imprimirNota(numeroSequencial: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${numeroSequencial}/imprimir`, {});
  }
}


