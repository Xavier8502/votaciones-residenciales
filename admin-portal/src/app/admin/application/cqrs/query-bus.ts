import { inject, Injectable, Injector, Type } from '@angular/core';
import { Observable, throwError } from 'rxjs';

import { QUERY_HANDLER_REGISTRY } from './handler-registry';

export interface Query<TResult = unknown> {
  readonly type: string;
}

export interface QueryHandler<TQuery extends Query<TResult>, TResult> {
  execute(query: TQuery): Observable<TResult>;
}

@Injectable({ providedIn: 'root' })
export class QueryBus {
  private readonly injector = inject(Injector);

  execute<TResult>(query: Query<TResult>): Observable<TResult> {
    const token = QUERY_HANDLER_REGISTRY[query.type] as
      | Type<QueryHandler<Query<TResult>, TResult>>
      | undefined;

    if (!token) {
      return throwError(() => new Error(`No existe un QueryHandler para ${query.type}.`));
    }

    return this.injector.get(token).execute(query);
  }
}
