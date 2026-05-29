import { inject, Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { AuthSession, LoginCredentials } from '../../domain/auth/auth-session';
import { AuthRepository } from '../../domain/repositories/auth.repository';
import { Command, CommandHandler } from '../cqrs/command-bus';

export class LoginCommand implements Command<AuthSession> {
  readonly type = '[Auth] Login';

  constructor(public readonly credentials: LoginCredentials) {}
}

@Injectable({ providedIn: 'root' })
export class LoginCommandHandler implements CommandHandler<LoginCommand, AuthSession> {
  private readonly authRepository = inject(AuthRepository);
  private readonly session = inject(AuthSessionService);

  execute(command: LoginCommand): Observable<AuthSession> {
    return from(this.authRepository.login(command.credentials)).pipe(
      map((result) => {
        if (result.rol !== 'AdminConjunto') {
          this.session.clear();
          throw new Error('Esta interfaz está reservada para administradores del conjunto.');
        }

        this.session.setSession(result);
        return result;
      })
    );
  }
}
