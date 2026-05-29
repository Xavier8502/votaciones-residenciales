import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { LucideArrowRight, LucideRefreshCw } from '@lucide/angular';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { QueryBus } from '../../application/cqrs/query-bus';
import { GetVotacionesQuery } from '../../application/votaciones/votaciones.cqrs';
import { ResidentVoteFilter, residentVoteFilters } from '../../domain/shared/catalogs';
import { VotacionResumen } from '../../domain/votaciones/votacion';

@Component({
  selector: 'app-votaciones-page',
  imports: [CommonModule, RouterLink, LucideArrowRight, LucideRefreshCw],
  templateUrl: './votaciones-page.component.html',
  styleUrl: './votaciones-page.component.scss'
})
export class VotacionesPageComponent {
  private readonly auth = inject(AuthSessionService);
  private readonly queryBus = inject(QueryBus);

  readonly loading = signal(false);
  readonly error = signal('');
  readonly votaciones = signal<VotacionResumen[]>([]);
  readonly activeFilter = signal<ResidentVoteFilter>('all');
  readonly filters = residentVoteFilters;

  readonly filtered = computed(() => {
    switch (this.activeFilter()) {
      case 'open':
        return this.votaciones().filter((item) => item.estado === 'Abierta');
      case 'closed':
        return this.votaciones().filter((item) => item.estado !== 'Abierta' && item.estado !== 'Borrador');
      default:
        return this.votaciones();
    }
  });

  constructor() {
    this.load();
  }

  load(): void {
    const conjuntoId = this.auth.currentConjuntoId();
    if (!conjuntoId) {
      this.error.set('No fue posible identificar el conjunto del residente.');
      return;
    }

    this.loading.set(true);
    this.error.set('');

    this.queryBus.execute<VotacionResumen[]>(new GetVotacionesQuery(conjuntoId)).subscribe({
      next: (items) => {
        this.votaciones.set(
          [...items].sort((left, right) => right.fechaInicio.localeCompare(left.fechaInicio))
        );
        this.loading.set(false);
      },
      error: (error: Error) => {
        this.error.set(error.message || 'No fue posible cargar las votaciones.');
        this.loading.set(false);
      }
    });
  }

  setFilter(value: ResidentVoteFilter): void {
    this.activeFilter.set(value);
  }
}
