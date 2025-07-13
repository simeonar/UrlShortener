import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-stats',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  template: `
    <div class="container py-5">
      <h2>Link Statistics</h2>
      <div *ngIf="loading">Loading...</div>
      <div *ngIf="error" class="alert alert-danger">{{ error }}</div>
      <div *ngIf="stats">
        <div class="mb-3">
          <b>Short code:</b> {{ stats.shortCode }}<br>
          <b>Total clicks:</b> {{ stats.totalClicks }}<br>
          <b>Last click:</b> {{ stats.lastClick ? (stats.lastClick | date:'medium') : 'Never' }}
        </div>
        <div *ngIf="stats.byDay && stats.byDay.length">
          <h5>Clicks by day</h5>
          <table class="table table-sm table-bordered w-auto">
            <thead>
              <tr><th>Date</th><th>Clicks</th></tr>
            </thead>
            <tbody>
              <tr *ngFor="let d of stats.byDay">
                <td>{{ d.date }}</td>
                <td>{{ d.count }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `,
  styles: [``]
})
export class StatsComponent {
  stats: any = null;
  error: string | null = null;
  loading = true;

  constructor(private route: ActivatedRoute, private http: HttpClient) {
    const shortCode = this.route.snapshot.paramMap.get('shortCode');
    if (shortCode) {
      this.http.get(`/api/stats/${shortCode}`).subscribe({
        next: (res) => {
          this.stats = res;
          this.loading = false;
        },
        error: (err) => {
          this.error = err?.error?.message || 'Failed to load stats.';
          this.loading = false;
        }
      });
    } else {
      this.error = 'No shortCode provided.';
      this.loading = false;
    }
  }
}
