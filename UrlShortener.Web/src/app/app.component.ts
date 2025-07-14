import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginModalComponent } from './login-modal/login-modal.component';
import { RegisterModalComponent } from './register-modal/register-modal.component';

import { HttpClient } from '@angular/common/http';
import { OnInit } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  imports: [CommonModule, RouterModule, LoginModalComponent, RegisterModalComponent],
})
export class AppComponent implements OnInit {
  signatureValue = '';
  showLoginModal = false;
  showRegisterModal = false;
  copyVisible = false;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get('/branding.properties', { responseType: 'text' }).subscribe(data => {
      const props = this.parseProperties(data);
      this.copyVisible = props['copy.visible'] === 'true';
      this.signatureValue = `© ${props['author.name'] ?? ''} ${props['author.surname'] ?? ''}`.trim();
    });
  }

  private parseProperties(data: string): Record<string, string> {
    const result: Record<string, string> = {};
    data.split('\n').forEach(line => {
      const trimmed = line.trim();
      if (!trimmed || trimmed.startsWith('#')) return;
      const idx = trimmed.indexOf('=');
      if (idx > 0) {
        const key = trimmed.substring(0, idx).trim();
        const value = trimmed.substring(idx + 1).trim();
        result[key] = value;
      }
    });
    return result;
  }

  get userName(): string | null {
    if (typeof window !== 'undefined' && window.localStorage) {
      return localStorage.getItem('userName');
    }
    return null;
  }
  openLogin() {
    this.showLoginModal = true;
  }
  openRegister() {
    this.showRegisterModal = true;
  }
  onLoginModalClosed() {
    this.showLoginModal = false;
  }
  onRegisterModalClosed() {
    this.showRegisterModal = false;
  }
  onLoggedIn() {
    this.showLoginModal = false;
  }
  onRegistered() {
    this.showRegisterModal = false;
    // Можно показать уведомление или сразу открыть логин
  }
  logout() {
    if (typeof window !== 'undefined' && window.localStorage) {
      localStorage.removeItem('apiKey');
      localStorage.removeItem('userName');
      window.location.href = '/home';
    }
  }
}


