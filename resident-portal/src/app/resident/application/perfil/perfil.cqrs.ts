import { inject, Injectable } from '@angular/core';

import { ChangePinDraft } from '../../domain/perfil/change-pin';
import { PerfilRepository } from '../../domain/repositories/perfil.repository';
import { Command, CommandHandler } from '../cqrs/command-bus';

export class UpdatePinCommand implements Command<void> {
  readonly type = '[Perfil] Update Pin';

  constructor(readonly draft: ChangePinDraft) {}
}

@Injectable({ providedIn: 'root' })
export class UpdatePinCommandHandler implements CommandHandler<UpdatePinCommand, void> {
  private readonly perfilRepository = inject(PerfilRepository);

  execute(command: UpdatePinCommand) {
    return this.perfilRepository.changePin(command.draft);
  }
}
