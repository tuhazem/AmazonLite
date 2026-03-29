import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { CartService } from '../services/cart.service';
import { LucideAngularModule, ShoppingCart, LogIn, LogOut, User, Search } from 'lucide-angular';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule, LucideAngularModule],
  template: `
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark sticky-top">
      <div class="container">
        <a class="navbar-brand d-flex align-items-center" routerLink="/">
          <span class="fw-bold fs-3 text-warning">Amazon</span>
        </a>
        
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
          <span class="navbar-toggler-icon"></span>
        </button>
        
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav me-auto">
            <li class="nav-item">
              <a class="nav-link" routerLink="/" routerLinkActive="active" [routerLinkActiveOptions]="{exact: true}">Home</a>
            </li>
          </ul>

          <div class="d-flex align-items-center">
            <ng-container *ngIf="authService.currentUser$ | async as user; else guest">
              <div class="nav-item dropdown me-3">
                <a class="nav-link dropdown-toggle text-white d-flex align-items-center" href="#" id="userDropdown" role="button" data-bs-toggle="dropdown">
                  <lucide-icon [name]="UserIcon" class="me-1" [size]="20"></lucide-icon>
                  {{ user.email }}
                </a>
                <ul class="dropdown-menu dropdown-menu-end">
                  <li><button class="dropdown-item d-flex align-items-center" (click)="logout()">
                    <lucide-icon [name]="LogOutIcon" class="me-2" [size]="16"></lucide-icon> Logout
                  </button></li>
                </ul>
              </div>
            </ng-container>
            
            <ng-template #guest>
              <a routerLink="/login" class="btn btn-outline-light me-2 d-flex align-items-center">
                <lucide-icon [name]="LogInIcon" class="me-1" [size]="18"></lucide-icon> Login
              </a>
            </ng-template>

            <a routerLink="/cart" class="btn btn-warning d-flex align-items-center position-relative">
              <lucide-icon [name]="CartIcon" class="me-1" [size]="20"></lucide-icon>
              Cart
              <span *ngIf="(cartService.cart$ | async)?.items?.length" class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
                {{ (cartService.cart$ | async)?.items?.length }}
              </span>
            </a>
          </div>
        </div>
      </div>
    </nav>
  `,
  styles: [`
    .navbar-brand { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; }
    .nav-link { font-weight: 500; }
  `]
})
export class NavbarComponent {
  readonly CartIcon = ShoppingCart;
  readonly LogInIcon = LogIn;
  readonly LogOutIcon = LogOut;
  readonly UserIcon = User;
  readonly SearchIcon = Search;

  constructor(public authService: AuthService, public cartService: CartService) {}

  logout() {
    this.authService.logout();
  }
}
