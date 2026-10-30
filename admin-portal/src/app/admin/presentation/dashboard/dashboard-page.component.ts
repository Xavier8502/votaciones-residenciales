import { CommonModule, DecimalPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';

import {
  LucideBuilding2,
  LucideHouse,
  LucideRefreshCw,
  LucideUsers,
  LucideVote
} from '@lucide/angular';

import { QueryBus } from '../../application/cqrs/query-bus';
import { GetDashboardQuery } from '../../application/dashboard/dashboard.cqrs';
import { DashboardConjunto } from '../../domain/dashboard/dashboard';

@Component({
  selector: 'app-dashboard-page',
  imports: [
    CommonModule,
    DecimalPipe,
    LucideBuilding2,
    LucideHouse,
    LucideRefreshCw,
    LucideUsers,
    LucideVote
  ],
  templateUrl: './dashboard-page.component.html',
  styleUrl: './dashboard-page.component.scss'
})
export class DashboardPageComponent {
  private readonly queryBus = inject(QueryBus);

  readonly loading = signal(true);
  readonly error = signal('');
  readonly conjuntos = signal<DashboardConjunto[]>([]);

  constructor() {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set('');

    this.queryBus.execute<DashboardConjunto[]>(
      new GetDashboardQuery()
    ).subscribe({
      next: (resultado) => {
        this.conjuntos.set(resultado);
        this.loading.set(false);
      },
      error: (error: Error) => {
        this.error.set(
          error.message || 'No fue posible cargar los indicadores del dashboard.'
        );
        this.loading.set(false);
      }
    });
  }
}