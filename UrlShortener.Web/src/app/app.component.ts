import { signature } from './copyright';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginModalComponent } from './login-modal/login-modal.component';
import { RegisterModalComponent } from './register-modal/register-modal.component';

@Component({
  standalone: true,
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  imports: [CommonModule, RouterModule, LoginModalComponent, RegisterModalComponent],
})
export class AppComponent {
  signatureValue = signature;
  showLoginModal = false;
  showRegisterModal = false;
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


