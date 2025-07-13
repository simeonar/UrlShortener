import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface AdminStats {
  totalLinks: number;
  totalUsers: number;
  totalClicks: number;
}

@Component({
  selector: 'app-admin-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.scss']
})
export class AdminPageComponent {
  links: any[] = [];
  users: any[] = [];
  blockedLinks: any[] = [];
  stats: AdminStats|null = null;
  error = '';

  async showLinks() {
    this.error = '';
    try {
      const resp = await fetch('/api/admin/links', {
        headers: { 'X-Api-Key': 'admin-secret-key' }
      });
      if (!resp.ok) throw new Error('Failed to load links');
      this.links = await resp.json();
    } catch (e: any) {
      this.error = e.message;
    }
  }

  async showUsers() {
    this.error = '';
    try {
      const resp = await fetch('/api/admin/users', {
        headers: { 'X-Api-Key': 'admin-secret-key' }
      });
      if (!resp.ok) throw new Error('Failed to load users');
      this.users = await resp.json();
    } catch (e: any) {
      this.error = e.message;
    }
  }

  async showStats() {
    this.error = '';
    try {
      const resp = await fetch('/api/admin/stats', {
        headers: { 'X-Api-Key': 'admin-secret-key' }
      });
      if (!resp.ok) throw new Error('Failed to load stats');
      this.stats = await resp.json();
    } catch (e: any) {
      this.error = e.message;
    }
  }

  async showBlockedLinks() {
    this.error = '';
    try {
      const resp = await fetch('/api/admin/links/blocked', {
        headers: { 'X-Api-Key': 'admin-secret-key' }
      });
      if (!resp.ok) throw new Error('Failed to load blocked links');
      this.blockedLinks = await resp.json();
    } catch (e: any) {
      this.error = e.message;
    }
  }

  async blockLink(shortCode: string) {
    this.error = '';
    try {
      const resp = await fetch(`/api/admin/links/${shortCode}/block`, {
        method: 'POST',
        headers: { 'X-Api-Key': 'admin-secret-key' }
      });
      if (!resp.ok) throw new Error('Failed to block link');
      await this.showLinks();
      await this.showBlockedLinks();
    } catch (e: any) {
      this.error = e.message;
    }
  }

  async unblockLink(shortCode: string) {
    this.error = '';
    try {
      const resp = await fetch(`/api/admin/links/${shortCode}/unblock`, {
        method: 'POST',
        headers: { 'X-Api-Key': 'admin-secret-key' }
      });
      if (!resp.ok) throw new Error('Failed to unblock link');
      await this.showLinks();
      await this.showBlockedLinks();
    } catch (e: any) {
      this.error = e.message;
    }
  }

  logout() {
    localStorage.removeItem('admin_logged_in');
    localStorage.removeItem('admin_token');
    window.location.href = '/admin-login';
  }
}
