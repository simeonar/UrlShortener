import { Component, inject, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import Chart from 'chart.js/auto';

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
        <canvas #chartCanvas width="400" height="200"></canvas>
      </div>
    </div>
  `,
  styles: [``]
})
export class DashboardComponent implements AfterViewInit {
  form = inject(FormBuilder).group({
    shortCode: ['', Validators.required]
  });
  loading = false;
  error: string | null = null;
  stats: any = null;
  private http = inject(HttpClient);
  @ViewChild('chartCanvas') chartCanvas?: ElementRef<HTMLCanvasElement>;
  chart: Chart | null = null;

  ngAfterViewInit() {
    // Chart will be rendered after stats loaded
  }

  onSubmit() {
    if (this.form.invalid) return;
    this.loading = true;
    this.error = null;
    this.stats = null;
    if (this.chart) {
      this.chart.destroy();
      this.chart = null;
    }
    const code = this.form.value.shortCode;
    this.http.get<any>(`/api/stats/${code}`).subscribe({
      next: (res) => {
        this.loading = false;
        this.stats = res;
        setTimeout(() => this.renderChart(), 0);
      },
      error: (err) => {
        this.loading = false;
        this.error = err?.error?.message || 'Failed to load stats.';
      }
    });
  }

  renderChart() {
    if (!this.chartCanvas || !this.stats?.byDay) return;
    const ctx = this.chartCanvas.nativeElement.getContext('2d');
    if (!ctx) return;
    const labels = this.stats.byDay.map((d: any) => d.date);
    const data = this.stats.byDay.map((d: any) => d.count);
    this.chart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels,
        datasets: [{
          label: 'Clicks by day',
          data,
          backgroundColor: 'rgba(54, 162, 235, 0.5)',
          borderColor: 'rgba(54, 162, 235, 1)',
          borderWidth: 1
        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: { display: false },
        },
        scales: {
          y: { beginAtZero: true }
        }
      }
    });
  }
}
