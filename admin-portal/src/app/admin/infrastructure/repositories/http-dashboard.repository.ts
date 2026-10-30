import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { DashboardConjunto } from '../../domain/dashboard/dashboard';
import { DashboardRepository } from '../../domain/repositories/dashboard.repository';

@Injectable()
export class HttpDashboardRepository extends DashboardRepository {
  private readonly http = inject(HttpClient);

  getDashboard(): Promise<DashboardConjunto[]> {
    return firstValueFrom(
      this.http.get<DashboardConjunto[]>('/api/admin/dashboard')
    );
  }
}