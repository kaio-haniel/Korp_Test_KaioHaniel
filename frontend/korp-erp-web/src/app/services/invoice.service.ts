import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Invoice, CreateInvoiceDto } from '../models/invoice.model';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5002/api/invoices';


  getInvoices(): Observable<Invoice[]> {
    return this.http.get<Invoice[]>(this.apiUrl);
  }


  getInvoiceById(id: number): Observable<Invoice> {
    return this.http.get<Invoice>(`${this.apiUrl}/${id}`);
  }


  createInvoice(dto: CreateInvoiceDto): Observable<Invoice> {
    return this.http.post<Invoice>(this.apiUrl, dto);
  }


  closeInvoice(id: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${id}/close`, {});
  }
}