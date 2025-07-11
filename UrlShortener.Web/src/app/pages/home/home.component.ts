import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  standalone: true,
  template: `
    <div class="container py-5">
      <h1>URL Shortener</h1>
      <form>
        <div class="mb-3">
          <label for="url" class="form-label">Enter URL to shorten</label>
          <input type="url" class="form-control" id="url" placeholder="https://example.com">
        </div>
        <button type="submit" class="btn btn-primary">Shorten</button>
      </form>
    </div>
  `,
  styles: [``]
})
export class HomeComponent {}
