
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
  // {
  //   path: 'dashboard',
  //   loadComponent: () => import('./dashboard/dashboard.component').then(m => m.DashboardComponent)
  // },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
];


