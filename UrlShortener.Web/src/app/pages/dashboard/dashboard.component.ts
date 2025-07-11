import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [ReactiveFormsModule, HttpClientModule],
  template: `
    <div class="container py-5">
      <h2>Statistics Dashboard</h2>
      <form [formGroup]="form" (ngSubmit)="onSubmit()" class="mb-4">
        <div class="row g-2 align-items-end">
          <div class="col-auto">
            <label for="shortCode" class="form-label">Short code</label>
            <input type="text" class="form-control" id="shortCode" formControlName="shortCode" placeholder="abc123">
          </div>
          <div class="col-auto">
            <button type="submit" class="btn btn-primary" [disabled]="form.invalid || loading">Load stats</button>
          </div>
        </div>
      </form>
      <div *ngIf="error" class="alert alert-danger">{{ error }}</div>
      <div *ngIf="stats">
        <h5>Clicks: {{ stats.totalClicks }}</h5>
        <h6>By day:</h6>
        <ul>
          <li *ngFor="let d of stats.byDay">{{ d.date }}: {{ d.count }}</li>
        </ul>
        <!-- Chart.js chart placeholder -->
        <canvas id="chart" width="400" height="200"></canvas>
      </div>
    </div>
  `,
  styles: [``]
})
export class DashboardComponent {
  form = inject(FormBuilder).group({
    shortCode: ['', Validators.required]
  });
  loading = false;
  error: string | null = null;
  stats: any = null;
  private http = inject(HttpClient);

  onSubmit() {
    if (this.form.invalid) return;
    this.loading = true;
    this.error = null;
    this.stats = null;
    const code = this.form.value.shortCode;
    this.http.get<any>(`/api/stats/${code}`).subscribe({
      next: (res) => {
        this.loading = false;
        this.stats = res;
        // TODO: Chart.js render
      },
      error: (err) => {
        this.loading = false;
        this.error = err?.error?.message || 'Failed to load stats.';
      }
    });
  }
}
