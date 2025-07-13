import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="modal-backdrop" (click)="close()"></div>
    <div class="modal-dialog-centered">
      <form (ngSubmit)="onSubmit()" #loginForm="ngForm" class="modal-content p-4">
        <h2 class="mb-3">Sign in</h2>
        <div *ngIf="registered" class="text-success mb-2">Registration successful! Please log in.</div>
        <div class="mb-2">
          <label for="userName" class="form-label">Username</label>
          <input id="userName" name="userName" [(ngModel)]="userName" required class="form-control" />
        </div>
        <div class="mb-3">
          <label for="password" class="form-label">Password</label>
          <input id="password" name="password" type="password" [(ngModel)]="password" required class="form-control" />
        </div>
        <button type="submit" class="btn btn-primary w-100">Sign in</button>
        <div *ngIf="error" class="text-danger mt-2">{{ error }}</div>
      </form>
    </div>
  `,
  styles: [`
    .modal-backdrop {
      position: fixed;
      top: 0; left: 0; right: 0; bottom: 0;
      background: rgba(0,0,0,0.3);
      z-index: 1040;
    }
    .modal-dialog-centered {
      position: fixed;
      top: 50%; left: 50%;
      transform: translate(-50%, -50%);
      z-index: 1050;
      min-width: 320px;
      max-width: 90vw;
    }
    .modal-content {
      background: #fff;
      border-radius: 8px;
      box-shadow: 0 2px 16px rgba(0,0,0,0.15);
    }
  `]
})
export class LoginModalComponent {
  userName = '';
  password = '';
  error = '';
  registered = false;
  @Output() loggedIn = new EventEmitter<void>();
  @Output() closed = new EventEmitter<void>();

  constructor(private auth: AuthService) {}

  onSubmit() {
    this.error = '';
    this.auth.login({ userName: this.userName, password: this.password }).subscribe({
      next: (res) => {
        localStorage.setItem('apiKey', res.apiKey);
        localStorage.setItem('userName', res.userName);
        this.loggedIn.emit();
        this.close();
      },
      error: () => {
        this.error = 'Login failed';
      },
    });
  }

  close() {
    this.closed.emit();
  }
}
