import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  imports: [CommonModule, FormsModule],
})
export class LoginComponent {
  userName = '';
  password = '';
  error = '';

  registered = false;
  constructor(private auth: AuthService, private router: Router) {
    const nav = this.router.getCurrentNavigation();
    this.registered = !!nav?.extras?.state?.['registered'];
  }

  onSubmit() {
    this.error = '';
    this.auth.login({ userName: this.userName, password: this.password }).subscribe({
      next: (res) => {
        localStorage.setItem('apiKey', res.apiKey);
        localStorage.setItem('userName', res.userName);
        this.router.navigate(['/dashboard']); // Navigate to dashboard after successful login
      },
      error: (err) => {
        this.error = 'Login failed';
      },
    });
  }
}
