import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-result',
  imports: [CommonModule],
  template: `
    <div class="container py-5" *ngIf="result && result.shortUrl && result.shortCode; else noData">
      <h2>Shortened URL Result</h2>
      <div class="alert alert-success" *ngIf="result.shortUrl">
        <strong>Short URL:</strong>
        <a [href]="result.shortUrl" target="_blank">{{ result.shortUrl }}</a>
      </div>
      <div class="mb-3" *ngIf="result.originalUrl">
        <strong>Original URL: </strong>
        <span>{{ result.originalUrl }}</span>
      </div>
      <div class="mb-3" *ngIf="result.shortUrl">
        <img [src]="qrCodeUrl" alt="QR Code" width="200" height="200">
        <div class="mt-2 small text-muted">
          QR data: <span>{{ qrCodeUrl }}</span>
        </div>
      </div>
      <a (click)="onShortenAnother()" class="btn btn-secondary">Shorten another</a>
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
    if (this.result && this.result.shortUrl) {
      this.qrCodeUrl = `https://api.qrserver.com/v1/create-qr-code/?size=200x200&data=${encodeURIComponent(this.result.shortUrl)}`;
    }
  }

  onShortenAnother() {
    this.router.navigate(['/'], { state: {} });
  }
}
