import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-login.component.html',
  styleUrls: ['./admin-login.component.scss']
})
export class AdminLoginComponent {
  username = '';
  password = '';
  error = '';

  constructor(private router: Router) {}

  async login() {
    if (this.username === 'admin' && this.password === 'admin') {
      // Try to login via API and get token
      try {
        let resp = await fetch('/api/auth/login', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ email: 'admin@example.com', password: 'Admin123!@#' })
        });
        if (resp.status === 401) {
          // Try to register admin if not exists
          await fetch('/api/auth/register', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email: 'admin@example.com', password: 'Admin123!@#', isAdmin: true })
          });
          // Try login again
          resp = await fetch('/api/auth/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email: 'admin@example.com', password: 'Admin123!@#' })
          });
        }
        if (!resp.ok) throw new Error('API login failed');
        const data = await resp.json();
        localStorage.setItem('admin_logged_in', 'true');
        localStorage.setItem('admin_token', data.token);
        this.router.navigate(['/admin']);
      } catch (e: any) {
        this.error = 'API login failed: ' + (e.message || e);
      }
    } else {
      this.error = 'Invalid credentials';
    }
  }
}
