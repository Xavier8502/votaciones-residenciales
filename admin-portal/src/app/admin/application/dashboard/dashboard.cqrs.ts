import { inject, Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';

import { DashboardConjunto } from '../../domain/dashboard/dashboard';
import { DashboardRepository } from '../../domain/repositories/dashboard.repository';
import { Query, QueryHandler } from '../cqrs/query-bus';

export class GetDashboardQuery implements Query<DashboardConjunto[]> {
  readonly type = '[Dashboard] Get All';
}

@Injectable({ providedIn: 'root' })
export class GetDashboardQueryHandler
  implements QueryHandler<GetDashboardQuery, DashboardConjunto[]>
{
  private readonly repository = inject(DashboardRepository);

  execute(): Observable<DashboardConjunto[]> {
    return from(this.repository.getDashboard());
  }
}