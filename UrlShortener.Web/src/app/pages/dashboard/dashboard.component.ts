import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  template: `
    <div class="container py-5">
      <h2>Statistics Dashboard</h2>
      <!-- Chart.js stats will be here -->
    </div>
  `,
  styles: [``]
})
export class DashboardComponent {}
