import { CommonModule, DecimalPipe } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { forkJoin } from 'rxjs';
import {
  LucideBuilding2,
  LucideHouse,
  LucideRefreshCw,
  LucideUsers,
  LucideVote
} from '@lucide/angular';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { QueryBus } from '../../application/cqrs/query-bus';
import { GetConjuntoDetailQuery } from '../../application/conjuntos/conjuntos.cqrs';
import { GetInmueblesQuery } from '../../application/inmuebles/inmuebles.cqrs';
import { GetResidentesQuery } from '../../application/residentes/residentes.cqrs';
import { GetVotacionesQuery } from '../../application/votaciones/votaciones.cqrs';
import { ConjuntoDetalle } from '../../domain/conjuntos/conjunto';
import { Inmueble } from '../../domain/inmuebles/inmueble';
import { Residente } from '../../domain/residentes/residente';
import { VotacionResumen } from '../../domain/votaciones/votacion';

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
  private readonly auth = inject(AuthSessionService);

  readonly loading = signal(true);
  readonly error = signal('');
  readonly conjunto = signal<ConjuntoDetalle | null>(null);
  readonly inmuebles = signal<Inmueble[]>([]);
  readonly residentes = signal<Residente[]>([]);
  readonly votaciones = signal<VotacionResumen[]>([]);

  readonly metricas = computed(() => [
    { label: 'Inmuebles', value: this.inmuebles().length, tone: 'primary' },
    {
      label: 'Residentes activos',
      value: this.residentes().filter((item) => item.activo).length,
      tone: 'support'
    },
    { label: 'Votaciones', value: this.votaciones().length, tone: 'accent' },
    { label: 'Coeficiente total', value: this.conjunto()?.totalCoeficientes ?? 0, tone: 'neutral' }
  ]);

  constructor() {
    this.load();
  }

  load(): void {
    const conjuntoId = this.auth.currentConjuntoId();
    if (!conjuntoId) {
      this.error.set('La sesión no tiene un conjunto asociado.');
      this.loading.set(false);
      return;
    }

    this.loading.set(true);
    this.error.set('');

    forkJoin({
      conjunto: this.queryBus.execute<ConjuntoDetalle>(new GetConjuntoDetailQuery(conjuntoId)),
      inmuebles: this.queryBus.execute<Inmueble[]>(new GetInmueblesQuery(conjuntoId, null)),
      residentes: this.queryBus.execute<Residente[]>(new GetResidentesQuery(conjuntoId, null)),
      votaciones: this.queryBus.execute<VotacionResumen[]>(new GetVotacionesQuery(conjuntoId, null))
    }).subscribe({
      next: ({ conjunto, inmuebles, residentes, votaciones }) => {
        this.conjunto.set(conjunto);
        this.inmuebles.set(inmuebles);
        this.residentes.set(residentes);
        this.votaciones.set(votaciones);
        this.loading.set(false);
      },
      error: (error: Error) => {
        this.error.set(error.message || 'No fue posible cargar el tablero.');
        this.loading.set(false);
      }
    });
  }
}
