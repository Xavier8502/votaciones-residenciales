
import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import {
  LoginCredentials,
  LoginResult
} from '../../domain/auth/auth-session';
import { AuthRepository } from '../../domain/repositories/auth.repository';

@Injectable()
export class HttpAuthRepository extends AuthRepository {
  private readonly http = inject(HttpClient);

  login(credentials: LoginCredentials): Promise<LoginResult> {
    return firstValueFrom(
      this.http.post<LoginResult>('/api/auth/login', credentials)
    );
  }
}
