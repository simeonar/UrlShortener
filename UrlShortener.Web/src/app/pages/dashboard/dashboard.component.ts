// TODO: For dev mode, short links should be proxied to backend (e.g., via nginx or custom dev proxy).
// TODO: For dev mode, short links should be proxied to backend (e.g., via nginx or custom dev proxy).
// Now, short links are generated for backend port directly using current host. Implement proper proxy for seamless UX later.

// Backend port for dev mode. Change if backend runs on another port.
const BACKEND_PORT = 5212;
import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, FormsModule, HttpClientModule],
  template: `
    <div class="container py-5">
      <h2>My Links</h2>
      <div *ngIf="error && error === 'Not authenticated.'">
        <div class="alert alert-danger">{{ error }}</div>
        <!-- Guest search form -->
        <div class="card mt-4" style="max-width:400px;">
          <div class="card-body">
            <h5 class="card-title">Find Link by Short Code</h5>
            <form (ngSubmit)="onGuestSearch()" #guestForm="ngForm">
              <div class="mb-3">
                <input type="text" class="form-control" [(ngModel)]="guestShortCode" name="guestShortCode" placeholder="Enter short code" required />
              </div>
              <button type="submit" class="btn btn-primary w-100" [disabled]="!guestShortCode">Find</button>
            </form>
            <div *ngIf="guestResult" class="mt-3">
              <div *ngIf="guestResult.shortUrl; else notFound">
                <div class="alert alert-success">
                  <strong>Short URL:</strong> <a [href]="guestResult.shortUrl" target="_blank">{{ guestResult.shortUrl }}</a><br>
                  <strong>Original URL:</strong> {{ guestResult.originalUrl }}
                </div>
              </div>
              <ng-template #notFound>
                <div class="alert alert-warning">Not found or error.</div>
              </ng-template>
            </div>
          </div>
        </div>
      </div>
      <div *ngIf="!error">
        <div *ngIf="!links">Loading...</div>
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
                <button class="btn btn-outline-info btn-sm me-1" (click)="stats(link.shortCode)">Stats</button>
                <button class="btn btn-outline-dark btn-sm" (click)="showQr(link)">QR</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <!-- QR Modal -->
      <div class="modal fade show" tabindex="-1" [ngStyle]="{display: showQrModal ? 'block' : 'none', background: 'rgba(0,0,0,0.5)'}" *ngIf="showQrModal">
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title">QR Code</h5>
              <button type="button" class="btn-close" (click)="closeQrModal()"></button>
            </div>
            <div class="modal-body text-center">
              <img [src]="getQrDataUrl(qrUrl)" alt="QR Code" />
              <div class="mt-2"><a [href]="qrUrl" target="_blank">{{ qrUrl }}</a></div>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" (click)="closeQrModal()">Close</button>
            </div>
          </div>
        </div>
      </div>
      <!-- End QR Modal -->
    </div>
  `,
  styles: [``]
})
export class DashboardComponent implements OnInit {
  getQrDataUrl(url: string): string {
    if (!url || !this.links) return '';
    const link = this.links.find(l => l.shortUrl === url);
    return link?.qrDataUrl || '';
  }
  links: any[] | null = null;
  error: string | null = null;
  copyMessage: string | null = null;
  guestShortCode: string = '';
  guestResult: any = null;
  private http = inject(HttpClient);
  private router = inject(Router);

  showQrModal: boolean = false;
  qrUrl: string = '';

  showQr(link: any) {
    this.qrUrl = link.shortUrl || '';
    this.showQrModal = true;
  }

  closeQrModal() {
    this.showQrModal = false;
    this.qrUrl = '';
  }

  onGuestSearch() {
    this.guestResult = null;
    if (!this.guestShortCode) return;
    this.http.get<any>(`/api/urls/${this.guestShortCode}`).subscribe({
      next: (res) => {
        this.guestResult = res;
      },
      error: () => {
        this.guestResult = {};
      }
    });
  }

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
        const host = window.location.hostname;
        this.links = res.map(link => {
          // shortUrl
          if (!link.shortUrl && link.shortCode) {
            link.shortUrl = `http://${host}:${BACKEND_PORT}/` + link.shortCode;
          }
          // qrDataUrl
          link.qrDataUrl = `https://api.qrserver.com/v1/create-qr-code/?size=200x200&data=${encodeURIComponent(link.shortUrl)}`;
          return link;
        });
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

  encodeURIComponentUrl(url: string): string {
    return encodeURIComponent(url);
  }
}
