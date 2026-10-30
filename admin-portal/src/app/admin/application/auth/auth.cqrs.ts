import { inject, Injectable } from '@angular/core';
import { from, Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { LoginCredentials, LoginResult } from '../../domain/auth/auth-session';
import { AuthRepository } from '../../domain/repositories/auth.repository';
import { Command, CommandHandler } from '../cqrs/command-bus';

export class LoginCommand implements Command<LoginResult> {
  readonly type = '[Auth] Login';

  constructor(public readonly credentials: LoginCredentials) {}
}

@Injectable({ providedIn: 'root' })
export class LoginCommandHandler
  implements CommandHandler<LoginCommand, LoginResult> {
  private readonly authRepository = inject(AuthRepository);
  private readonly session = inject(AuthSessionService);

  execute(command: LoginCommand): Observable<LoginResult> {
    return from(this.authRepository.login(command.credentials)).pipe(
      map((result) => {
        if (!['AdminConjunto', 'SuperAdmin'].includes(result.rol)) {
          throw new Error('Este usuario no tiene acceso al portal administrativo.');
        }

        if (!result.token && !result.requiereSeleccionConjunto) {
          throw new Error('La API no devolvió un token de autenticación.');
        }

        if (result.requiereSeleccionConjunto && !result.conjuntos.length) {
          throw new Error('No hay conjuntos disponibles para este administrador.');
        }

        return result;
      }),
      tap((result) => {
        if (result.token) {
          this.session.setSession({
            token: result.token,
            nombreCompleto: result.nombreCompleto,
            rol: result.rol,
            conjuntoId: result.conjuntoId,
            actorId: result.actorId
          });
        }
      }),
      catchError((error) => {
        this.session.clear();
        return throwError(() => error);
      })
    );
  }
}