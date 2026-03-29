import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProductService, Product, PagedResult } from '../services/product.service';
import { CartService } from '../services/cart.service';
import { LucideAngularModule, ShoppingCart, Info, Star } from 'lucide-angular';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterModule, LucideAngularModule],
  template: `
    <div class="container py-4">
      <div class="row row-cols-1 row-cols-md-3 row-cols-lg-4 g-4">
        <div class="col" *ngFor="let product of products">
          <div class="card h-100 shadow-sm border-0 product-card overflow-hidden">
            <div class="product-image bg-light d-flex align-items-center justify-content-center" style="height: 200px;">
              <span class="text-muted">No Image</span>
            </div>
            <div class="card-body d-flex flex-column p-3">
              <h5 class="card-title text-truncate fw-bold mb-1">{{ product.name }}</h5>
              <p class="card-text text-muted small text-truncate mb-2">{{ product.description }}</p>
              
              <div class="d-flex align-items-center mb-2">
                <span class="badge bg-warning text-dark d-flex align-items-center me-2">
                  <lucide-icon [name]="StarIcon" [size]="14" class="me-1"></lucide-icon> 4.5
                </span>
                <span class="text-muted small">(128)</span>
              </div>

              <div class="mt-auto d-flex align-items-center justify-content-between pt-2">
                <span class="fs-4 fw-bold text-dark">{{ product.price | currency:'USD' }}</span>
                <button (click)="addToCart(product)" class="btn btn-warning btn-sm d-flex align-items-center shadow-sm">
                  <lucide-icon [name]="CartIcon" [size]="16" class="me-1"></lucide-icon>
                  Add to Cart
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
      
      <div *ngIf="products.length === 0" class="text-center py-5">
        <h3 class="text-muted">No products found.</h3>
      </div>
    </div>
  `,
  styles: [`
    .product-card { transition: transform 0.2s; }
    .product-card:hover { transform: translateY(-5px); }
    .product-image { transition: background-color 0.2s; }
    .product-card:hover .product-image { background-color: #f8f9fa !important; }
  `]
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  readonly CartIcon = ShoppingCart;
  readonly StarIcon = Star;

  constructor(private productService: ProductService, private cartService: CartService) {}

  ngOnInit() {
    this.productService.getProducts({ pageSize: 12 }).subscribe(res => {
      this.products = res.items;
    });
  }

  addToCart(product: Product) {
    this.cartService.addToCart(product.id, 1).subscribe({
      next: () => alert(`${product.name} added to cart!`),
      error: (err) => console.error(err)
    });
  }
}
