import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product, CreateProductDto } from '../models/product.model';

@Injectable({
  providedIn: 'root' 
})
export class ProductService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5001/api/products';


  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }

  
  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`);
  }

  
  createProduct(dto: CreateProductDto): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, dto);
  }
}