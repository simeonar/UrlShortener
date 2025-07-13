import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  imports: [CommonModule, FormsModule],
})
export class RegisterComponent {
  userName = '';
  password = '';
  error = '';

  constructor(private auth: AuthService, private router: Router) {}

  onSubmit() {
    this.error = '';
    this.auth.register({ userName: this.userName, password: this.password }).subscribe({
      next: (res) => {
        localStorage.setItem('apiKey', res.apiKey);
        this.router.navigate(['/login'], { state: { registered: true } });
      },
      error: (err) => {
        this.error = 'Registration failed';
      },
    });
  }
}
