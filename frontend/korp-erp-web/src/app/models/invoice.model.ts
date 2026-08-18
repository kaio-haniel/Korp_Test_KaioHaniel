export enum InvoiceStatus {
  Open = 0,
  Closed = 1
}

export interface InvoiceItem {
  id: number;
  invoiceId: number;
  productId: number;
  productCode: string;
  productDescription: string;
  quantity: number;
}

export interface Invoice {
  id: number;
  number: number;
  status: InvoiceStatus | number;
  createAt: string; // no C# está CreateAt
  items: InvoiceItem[];
}

export interface CreateInvoiceItemDto {
  productId: number;
  productCode: string;
  productDescription: string;
  quantity: number;
}

export interface CreateInvoiceDto {
  items: CreateInvoiceItemDto[];
}