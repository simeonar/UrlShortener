import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  template: `
    <div class="container py-5">
      <h1>URL Shortener</h1>
      <form [formGroup]="form" (ngSubmit)="onSubmit()">
        <div class="mb-3">
          <label for="url" class="form-label">Enter URL to shorten</label>
          <input type="url" class="form-control" id="url" formControlName="url" placeholder="https://example.com">
          <div *ngIf="form.get('url')?.invalid && form.get('url')?.touched" class="text-danger small">
            Please enter a valid URL.
          </div>
        </div>
        <button type="submit" class="btn btn-primary" [disabled]="form.invalid || loading">Shorten</button>
      </form>
      <div *ngIf="error" class="alert alert-danger mt-3">{{ error }}</div>
    </div>
  `,
  styles: [``]
})
export class HomeComponent {
  form = inject(FormBuilder).group({
    url: ['', [Validators.required, Validators.pattern('https?://.+')]]
  });
  loading = false;
  error: string | null = null;
  private http = inject(HttpClient);
  private router = inject(Router);

  onSubmit() {
    if (this.form.invalid) return;
    this.loading = true;
    this.error = null;
    const apiKey = localStorage.getItem('apiKey');
    const options = apiKey ? { headers: { 'X-Api-Key': apiKey } } : {};
    this.http.post<any>('/api/urls/shorten', { originalUrl: this.form.value.url }, options).subscribe({
      next: (res) => {
        this.loading = false;
        this.router.navigate(['/result'], { state: { result: res } });
      },
      error: (err) => {
        this.loading = false;
        this.error = err?.error?.message || 'Failed to shorten URL.';
      }
    });
  }
}
