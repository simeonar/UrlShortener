import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  imports: [CommonModule, RouterModule],
})
export class AppComponent {
  get userName(): string | null {
    if (typeof window !== 'undefined' && window.localStorage) {
      return localStorage.getItem('userName');
    }
    return null;
  }
  logout() {
    if (typeof window !== 'undefined' && window.localStorage) {
      localStorage.removeItem('apiKey');
      localStorage.removeItem('userName');
      window.location.href = '/login';
    }
  }
}


