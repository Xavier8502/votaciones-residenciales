import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';

import { AuthSessionService } from '../auth/auth-session.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthSessionService);
  const token = auth.token();

  if (!token || !req.url.includes('/api') || req.url.includes('/api/auth/login')) {
    return next(req);
  }

  return next(
    req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    })
  );
};
