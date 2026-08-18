export interface Product {
    id: number;
    code: string;
    description: string;
    stockQuantity: number;
  }
  
  export interface CreateProductDto {
    code: string;
    description: string;
    stockQuantity: number;
  }