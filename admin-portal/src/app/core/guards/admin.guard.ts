import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { AuthSessionService } from '../auth/auth-session.service';

export const adminGuard: CanActivateFn = () => {
  const auth = inject(AuthSessionService);
  const router = inject(Router);

  if (auth.isAdmin()) {
    return true;
  }

  if (auth.isSuperAdmin()) {
    return router.createUrlTree(['/superadmin/asignaciones']);
  }

  auth.clear();
  return router.createUrlTree(['/login']);
};
