import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { LucideArrowRight, LucideCalendarClock, LucideVote } from '@lucide/angular';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { InstallPromptService } from '../../../core/pwa/install-prompt.service';
import { QueryBus } from '../../application/cqrs/query-bus';
import { GetVotacionesQuery } from '../../application/votaciones/votaciones.cqrs';
import { VotacionResumen } from '../../domain/votaciones/votacion';

@Component({
  selector: 'app-home-page',
  imports: [CommonModule, RouterLink, LucideArrowRight, LucideCalendarClock, LucideVote],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {
  private readonly auth = inject(AuthSessionService);
  private readonly queryBus = inject(QueryBus);

  readonly install = inject(InstallPromptService);
  readonly loading = signal(false);
  readonly error = signal('');
  readonly votaciones = signal<VotacionResumen[]>([]);

  readonly abiertas = computed(() => this.votaciones().filter((item) => item.estado === 'Abierta'));
  readonly cerradas = computed(() => this.votaciones().filter((item) => item.estado === 'Cerrada' || item.estado === 'ActaGenerada'));
  readonly proximas = computed(() =>
    [...this.abiertas()].sort((left, right) => left.fechaFin.localeCompare(right.fechaFin)).slice(0, 2)
  );

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
        this.votaciones.set(items);
        this.loading.set(false);
      },
      error: (error: Error) => {
        this.error.set(error.message || 'No fue posible cargar el resumen.');
        this.loading.set(false);
      }
    });
  }

  async installApp(): Promise<void> {
    await this.install.promptInstall();
  }
}
