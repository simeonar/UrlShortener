import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';

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

  constructor(private auth: AuthService) {}

  onSubmit() {
    this.error = '';
    this.auth.login({ userName: this.userName, password: this.password }).subscribe({
      next: (res) => {
        localStorage.setItem('apiKey', res.apiKey);
        // redirect to dashboard or show success
      },
      error: (err) => {
        this.error = 'Login failed';
      },
    });
  }
}
