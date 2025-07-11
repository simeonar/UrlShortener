import { Component } from '@angular/core';

@Component({
  selector: 'app-result',
  standalone: true,
  template: `
    <div class="container py-5">
      <h2>Shortened URL Result</h2>
      <!-- QR code and result info will be here -->
    </div>
  `,
  styles: [``]
})
export class ResultComponent {}
