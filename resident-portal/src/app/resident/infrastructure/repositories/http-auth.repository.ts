import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { AuthSession } from '../../domain/auth/auth-session';
import { AuthRepository, LoginDraft } from '../../domain/repositories/auth.repository';

@Injectable()
export class HttpAuthRepository implements AuthRepository {
  private readonly http = inject(HttpClient);

  login(draft: LoginDraft) {
    return this.http.post<AuthSession>('/api/auth/login', draft);
  }
}
