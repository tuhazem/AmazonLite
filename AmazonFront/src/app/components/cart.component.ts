import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { CartService, CartItem } from '../services/cart.service';
import { OrderService } from '../services/order.service';
import { AuthService } from '../services/auth.service';
import { LucideAngularModule, ShoppingCart, Trash2, Plus, Minus, CreditCard, ShoppingBag } from 'lucide-angular';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule, LucideAngularModule],
  template: `
    <div class="container py-5 min-vh-100">
      <div class="row g-5">
        <div class="col-lg-8">
          <div class="card shadow-sm border-0 rounded-4 overflow-hidden">
            <div class="card-header bg-white py-4 px-4 d-flex align-items-center justify-content-between">
              <h2 class="h4 mb-0 fw-bold d-flex align-items-center text-dark">
                <lucide-icon [name]="CartIcon" [size]="24" class="me-3 text-warning"></lucide-icon>
                Shopping Cart
              </h2>
              <span *ngIf="(cartService.cart$ | async)?.items?.length" class="badge bg-light text-muted border px-3 py-2 rounded-pill">
                {{ (cartService.cart$ | async)?.items?.length }} Items
              </span>
            </div>
            
            <div class="card-body p-0">
              <ng-container *ngIf="cartService.cart$ | async as cart; else emptyCart">
                <div *ngIf="cart.items.length > 0; else emptyCart">
                  <div *ngFor="let item of cart.items" class="p-4 border-bottom cart-item-row bg-white">
                    <div class="row align-items-center g-3">
                      <div class="col-md-2 text-center">
                        <div class="bg-light rounded-3 d-flex align-items-center justify-content-center mx-auto" style="width: 80px; height: 80px;">
                          <span class="text-muted small">Img</span>
                        </div>
                      </div>
                      <div class="col-md-4">
                        <h5 class="mb-1 fw-bold text-dark text-truncate">{{ item.productName }}</h5>
                        <p class="text-muted small mb-0 d-flex align-items-center">
                          <span class="me-2">Unit Price:</span>
                          <span class="fw-bold text-dark">{{ item.unitPrice | currency }}</span>
                        </p>
                      </div>
                      <div class="col-md-3">
                        <div class="input-group input-group-sm rounded-pill overflow-hidden border bg-light shadow-sm mx-auto" style="max-width: 120px;">
                          <button (click)="updateQuantity(item, -1)" class="btn btn-link text-dark px-2 border-0" [disabled]="item.quantity <= 1">
                            <lucide-icon [name]="MinusIcon" [size]="14"></lucide-icon>
                          </button>
                          <span class="form-control text-center bg-transparent border-0 fw-bold fs-6">{{ item.quantity }}</span>
                          <button (click)="updateQuantity(item, 1)" class="btn btn-link text-dark px-2 border-0">
                            <lucide-icon [name]="PlusIcon" [size]="14"></lucide-icon>
                          </button>
                        </div>
                      </div>
                      <div class="col-md-2 text-end">
                        <span class="fs-5 fw-bold text-dark">{{ item.unitPrice * item.quantity | currency }}</span>
                      </div>
                      <div class="col-md-1 text-end">
                        <button (click)="removeItem(item.productId)" class="btn btn-outline-danger btn-sm rounded-circle border-0 shadow-sm" title="Remove Item">
                          <lucide-icon [name]="TrashIcon" [size]="18"></lucide-icon>
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              </ng-container>
              
              <ng-template #emptyCart>
                <div class="text-center py-5 bg-white">
                  <lucide-icon [name]="CartIcon" [size]="64" class="text-muted mb-4 opacity-25"></lucide-icon>
                  <h3 class="h5 text-muted mb-3">Your cart is currently empty</h3>
                  <a routerLink="/" class="btn btn-warning px-5 fw-bold rounded-pill shadow-sm">
                    <lucide-icon [name]="ShoppingBagIcon" [size]="20" class="me-2"></lucide-icon>
                    Start Shopping
                  </a>
                </div>
              </ng-template>
            </div>
          </div>
        </div>
        
        <div class="col-lg-4" *ngIf="(cartService.cart$ | async)?.items?.length">
          <div class="card shadow-sm border-0 rounded-4 overflow-hidden position-sticky" style="top: 100px;">
            <div class="card-header bg-dark text-white py-4 px-4 border-0">
              <h2 class="h5 mb-0 fw-bold">Order Summary</h2>
            </div>
            <div class="card-body p-4 bg-white">
              <div class="d-flex justify-content-between mb-3 align-items-center">
                <span class="text-muted">Subtotal</span>
                <span class="fw-bold fs-5 text-dark">{{ (cartService.cart$ | async)?.total | currency }}</span>
              </div>
              <div class="d-flex justify-content-between mb-3 align-items-center">
                <span class="text-muted">Shipping</span>
                <span class="text-success fw-bold small">FREE</span>
              </div>
              <hr class="my-4">
              <div class="d-flex justify-content-between mb-5 align-items-center">
                <span class="h4 mb-0 fw-bold text-dark">Total</span>
                <span class="h3 mb-0 fw-bold text-warning">{{ (cartService.cart$ | async)?.total | currency }}</span>
              </div>
              
              <div class="d-grid gap-3">
                <button (click)="checkout()" class="btn btn-warning btn-lg fw-bold py-3 rounded-3 shadow-sm d-flex align-items-center justify-content-center">
                  <lucide-icon [name]="CreditCardIcon" [size]="20" class="me-2"></lucide-icon>
                  Proceed to Checkout
                </button>
                <button (click)="cartService.clearCart()" class="btn btn-outline-danger btn-sm border-0 fw-bold">
                  Clear Cart
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .cart-item-row { transition: background-color 0.2s; }
    .cart-item-row:hover { background-color: #f8f9fa !important; }
    .btn-link:hover { text-decoration: none; }
    .btn-link:focus { box-shadow: none; }
  `]
})
export class CartComponent {
  readonly CartIcon = ShoppingCart;
  readonly TrashIcon = Trash2;
  readonly PlusIcon = Plus;
  readonly MinusIcon = Minus;
  readonly CreditCardIcon = CreditCard;
  readonly ShoppingBagIcon = ShoppingBag;

  constructor(
    public cartService: CartService,
    private orderService: OrderService,
    private authService: AuthService,
    private router: Router
  ) {}

  updateQuantity(item: CartItem, change: number) {
    this.cartService.updateQuantity(item.productId, item.quantity + change).subscribe();
  }

  removeItem(productId: number) {
    this.cartService.removeItem(productId).subscribe();
  }

  checkout() {
    if (!this.authService.currentUserValue) {
      this.router.navigate(['/login']);
      return;
    }

    const cartId = this.cartService.cartIdValue;
    if (cartId) {
      this.orderService.checkout(cartId).subscribe({
        next: (order) => {
          alert(`Order placed successfully! Order ID: ${order.id}`);
          this.cartService.clearCart().subscribe();
          this.router.navigate(['/']);
        },
        error: (err) => alert('Checkout failed: ' + (err.error?.detail || err.message))
      });
    }
  }
}
