import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const adminAuthGuard: CanActivateFn = (route, state) => {
  // Simple check: admin credentials in localStorage
  const isAdmin = localStorage.getItem('admin_logged_in') === 'true';
  if (isAdmin) {
    return true;
  } else {
    // Redirect to admin login (can be /admin?login or /login)
    return inject(Router).createUrlTree(['/admin-login']);
  }
};
