import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import { Product, CreateProductDto } from '../../models/product.model';

@Component({
  selector: 'app-stock-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './stock-list.html',
  styleUrl: './stock-list.scss'
})
export class StockList implements OnInit {
  private productService = inject(ProductService);


  products = signal<Product[]>([]);
  isLoading = signal<boolean>(false);
  errorMessage = signal<string>('');
  successMessage = signal<string>('');

  newProduct: CreateProductDto = {
    code: '',
    description: '',
    stockQuantity: 0
  };

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.productService.getProducts().subscribe({
      next: (data) => {
        this.products.set(data);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('Não foi possível carregar os produtos. Verifique se o StockService está ligado.');
        this.isLoading.set(false);
      }
    });
  }

  onSubmit(): void {
    if (!this.newProduct.code || !this.newProduct.description) {
      this.errorMessage.set('Por favor, preencha o código e a descrição do produto.');
      return;
    }

    this.errorMessage.set('');
    this.successMessage.set('');

    this.productService.createProduct(this.newProduct).subscribe({
      next: () => {
        this.successMessage.set('Produto cadastrado com sucesso!');
        this.newProduct = { code: '', description: '', stockQuantity: 0 };
        this.loadProducts();
      },
      error: (err) => {
        this.errorMessage.set(err.error?.message || 'Erro ao cadastrar produto.');
      }
    });
  }
}