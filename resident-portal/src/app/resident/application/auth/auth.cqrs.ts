import { inject, Injectable } from '@angular/core';
import { tap } from 'rxjs';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { AuthSession } from '../../domain/auth/auth-session';
import { AuthRepository, LoginDraft } from '../../domain/repositories/auth.repository';
import { Command, CommandHandler } from '../cqrs/command-bus';

export class LoginCommand implements Command<AuthSession> {
  readonly type = '[Auth] Login';

  constructor(readonly draft: LoginDraft) {}
}

@Injectable({ providedIn: 'root' })
export class LoginCommandHandler implements CommandHandler<LoginCommand, AuthSession> {
  private readonly authRepository = inject(AuthRepository);
  private readonly session = inject(AuthSessionService);

  execute(command: LoginCommand) {
    return this.authRepository.login(command.draft).pipe(
      tap((result) => {
        if (result.rol !== 'Residente' && result.rol !== 'Propietario') {
          throw new Error('Este portal solo permite acceso a residentes y propietarios.');
        }

        this.session.setSession(result);
      })
    );
  }
}
