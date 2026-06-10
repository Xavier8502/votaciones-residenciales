import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';

import { AuthSessionService } from '../auth/auth-session.service';
import { appRuntimeConfig } from '../config/app-runtime-config';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthSessionService);
  const token = auth.token();
  const shouldPrefixApi = req.url.startsWith('/api') && !!appRuntimeConfig.apiBaseUrl;
  const resolvedRequest = shouldPrefixApi
    ? req.clone({ url: `${appRuntimeConfig.apiBaseUrl}${req.url}` })
    : req;

  if (
    !token
    || !resolvedRequest.url.includes('/api')
    || resolvedRequest.url.includes('/api/auth/login')
  ) {
    return next(resolvedRequest);
  }

  return next(
    resolvedRequest.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    })
  );
};
