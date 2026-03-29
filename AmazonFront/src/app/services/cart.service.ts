import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../environments/environment';

export interface CartItem {
  id: number;
  productId: number;
  productName: string;
  unitPrice: number;
  quantity: number;
}

export interface Cart {
  id: number;
  items: CartItem[];
  total: number;
}

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartIdSubject = new BehaviorSubject<number | null>(null);
  private cartSubject = new BehaviorSubject<Cart | null>(null);
  public cart$ = this.cartSubject.asObservable();

  constructor(private http: HttpClient) {
    const savedId = localStorage.getItem('cartId');
    if (savedId) {
      this.cartIdSubject.next(+savedId);
      this.refreshCart(+savedId);
    } else {
      this.createCart();
    }
  }

  get cartIdValue(): number | null {
    return this.cartIdSubject.value;
  }

  createCart() {
    this.http.post<number>(`${environment.apiUrl}/Cart`, {}).subscribe(id => {
      localStorage.setItem('cartId', id.toString());
      this.cartIdSubject.next(id);
      this.refreshCart(id);
    });
  }

  refreshCart(id: number) {
    this.http.get<Cart>(`${environment.apiUrl}/Cart/${id}`).subscribe(cart => {
      this.cartSubject.next(cart);
    });
  }

  addToCart(productId: number, quantity: number): Observable<any> {
    const id = this.cartIdValue;
    if (!id) return new Observable(subscriber => subscriber.error('No cart id'));
    
    return this.http.post(`${environment.apiUrl}/Cart/${id}/items`, { productId, quantity }).pipe(
      tap(() => this.refreshCart(id))
    );
  }

  updateQuantity(productId: number, quantity: number): Observable<any> {
    const id = this.cartIdValue;
    if (!id) return new Observable(subscriber => subscriber.error('No cart id'));

    return this.http.put(`${environment.apiUrl}/Cart/${id}/items/${productId}`, { quantity }).pipe(
      tap(() => this.refreshCart(id))
    );
  }

  removeItem(productId: number): Observable<any> {
    const id = this.cartIdValue;
    if (!id) return new Observable(subscriber => subscriber.error('No cart id'));

    return this.http.delete(`${environment.apiUrl}/Cart/${id}/items/${productId}`).pipe(
      tap(() => this.refreshCart(id))
    );
  }

  clearCart(): Observable<any> {
    const id = this.cartIdValue;
    if (!id) return new Observable(subscriber => subscriber.error('No cart id'));

    return this.http.delete(`${environment.apiUrl}/Cart/${id}`).pipe(
      tap(() => this.cartSubject.next(null))
    );
  }
}
