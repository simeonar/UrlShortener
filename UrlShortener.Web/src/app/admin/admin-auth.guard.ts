import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const adminAuthGuard: CanActivateFn = (route, state) => {
  if (typeof window !== 'undefined') {
    const isAdmin = localStorage.getItem('admin_logged_in') === 'true';
    if (isAdmin) {
      return true;
    } else {
      return inject(Router).createUrlTree(['/admin-login']);
    }
  }
  // SSR: allow navigation to avoid ReferenceError
  return true;
};
