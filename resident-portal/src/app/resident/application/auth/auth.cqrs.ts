
import { inject, Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import {
  AuthSession,
  LoginCredentials,
  LoginResult
} from '../../domain/auth/auth-session';
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
        if (result.rol !== 'AdminConjunto') {
          this.session.clear();
          throw new Error(
            'Esta interfaz está reservada para administradores del conjunto.'
          );
        }

        // Todavía no hay JWT: el usuario debe elegir un conjunto.
        if (result.requiereSeleccionConjunto || !result.token) {
          return result;
        }

        const session: AuthSession = {
          token: result.token,
          nombreCompleto: result.nombreCompleto,
          rol: result.rol,
          conjuntoId: result.conjuntoId,
          actorId: result.residenteId
        };

        this.session.setSession(session);

        return result;
      })
    );
  }
}
