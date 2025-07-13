import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-manage',
  standalone: true,
  imports: [ReactiveFormsModule, HttpClientModule, CommonModule],
  template: `
    <div class="container py-5">
      <h2>Manage Links</h2>
      <form [formGroup]="form" (ngSubmit)="onSubmit()" class="mb-4">
        <div class="row g-2 align-items-end">
          <div class="col-auto">
            <label for="shortCode" class="form-label">Short code</label>
            <input type="text" class="form-control" id="shortCode" formControlName="shortCode" placeholder="abc123">
          </div>
          <div class="col-auto">
            <button type="submit" class="btn btn-primary" [disabled]="form.invalid || loading">Find</button>
          </div>
        </div>
      </form>
      <div *ngIf="error" class="alert alert-danger">{{ error }}</div>
      <div *ngIf="urlInfo">
        <h5>Short URL: <a [href]="urlInfo.shortUrl" target="_blank">{{ urlInfo.shortUrl }}</a></h5>
        <div><strong>Short code:</strong> {{ urlInfo.shortCode }}</div>
        <div>Original URL: {{ urlInfo.originalUrl }}</div>
        <div>Created: {{ urlInfo.createdAt | date:'medium' }}</div>
        <div>Status: <span [ngClass]="{'text-success': urlInfo.active, 'text-danger': !urlInfo.active}">{{ urlInfo.active ? 'Active' : 'Inactive' }}</span></div>
        <!-- Здесь можно добавить кнопки для управления -->
      </div>
    </div>
  `,
  styles: [``]
})
export class ManageComponent {
  form = inject(FormBuilder).group({
    shortCode: ['', Validators.required]
  });
  loading = false;
  error: string | null = null;
  urlInfo: any = null;
  private http = inject(HttpClient);

  onSubmit() {
    if (this.form.invalid) return;
    this.loading = true;
    this.error = null;
    this.urlInfo = null;
    const code = this.form.value.shortCode;
    this.http.get<any>(`/api/urls/${code}`).subscribe({
      next: (res) => {
        this.loading = false;
        this.urlInfo = res;
      },
      error: (err) => {
        this.loading = false;
        this.error = err?.error?.message || 'Not found or error.';
      }
    });
  }
}
