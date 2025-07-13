
import { Routes } from '@angular/router';

export const appRoutes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./pages/dashboard/dashboard.component').then(m => m.DashboardComponent)
  },
  {
    path: 'home',
    loadComponent: () => import('./pages/home/home.component').then(m => m.HomeComponent)
  },
  {
    path: 'result',
    loadComponent: () => import('./pages/result/result.component').then(m => m.ResultComponent)
  },
  {
    path: 'manage',
    loadComponent: () => import('./pages/manage/manage.component').then(m => m.ManageComponent)
  },
  {
    path: 'stats/:shortCode',
    loadComponent: () => import('./pages/stats/stats.component').then(m => m.StatsComponent)
  },
  // Default route: redirect authenticated users to home, otherwise to login
  { path: '', redirectTo: '/home', pathMatch: 'full' },
];


