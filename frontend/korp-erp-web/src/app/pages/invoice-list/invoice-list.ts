import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { InvoiceService } from '../../services/invoice.service';
import { Invoice, InvoiceStatus } from '../../models/invoice.model';

@Component({
  selector: 'app-invoice-list',
  imports: [CommonModule, RouterLink],
  templateUrl: './invoice-list.html',
  styleUrl: './invoice-list.scss'
})
export class InvoiceList implements OnInit {
  private invoiceService = inject(InvoiceService);

  invoices = signal<Invoice[]>([]);
  isLoading = signal<boolean>(false);
  errorMessage = signal<string>('');
  successMessage = signal<string>('');
  InvoiceStatus = InvoiceStatus;

  ngOnInit(): void {
    this.loadInvoices();
  }

  loadInvoices(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.invoiceService.getInvoices().subscribe({
      next: (data) => {
        console.log('Notas Fiscais vindas da API:', data); // <-- Adicione este log
        this.invoices.set(data);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('Erro ao carregar as notas fiscais.');
        this.isLoading.set(false);
      }
    });
  }

  onCloseInvoice(invoice: Invoice): void {
    if (invoice.status === InvoiceStatus.Closed) return;

    const confirmClose = confirm(`Deseja fechar a Nota Fiscal #${invoice.number}?`);
    if (!confirmClose) return;

    this.errorMessage.set('');
    this.successMessage.set('');

    this.invoiceService.closeInvoice(invoice.id).subscribe({
      next: () => {
        this.successMessage.set(`Nota Fiscal #${invoice.number} fechada com sucesso!`);
        this.loadInvoices();
      },
      error: (err) => {
        this.errorMessage.set(err.error || 'Erro ao fechar nota fiscal.');
      }
    });
  }
}