import { Routes } from '@angular/router';
import { ProdutosComponent } from './components/produtos/produtos';
import { NotasFiscaisComponent } from './components/notas-fiscais/notas-fiscais';

export const routes: Routes = [
  { path: '', redirectTo: 'produtos', pathMatch: 'full' },
  { path: 'produtos', component: ProdutosComponent },
  { path: 'notas-fiscais', component: NotasFiscaisComponent },
];

