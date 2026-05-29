import { inject, Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';

import { VotacionRepository } from '../../domain/repositories/votacion.repository';
import {
  ResultadosVotacion,
  VotacionDetalle,
  VotacionDraft,
  VotacionResumen
} from '../../domain/votaciones/votacion';
import { Command, CommandHandler } from '../cqrs/command-bus';
import { Query, QueryHandler } from '../cqrs/query-bus';

export class GetVotacionesQuery implements Query<VotacionResumen[]> {
  readonly type = '[Votaciones] Get By Conjunto';

  constructor(public readonly conjuntoId: string, public readonly estado?: number | null) {}
}

export class GetVotacionDetailQuery implements Query<VotacionDetalle> {
  readonly type = '[Votaciones] Get Detail';

  constructor(public readonly id: string) {}
}

export class GetVotacionResultadosQuery implements Query<ResultadosVotacion> {
  readonly type = '[Votaciones] Get Resultados';

  constructor(public readonly id: string) {}
}

export class CreateVotacionCommand implements Command<string> {
  readonly type = '[Votaciones] Create';

  constructor(public readonly draft: VotacionDraft) {}
}

export class AbrirVotacionCommand implements Command<void> {
  readonly type = '[Votaciones] Abrir';

  constructor(public readonly id: string) {}
}

export class CerrarVotacionCommand implements Command<void> {
  readonly type = '[Votaciones] Cerrar';

  constructor(public readonly id: string) {}
}

@Injectable({ providedIn: 'root' })
export class GetVotacionesQueryHandler implements QueryHandler<GetVotacionesQuery, VotacionResumen[]> {
  private readonly repository = inject(VotacionRepository);

  execute(query: GetVotacionesQuery): Observable<VotacionResumen[]> {
    return from(this.repository.getByConjunto(query.conjuntoId, query.estado));
  }
}

@Injectable({ providedIn: 'root' })
export class GetVotacionDetailQueryHandler
  implements QueryHandler<GetVotacionDetailQuery, VotacionDetalle>
{
  private readonly repository = inject(VotacionRepository);

  execute(query: GetVotacionDetailQuery): Observable<VotacionDetalle> {
    return from(this.repository.getById(query.id));
  }
}

@Injectable({ providedIn: 'root' })
export class GetVotacionResultadosQueryHandler
  implements QueryHandler<GetVotacionResultadosQuery, ResultadosVotacion>
{
  private readonly repository = inject(VotacionRepository);

  execute(query: GetVotacionResultadosQuery): Observable<ResultadosVotacion> {
    return from(this.repository.getResultados(query.id));
  }
}

@Injectable({ providedIn: 'root' })
export class CreateVotacionCommandHandler
  implements CommandHandler<CreateVotacionCommand, string>
{
  private readonly repository = inject(VotacionRepository);

  execute(command: CreateVotacionCommand): Observable<string> {
    return from(this.repository.create(command.draft));
  }
}

@Injectable({ providedIn: 'root' })
export class AbrirVotacionCommandHandler
  implements CommandHandler<AbrirVotacionCommand, void>
{
  private readonly repository = inject(VotacionRepository);

  execute(command: AbrirVotacionCommand): Observable<void> {
    return from(this.repository.abrir(command.id));
  }
}

@Injectable({ providedIn: 'root' })
export class CerrarVotacionCommandHandler
  implements CommandHandler<CerrarVotacionCommand, void>
{
  private readonly repository = inject(VotacionRepository);

  execute(command: CerrarVotacionCommand): Observable<void> {
    return from(this.repository.cerrar(command.id));
  }
}
