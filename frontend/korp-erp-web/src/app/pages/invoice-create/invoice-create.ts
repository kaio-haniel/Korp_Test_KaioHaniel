import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { InvoiceService } from '../../services/invoice.service';
import { Product } from '../../models/product.model';
import { CreateInvoiceItemDto } from '../../models/invoice.model';

@Component({
  selector: 'app-invoice-create',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './invoice-create.html',
  styleUrl: './invoice-create.scss'
})
export class InvoiceCreate implements OnInit {
  private productService = inject(ProductService);
  private invoiceService = inject(InvoiceService);
  private router = inject(Router);

  products = signal<Product[]>([]);
  selectedProductId: number | null = null;
  selectedQuantity: number = 1;

  items = signal<CreateInvoiceItemDto[]>([]);
  errorMessage = signal<string>('');
  isSubmitting = signal<boolean>(false);

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe({
      next: (data) => {
        this.products.set(data);
      },
      error: () => {
        this.errorMessage.set('Não foi possível carregar os produtos disponíveis no estoque.');
      }
    });
  }

  addItem(): void {
    if (!this.selectedProductId || this.selectedQuantity <= 0) {
      this.errorMessage.set('Selecione um produto e informe uma quantidade válida maior que zero.');
      return;
    }

    const prodId = Number(this.selectedProductId);
    const product = this.products().find(p => p.id === prodId);
    if (!product) return;

    const currentItems = [...this.items()];
    const existingItem = currentItems.find(i => i.productId === prodId);

    if (existingItem) {
      existingItem.quantity += Number(this.selectedQuantity);
    } else {
      currentItems.push({
        productId: prodId,
        productCode: product.code,
        productDescription: product.description,
        quantity: Number(this.selectedQuantity)
      });
    }

    this.items.set(currentItems);
    this.errorMessage.set('');
    this.selectedProductId = null;
    this.selectedQuantity = 1;
  }

  removeItem(index: number): void {
    const currentItems = [...this.items()];
    currentItems.splice(index, 1);
    this.items.set(currentItems);
  }

  saveInvoice(): void {
    if (this.items().length === 0) {
      this.errorMessage.set('Adicione ao menos um produto antes de salvar a nota fiscal.');
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set('');

    this.invoiceService.createInvoice({ items: this.items() }).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.router.navigate(['/faturamento']);
      },
      error: (err) => {
        this.isSubmitting.set(false);
        this.errorMessage.set(err.error || 'Erro ao gerar a nota fiscal.');
      }
    });
  }
}