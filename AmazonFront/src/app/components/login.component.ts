import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { LucideAngularModule, LogIn, Mail, Lock } from 'lucide-angular';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule, LucideAngularModule],
  template: `
    <div class="container d-flex align-items-center justify-content-center min-vh-100">
      <div class="card shadow-lg border-0 rounded-4 overflow-hidden" style="width: 100%; max-width: 450px;">
        <div class="card-header bg-dark text-white text-center py-4 border-0">
          <h2 class="fw-bold mb-1">Welcome Back</h2>
          <p class="text-muted small mb-0">Sign in to your Amazon account</p>
        </div>
        <div class="card-body p-4 p-md-5 bg-white">
          <form [formGroup]="loginForm" (ngSubmit)="onSubmit()">
            <div class="mb-4">
              <label class="form-label fw-bold text-dark">Email Address</label>
              <div class="input-group input-group-lg border-0 rounded-3 overflow-hidden bg-light shadow-sm">
                <span class="input-group-text bg-transparent border-0 pe-0 text-muted">
                  <lucide-icon [name]="MailIcon" [size]="20"></lucide-icon>
                </span>
                <input type="email" formControlName="email" class="form-control bg-transparent border-0 fs-6 py-3" placeholder="email@example.com">
              </div>
              <div *ngIf="f['email'].touched && f['email'].errors" class="text-danger mt-1 small">
                <span *ngIf="f['email'].errors['required']">Email is required.</span>
                <span *ngIf="f['email'].errors['email']">Invalid email format.</span>
              </div>
            </div>

            <div class="mb-4">
              <label class="form-label fw-bold text-dark">Password</label>
              <div class="input-group input-group-lg border-0 rounded-3 overflow-hidden bg-light shadow-sm">
                <span class="input-group-text bg-transparent border-0 pe-0 text-muted">
                  <lucide-icon [name]="LockIcon" [size]="20"></lucide-icon>
                </span>
                <input type="password" formControlName="password" class="form-control bg-transparent border-0 fs-6 py-3" placeholder="••••••••">
              </div>
              <div *ngIf="f['password'].touched && f['password'].errors" class="text-danger mt-1 small">
                <span *ngIf="f['password'].errors['required']">Password is required.</span>
              </div>
            </div>

            <div class="d-grid gap-2 mt-4">
              <button type="submit" [disabled]="loginForm.invalid || loading" class="btn btn-warning btn-lg fw-bold py-3 rounded-3 shadow-sm d-flex align-items-center justify-content-center">
                <span *ngIf="loading" class="spinner-border spinner-border-sm me-2"></span>
                <lucide-icon *ngIf="!loading" [name]="LogInIcon" [size]="20" class="me-2"></lucide-icon>
                Sign In
              </button>
            </div>
            
            <div *ngIf="error" class="alert alert-danger mt-3 small text-center">{{ error }}</div>

            <div class="text-center mt-4 pt-2">
              <p class="text-muted small mb-0">Don't have an account? 
                <a routerLink="/register" class="text-warning fw-bold text-decoration-none">Create New</a>
              </p>
            </div>
          </form>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .form-control:focus { box-shadow: none; }
    .btn-warning:hover { background-color: #f7b731; border-color: #f7b731; }
  `]
})
export class LoginComponent {
  loginForm: FormGroup;
  loading = false;
  error = '';
  readonly LogInIcon = LogIn;
  readonly MailIcon = Mail;
  readonly LockIcon = Lock;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  get f() { return this.loginForm.controls; }

  onSubmit() {
    if (this.loginForm.invalid) return;
    this.loading = true;
    this.error = '';
    
    this.authService.login(this.loginForm.value).subscribe({
      next: () => this.router.navigate(['/']),
      error: (err) => {
        this.error = 'Invalid email or password.';
        this.loading = false;
      }
    });
  }
}
