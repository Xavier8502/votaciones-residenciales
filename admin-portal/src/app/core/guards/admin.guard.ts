import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { AuthSessionService } from '../auth/auth-session.service';

export const adminGuard: CanActivateFn = () => {
  const auth = inject(AuthSessionService);
  const router = inject(Router);

  if (auth.isAdmin()) {
    return true;
  }

  auth.clear();
  return router.createUrlTree(['/login']);
};
