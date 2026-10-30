import { inject, Injectable } from '@angular/core';
import { from, Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { AuthSession, LoginCredentials } from '../../domain/auth/auth-session';
import { AuthRepository } from '../../domain/repositories/auth.repository';
import { Command, CommandHandler } from '../cqrs/command-bus';

export class LoginCommand implements Command<AuthSession> {
  readonly type = '[Auth] Login';

  constructor(public readonly credentials: LoginCredentials) {}
}

@Injectable({ providedIn: 'root' })
export class LoginCommandHandler
  implements CommandHandler<LoginCommand, AuthSession> {
  private readonly authRepository = inject(AuthRepository);
  private readonly session = inject(AuthSessionService);

  execute(command: LoginCommand): Observable<AuthSession> {
    return from(this.authRepository.login(command.credentials)).pipe(
      map((result) => {
        if (!result.token) {
          throw new Error('La API no devolvió un token de autenticación.');
        }

        if (!['AdminConjunto', 'SuperAdmin'].includes(result.rol)) {
          throw new Error('Este usuario no tiene acceso al portal administrativo.');
        }

        return result;
      }),
      tap((result) => this.session.setSession(result)),
      catchError((error) => {
        this.session.clear();
        return throwError(() => error);
      })
    );
  }
}
