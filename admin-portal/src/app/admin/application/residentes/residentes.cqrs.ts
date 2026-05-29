import { inject, Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';

import { Residente, ResidenteDraft } from '../../domain/residentes/residente';
import { ResidenteRepository } from '../../domain/repositories/residente.repository';
import { Command, CommandHandler } from '../cqrs/command-bus';
import { Query, QueryHandler } from '../cqrs/query-bus';

export class GetResidentesQuery implements Query<Residente[]> {
  readonly type = '[Residentes] Get By Conjunto';

  constructor(public readonly conjuntoId: string, public readonly soloPropietarios?: boolean | null) {}
}

export class CreateResidenteCommand implements Command<string> {
  readonly type = '[Residentes] Create';

  constructor(public readonly draft: ResidenteDraft) {}
}

@Injectable({ providedIn: 'root' })
export class GetResidentesQueryHandler implements QueryHandler<GetResidentesQuery, Residente[]> {
  private readonly repository = inject(ResidenteRepository);

  execute(query: GetResidentesQuery): Observable<Residente[]> {
    return from(this.repository.getByConjunto(query.conjuntoId, query.soloPropietarios));
  }
}

@Injectable({ providedIn: 'root' })
export class CreateResidenteCommandHandler
  implements CommandHandler<CreateResidenteCommand, string>
{
  private readonly repository = inject(ResidenteRepository);

  execute(command: CreateResidenteCommand): Observable<string> {
    return from(this.repository.create(command.draft));
  }
}
