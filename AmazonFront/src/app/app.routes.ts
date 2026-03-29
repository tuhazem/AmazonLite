import { Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list.component';
import { LoginComponent } from './components/login.component';
import { CartComponent } from './components/cart.component';

export const routes: Routes = [
  { path: '', component: ProductListComponent },
  { path: 'login', component: LoginComponent },
  { path: 'cart', component: CartComponent },
  { path: '**', redirectTo: '' }
];
