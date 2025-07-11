import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./pages/home/home.component').then(m => m.HomeComponent) },
  { path: 'result', loadComponent: () => import('./pages/result/result.component').then(m => m.ResultComponent) },
  { path: 'dashboard', loadComponent: () => import('./pages/dashboard/dashboard.component').then(m => m.DashboardComponent) },
  { path: 'manage', loadComponent: () => import('./pages/manage/manage.component').then(m => m.ManageComponent) },
  { path: '**', redirectTo: '' }
];
