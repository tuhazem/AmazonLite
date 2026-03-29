import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from './components/navbar.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterModule, NavbarComponent],
  template: `
    <div class="min-vh-100 bg-light">
      <app-navbar></app-navbar>
      <main class="container py-4">
        <router-outlet></router-outlet>
      </main>
      
      <footer class="bg-dark text-white py-4 mt-5">
        <div class="container text-center">
          <p class="mb-0 small text-muted">© 2024 Amazon Clone. All rights reserved.</p>
        </div>
      </footer>
    </div>
  `,
  styles: [`
    :host { display: block; }
    main { min-height: calc(100vh - 160px); }
  `]
})
export class AppComponent {}
