import { Routes } from '@angular/router';
import { StockList } from './pages/stock-list/stock-list';
import { InvoiceList } from './pages/invoice-list/invoice-list';
import { InvoiceCreate } from './pages/invoice-create/invoice-create';

export const routes: Routes = [
  { path: '', redirectTo: 'estoque', pathMatch: 'full' },
  { path: 'estoque', component: StockList },
  { path: 'faturamento', component: InvoiceList },
  { path: 'faturamento/nova', component: InvoiceCreate }
];
