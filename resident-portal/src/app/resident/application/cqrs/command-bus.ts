import { inject, Injectable, Injector, Type } from '@angular/core';
import { Observable, throwError } from 'rxjs';

import { COMMAND_HANDLER_REGISTRY } from './handler-registry';

export interface Command<TResult = unknown> {
  readonly type: string;
}

export interface CommandHandler<TCommand extends Command<TResult>, TResult> {
  execute(command: TCommand): Observable<TResult>;
}

@Injectable({ providedIn: 'root' })
export class CommandBus {
  private readonly injector = inject(Injector);

  execute<TResult>(command: Command<TResult>): Observable<TResult> {
    const token = COMMAND_HANDLER_REGISTRY[command.type] as
      | Type<CommandHandler<Command<TResult>, TResult>>
      | undefined;

    if (!token) {
      return throwError(() => new Error(`No existe un CommandHandler para ${command.type}.`));
    }

    return this.injector.get(token).execute(command);
  }
}
