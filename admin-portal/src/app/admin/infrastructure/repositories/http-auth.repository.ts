import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import {
  LoginCredentials,
  LoginResult
} from '../../domain/auth/auth-session';
import { AuthRepository } from '../../domain/repositories/auth.repository';

interface LoginResponseDto {
  token: string | null;
  nombreCompleto: string;
  rol: string;
  conjuntoId: string | null;
  residenteId: string;
  requiereSeleccionConjunto: boolean;
  conjuntos: {
    id: string;
    nombre: string;
  }[];
}

@Injectable()
export class HttpAuthRepository extends AuthRepository {
  private readonly http = inject(HttpClient);

  login(credentials: LoginCredentials): Promise<LoginResult> {
    return firstValueFrom(
      this.http.post<LoginResponseDto>('/api/auth/login', credentials).pipe(
        // Se conserva la respuesta completa, incluida la selección de conjunto.
        // El token se valida en Application.
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        (source) => source
      )
    ).then((response) => ({
      token: response.token ?? '',
      nombreCompleto: response.nombreCompleto,
      rol: response.rol as LoginResult['rol'],
      conjuntoId: response.conjuntoId ?? '',
      actorId: response.residenteId,
      requiereSeleccionConjunto: response.requiereSeleccionConjunto,
      conjuntos: response.conjuntos ?? []
    }));
  }
}