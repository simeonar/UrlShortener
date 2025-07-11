import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-result',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container py-5" *ngIf="result; else noData">
      <h2>Shortened URL Result</h2>
      <div class="alert alert-success">
        <strong>Short URL:</strong>
        <a [href]="result.shortUrl" target="_blank">{{ result.shortUrl }}</a>
      </div>
      <div class="mb-3">
        <strong>Original URL:</strong>
        <span>{{ result.originalUrl }}</span>
      </div>
      <div class="mb-3">
        <img [src]="qrCodeUrl" alt="QR Code" width="200" height="200" *ngIf="qrCodeUrl">
      </div>
      <a routerLink="/" class="btn btn-secondary">Shorten another</a>
    </div>
    <ng-template #noData>
      <div class="container py-5">
        <div class="alert alert-warning">No result data. Please shorten a URL first.</div>
        <a routerLink="/" class="btn btn-primary">Go to Home</a>
      </div>
    </ng-template>
  `,
  styles: [``]
})
export class ResultComponent {
  result: any;
  qrCodeUrl: string | null = null;
  private router = inject(Router);

  constructor() {
    const nav = this.router.getCurrentNavigation();
    this.result = nav?.extras?.state?.['result'];
    if (this.result && this.result.shortCode) {
      this.qrCodeUrl = `/api/qr/${this.result.shortCode}?size=200&format=png`;
    }
  }
}
