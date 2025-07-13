// TODO: For dev mode, short links should be proxied to backend (e.g., via nginx or custom dev proxy).
// Now, short links are generated for backend port directly using current host. Implement proper proxy for seamless UX later.

// Backend port for dev mode. Change if backend runs on another port.
const BACKEND_PORT = 5212;
import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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
      <div *ngIf="copyMessage" class="alert alert-success" style="position:fixed;top:20px;right:20px;z-index:9999;min-width:200px;">{{ copyMessage }}</div>
      <table *ngIf="links" class="table table-bordered">
        <thead>
          <tr>
            <th>Short URL</th>
            <th>Short Code</th>
            <th>Original URL</th>
            <th>Created</th>
            <th>Clicks</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let link of links">
            <td><a [href]="link.shortUrl" target="_blank">{{ link.shortUrl }}</a></td>
            <td>{{ link.shortCode }}</td>
            <td>{{ link.originalUrl }}</td>
            <td>{{ link.createdAt | date:'medium' }}</td>
            <td>{{ link.clicksCount }}</td>
            <td>
              <button class="btn btn-outline-secondary btn-sm me-1" (click)="copy(link)">Copy</button>
              <button class="btn btn-outline-danger btn-sm me-1" (click)="delete(link.shortCode)">Delete</button>
              <button class="btn btn-outline-info btn-sm" (click)="stats(link.shortCode)">Stats</button>
            </td>
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
  copyMessage: string | null = null;
  private http = inject(HttpClient);
  private router = inject(Router);

  ngOnInit() {
    let apiKey: string | null = null;
    if (typeof window !== 'undefined' && window.localStorage) {
      apiKey = localStorage.getItem('apiKey');
    }
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

  copy(link: any) {
    let url = link.shortUrl;
    if (!url && link.shortCode) {
      // In dev mode, use backend port directly for short links. See TODO above.
      const host = window.location.hostname;
      url = `http://${host}:${BACKEND_PORT}/` + link.shortCode;
    }
    if (url) {
      if (navigator.clipboard) {
        navigator.clipboard.writeText(url).then(() => {
          this.showCopyMessage();
        }, () => {
          this.showCopyMessage('Failed to copy');
        });
      } else {
        const textarea = document.createElement('textarea');
        textarea.value = url;
        document.body.appendChild(textarea);
        textarea.select();
        const success = document.execCommand('copy');
        document.body.removeChild(textarea);
        this.showCopyMessage(success ? undefined : 'Failed to copy');
      }
    }
  }

  showCopyMessage(msg?: string) {
    this.copyMessage = msg || 'Copied to clipboard!';
    setTimeout(() => {
      this.copyMessage = null;
    }, 1500);
  }

  delete(shortCode: string) {
    const apiKey = localStorage.getItem('apiKey');
    if (!apiKey) return;
    if (!confirm('Are you sure you want to delete this link?')) return;
    this.http.delete(`/api/userlinks/${shortCode}`, {
      headers: new HttpHeaders({ 'X-Api-Key': apiKey })
    }).subscribe({
      next: () => {
        this.links = this.links?.filter(l => l.shortCode !== shortCode) || null;
      },
      error: (err) => {
        alert(err?.error?.message || 'Failed to delete link.');
      }
    });
  }

  stats(shortCode: string) {
    this.router.navigate([`/stats/${shortCode}`]);
  }
}
