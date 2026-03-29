import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

export interface OrderItem {
  id: number;
  productId: number;
  productName: string;
  unitPrice: number;
  quantity: number;
}

export interface Order {
  id: number;
  createdAt: string;
  total: number;
  status: number;
  items: OrderItem[];
}

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  constructor(private http: HttpClient) {}

  checkout(cartId: number): Observable<Order> {
    return this.http.post<Order>(`${environment.apiUrl}/Order/checkout/${cartId}`, {});
  }

  getOrder(id: number): Observable<Order> {
    return this.http.get<Order>(`${environment.apiUrl}/Order/${id}`);
  }

  updateStatus(id: number, status: number): Observable<any> {
    return this.http.put(`${environment.apiUrl}/Order/${id}/status`, status);
  }
}
