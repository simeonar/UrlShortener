import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, HttpClientModule],
  template: `
    <div class="container py-5">
      <h2>My Links</h2>
      <div *ngIf="error" class="alert alert-danger">{{ error }}</div>
      <div *ngIf="!links && !error">Loading...</div>
      <table *ngIf="links" class="table table-bordered">
        <thead>
          <tr>
            <th>Short URL</th>
            <th>Short Code</th>
            <th>Original URL</th>
            <th>Created</th>
            <th>Clicks</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let link of links">
            <td><a [href]="link.shortUrl" target="_blank">{{ link.shortUrl }}</a></td>
            <td>{{ link.shortCode }}</td>
            <td>{{ link.originalUrl }}</td>
            <td>{{ link.createdAt | date:'medium' }}</td>
            <td>{{ link.clicksCount }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  `,
  styles: [``]
})
export class DashboardComponent implements OnInit {
  links: any[] | null = null;
  error: string | null = null;
  private http = inject(HttpClient);
  ngOnInit() {
    const apiKey = localStorage.getItem('apiKey');
    if (!apiKey) {
      this.error = 'Not authenticated.';
      return;
    }
    this.http.get<any[]>('/api/userlinks', {
      headers: new HttpHeaders({ 'X-Api-Key': apiKey })
    }).subscribe({
      next: (res) => {
        this.links = res;
      },
      error: (err) => {
        this.error = err?.error?.message || 'Failed to load links.';
      }
    });
  }
}
