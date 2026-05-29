import { inject, Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';

import { Inmueble, InmuebleDraft, InmuebleUpdateDraft } from '../../domain/inmuebles/inmueble';
import { InmuebleRepository } from '../../domain/repositories/inmueble.repository';
import { Command, CommandHandler } from '../cqrs/command-bus';
import { Query, QueryHandler } from '../cqrs/query-bus';

export class GetInmueblesQuery implements Query<Inmueble[]> {
  readonly type = '[Inmuebles] Get By Conjunto';

  constructor(public readonly conjuntoId: string, public readonly tipo?: number | null) {}
}

export class CreateInmuebleCommand implements Command<string> {
  readonly type = '[Inmuebles] Create';

  constructor(public readonly draft: InmuebleDraft) {}
}

export class UpdateInmuebleCommand implements Command<void> {
  readonly type = '[Inmuebles] Update';

  constructor(public readonly draft: InmuebleUpdateDraft) {}
}

@Injectable({ providedIn: 'root' })
export class GetInmueblesQueryHandler implements QueryHandler<GetInmueblesQuery, Inmueble[]> {
  private readonly repository = inject(InmuebleRepository);

  execute(query: GetInmueblesQuery): Observable<Inmueble[]> {
    return from(this.repository.getByConjunto(query.conjuntoId, query.tipo));
  }
}

@Injectable({ providedIn: 'root' })
export class CreateInmuebleCommandHandler
  implements CommandHandler<CreateInmuebleCommand, string>
{
  private readonly repository = inject(InmuebleRepository);

  execute(command: CreateInmuebleCommand): Observable<string> {
    return from(this.repository.create(command.draft));
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateInmuebleCommandHandler
  implements CommandHandler<UpdateInmuebleCommand, void>
{
  private readonly repository = inject(InmuebleRepository);

  execute(command: UpdateInmuebleCommand): Observable<void> {
    return from(this.repository.update(command.draft));
  }
}
