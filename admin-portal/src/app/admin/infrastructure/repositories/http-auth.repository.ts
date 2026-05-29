import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { map } from 'rxjs/operators';

import { AuthSession, LoginCredentials } from '../../domain/auth/auth-session';
import { AuthRepository } from '../../domain/repositories/auth.repository';

interface LoginResponseDto {
  token: string;
  nombreCompleto: string;
  rol: string;
  conjuntoId: string;
  residenteId: string;
}

@Injectable()
export class HttpAuthRepository extends AuthRepository {
  private readonly http = inject(HttpClient);

  login(credentials: LoginCredentials): Promise<AuthSession> {
    return firstValueFrom(
      this.http.post<LoginResponseDto>('/api/auth/login', credentials).pipe(
        map((response) => ({
          token: response.token,
          nombreCompleto: response.nombreCompleto,
          rol: response.rol,
          conjuntoId: response.conjuntoId,
          actorId: response.residenteId
        }))
      )
    );
  }
}
