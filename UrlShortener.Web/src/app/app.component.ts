import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginModalComponent } from './login-modal/login-modal.component';

@Component({
  standalone: true,
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  imports: [CommonModule, RouterModule, LoginModalComponent],
})
export class AppComponent {
  showLoginModal = false;
  get userName(): string | null {
    if (typeof window !== 'undefined' && window.localStorage) {
      return localStorage.getItem('userName');
    }
    return null;
  }
  openLogin() {
    this.showLoginModal = true;
  }
  onLoginModalClosed() {
    this.showLoginModal = false;
  }
  onLoggedIn() {
    this.showLoginModal = false;
  }
  logout() {
    if (typeof window !== 'undefined' && window.localStorage) {
      localStorage.removeItem('apiKey');
      localStorage.removeItem('userName');
      window.location.href = '/login';
    }
  }
}


