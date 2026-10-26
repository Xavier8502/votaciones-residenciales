import { CommonModule, DecimalPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  LucideBuilding2,
  LucideHouse,
  LucideRefreshCw,
  LucideUsers,
  LucideVote
} from '@lucide/angular';

import { QueryBus } from '../../application/cqrs/query-bus';
import {
  GetConjuntoDetailQuery,
  GetConjuntosQuery
} from '../../application/conjuntos/conjuntos.cqrs';
import { GetInmueblesQuery } from '../../application/inmuebles/inmuebles.cqrs';
import { GetResidentesQuery } from '../../application/residentes/residentes.cqrs';
import { GetVotacionesQuery } from '../../application/votaciones/votaciones.cqrs';
import { ConjuntoDetalle, ConjuntoResumen } from '../../domain/conjuntos/conjunto';
import { Inmueble } from '../../domain/inmuebles/inmueble';
import { Residente } from '../../domain/residentes/residente';
import { VotacionResumen } from '../../domain/votaciones/votacion';

interface DashboardConjunto {
  conjunto: ConjuntoDetalle;
  totalInmuebles: number;
  totalResidentes: number;
  totalVotaciones: number;
}

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

    this.queryBus.execute<ConjuntoResumen[]>(new GetConjuntosQuery()).subscribe({
      next: (conjuntos) => this.loadMetrics(conjuntos),
      error: (error: Error) => {
        this.error.set(error.message || 'No fue posible cargar los conjuntos.');
        this.loading.set(false);
      }
    });
  }

  private loadMetrics(conjuntos: ConjuntoResumen[]): void {
    if (!conjuntos.length) {
      this.conjuntos.set([]);
      this.loading.set(false);
      return;
    }

    const requests = conjuntos.map((item) =>
      forkJoin({
        detalle: this.queryBus.execute<ConjuntoDetalle>(new GetConjuntoDetailQuery(item.id)),
        inmuebles: this.queryBus.execute<Inmueble[]>(new GetInmueblesQuery(item.id, null)),
        residentes: this.queryBus.execute<Residente[]>(new GetResidentesQuery(item.id, null)),
        votaciones: this.queryBus.execute<VotacionResumen[]>(new GetVotacionesQuery(item.id, null))
      }).pipe(
        map(({ detalle, inmuebles, residentes, votaciones }) => ({
          conjunto: detalle,
          totalInmuebles: inmuebles.length,
          totalResidentes: residentes.filter((residente) => residente.activo).length,
          totalVotaciones: votaciones.length
        }))
      )
    );

    forkJoin(requests).subscribe({
      next: (resultado) => {
        this.conjuntos.set(resultado);
        this.loading.set(false);
      },
      error: (error: Error) => {
        this.error.set(error.message || 'No fue posible cargar los indicadores de los conjuntos.');
        this.loading.set(false);
      }
    });
  }
}
