import { inject, Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';

import { ConjuntoDetalle, ConjuntoDraft, ConjuntoResumen } from '../../domain/conjuntos/conjunto';
import { ConjuntoRepository } from '../../domain/repositories/conjunto.repository';
import { Command, CommandHandler } from '../cqrs/command-bus';
import { Query, QueryHandler } from '../cqrs/query-bus';

export class GetConjuntosQuery implements Query<ConjuntoResumen[]> {
  readonly type = '[Conjuntos] Get All';
}

export class GetConjuntoDetailQuery implements Query<ConjuntoDetalle> {
  readonly type = '[Conjuntos] Get Detail';

  constructor(public readonly id: string) {}
}

export class CreateConjuntoCommand implements Command<string> {
  readonly type = '[Conjuntos] Create';

  constructor(public readonly draft: ConjuntoDraft) {}
}

export class UpdateConjuntoCommand implements Command<void> {
  readonly type = '[Conjuntos] Update';

  constructor(public readonly id: string, public readonly draft: Omit<ConjuntoDraft, 'nit'>) {}
}

@Injectable({ providedIn: 'root' })
export class GetConjuntosQueryHandler implements QueryHandler<GetConjuntosQuery, ConjuntoResumen[]> {
  private readonly repository = inject(ConjuntoRepository);

  execute(): Observable<ConjuntoResumen[]> {
    return from(this.repository.getAll());
  }
}

@Injectable({ providedIn: 'root' })
export class GetConjuntoDetailQueryHandler
  implements QueryHandler<GetConjuntoDetailQuery, ConjuntoDetalle>
{
  private readonly repository = inject(ConjuntoRepository);

  execute(query: GetConjuntoDetailQuery): Observable<ConjuntoDetalle> {
    return from(this.repository.getById(query.id));
  }
}

@Injectable({ providedIn: 'root' })
export class CreateConjuntoCommandHandler
  implements CommandHandler<CreateConjuntoCommand, string>
{
  private readonly repository = inject(ConjuntoRepository);

  execute(command: CreateConjuntoCommand): Observable<string> {
    return from(this.repository.create(command.draft));
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateConjuntoCommandHandler
  implements CommandHandler<UpdateConjuntoCommand, void>
{
  private readonly repository = inject(ConjuntoRepository);

  execute(command: UpdateConjuntoCommand): Observable<void> {
    return from(this.repository.update(command.id, command.draft));
  }
}
