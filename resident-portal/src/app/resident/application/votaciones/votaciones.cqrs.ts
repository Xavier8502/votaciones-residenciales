import { inject, Injectable } from '@angular/core';

import { VotacionRepository } from '../../domain/repositories/votacion.repository';
import {
  EmitirVotoDraft,
  ResultadosVotacion,
  VotacionDetalle,
  VotacionResumen
} from '../../domain/votaciones/votacion';
import { Command, CommandHandler } from '../cqrs/command-bus';
import { Query, QueryHandler } from '../cqrs/query-bus';

export class GetVotacionesQuery implements Query<VotacionResumen[]> {
  readonly type = '[Votaciones] Get By Conjunto';

  constructor(readonly conjuntoId: string) {}
}

export class GetVotacionDetailQuery implements Query<VotacionDetalle> {
  readonly type = '[Votaciones] Get Detail';

  constructor(readonly id: string) {}
}

export class GetVotacionResultadosQuery implements Query<ResultadosVotacion> {
  readonly type = '[Votaciones] Get Resultados';

  constructor(readonly id: string) {}
}

export class EmitVotoCommand implements Command<{ id: string }> {
  readonly type = '[Votaciones] Emitir Voto';

  constructor(
    readonly votacionId: string,
    readonly draft: EmitirVotoDraft
  ) {}
}

@Injectable({ providedIn: 'root' })
export class GetVotacionesQueryHandler implements QueryHandler<GetVotacionesQuery, VotacionResumen[]> {
  private readonly repository = inject(VotacionRepository);

  execute(query: GetVotacionesQuery) {
    return this.repository.getByConjunto(query.conjuntoId);
  }
}

@Injectable({ providedIn: 'root' })
export class GetVotacionDetailQueryHandler implements QueryHandler<GetVotacionDetailQuery, VotacionDetalle> {
  private readonly repository = inject(VotacionRepository);

  execute(query: GetVotacionDetailQuery) {
    return this.repository.getDetail(query.id);
  }
}

@Injectable({ providedIn: 'root' })
export class GetVotacionResultadosQueryHandler
  implements QueryHandler<GetVotacionResultadosQuery, ResultadosVotacion>
{
  private readonly repository = inject(VotacionRepository);

  execute(query: GetVotacionResultadosQuery) {
    return this.repository.getResultados(query.id);
  }
}

@Injectable({ providedIn: 'root' })
export class EmitVotoCommandHandler implements CommandHandler<EmitVotoCommand, { id: string }> {
  private readonly repository = inject(VotacionRepository);

  execute(command: EmitVotoCommand) {
    return this.repository.emitirVoto(command.votacionId, command.draft);
  }
}
