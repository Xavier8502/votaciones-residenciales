import { CommonModule, NgStyle } from '@angular/common';
import { Component, OnDestroy, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import {
  LucideArrowLeft,
  LucideBadgeCheck,
  LucideCircleAlert,
  LucideRefreshCw,
  LucideSendHorizontal
} from '@lucide/angular';

import { CommandBus } from '../../application/cqrs/command-bus';
import { QueryBus } from '../../application/cqrs/query-bus';
import {
  EmitVotoCommand,
  GetVotacionDetailQuery,
  GetVotacionResultadosQuery
} from '../../application/votaciones/votaciones.cqrs';
import { ResultadoPregunta, ResultadosVotacion, VotacionDetalle } from '../../domain/votaciones/votacion';
import { VotacionLiveService } from '../../infrastructure/repositories/votacion-live.service';

const RECEIPT_KEY = 'votaciones.resident.vote-receipts';

@Component({
  selector: 'app-votacion-detail-page',
  imports: [
    CommonModule,
    NgStyle,
    LucideArrowLeft,
    LucideBadgeCheck,
    LucideCircleAlert,
    LucideRefreshCw,
    LucideSendHorizontal
  ],
  templateUrl: './votacion-detail-page.component.html',
  styleUrl: './votacion-detail-page.component.scss'
})
export class VotacionDetailPageComponent implements OnDestroy {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly queryBus = inject(QueryBus);
  private readonly commandBus = inject(CommandBus);
  private readonly live = inject(VotacionLiveService);
  private readonly subscriptions: Subscription[] = [];
  private readonly palette = ['#0f766e', '#c57d16', '#9a3d66', '#2563eb', '#7c3aed', '#d97706'];

  readonly votacionId = this.route.snapshot.paramMap.get('id') ?? '';
  readonly loading = signal(false);
  readonly submitting = signal(false);
  readonly liveConnected = signal(false);
  readonly error = signal('');
  readonly feedback = signal('');
  readonly resultadosError = signal('');
  readonly detail = signal<VotacionDetalle | null>(null);
  readonly resultados = signal<ResultadosVotacion | null>(null);
  readonly voteReceipt = signal(this.readReceipt(this.votacionId));
  readonly selectedAnswers = signal<Record<string, string>>({});

  readonly canVote = computed(() => {
    const detail = this.detail();
    if (!detail || detail.estado !== 'Abierta' || this.voteReceipt()) {
      return false;
    }

    return detail.preguntas.every((pregunta) => !!this.selectedAnswers()[pregunta.id]);
  });

  readonly shouldShowResults = computed(() => {
    const detail = this.detail();
    if (!detail) {
      return false;
    }

    return detail.estado === 'Cerrada' || detail.estado === 'ActaGenerada';
  });

  readonly statusTone = computed(() => {
    switch (this.detail()?.estado) {
      case 'Abierta':
        return 'primary';
      case 'Cerrada':
      case 'ActaGenerada':
        return 'accent';
      default:
        return 'support';
    }
  });

  constructor() {
    this.load();
    this.observeRealtime();
  }

  selectOption(preguntaId: string, opcionId: string): void {
    this.selectedAnswers.set({
      ...this.selectedAnswers(),
      [preguntaId]: opcionId
    });
  }

  submitVote(): void {
    const detail = this.detail();
    if (!detail || !this.canVote()) {
      return;
    }

    const respuestas = detail.preguntas.map((pregunta) => ({
      preguntaId: pregunta.id,
      opcionId: this.selectedAnswers()[pregunta.id]
    }));

    this.submitting.set(true);
    this.error.set('');
    this.feedback.set('');

    this.commandBus.execute(new EmitVotoCommand(detail.id, { respuestas })).subscribe({
      next: () => {
        this.submitting.set(false);
        this.feedback.set('Tu voto fue registrado correctamente.');
        this.writeReceipt(detail.id);
        this.voteReceipt.set(true);
      },
      error: (error: { error?: { error?: string }; message?: string }) => {
        this.submitting.set(false);
        const message = error?.error?.error || error?.message || 'No fue posible emitir tu voto.';
        this.error.set(message);

        if (message.toLowerCase().includes('ya')) {
          this.writeReceipt(detail.id);
          this.voteReceipt.set(true);
        }
      }
    });
  }

  goBack(): void {
    void this.router.navigateByUrl('/app/votaciones');
  }

  optionBarStyle(index: number, percent: number): Record<string, string> {
    const bounded = Math.max(0, Math.min(100, percent));
    const visible = bounded === 0 ? 0 : Math.max(bounded, 5);

    return {
      '--bar-width': `${visible}%`,
      '--bar-color': this.palette[index % this.palette.length]
    };
  }

  topOption(pregunta: ResultadoPregunta): string {
    if (!pregunta.opciones.length) {
      return 'Sin datos aun';
    }

    const top = [...pregunta.opciones].sort((left, right) => right.totalVotos - left.totalVotos)[0];
    return `${top.texto} lidera con ${top.porcentajeVotos.toFixed(1)}%`;
  }

  refresh(): void {
    this.load();
  }

  ngOnDestroy(): void {
    void this.live.disconnect(this.votacionId);
    this.subscriptions.forEach((item) => item.unsubscribe());
  }

  private load(): void {
    if (!this.votacionId) {
      this.error.set('No se encontro la votacion solicitada.');
      return;
    }

    this.loading.set(true);
    this.error.set('');
    this.resultadosError.set('');

    this.queryBus.execute<VotacionDetalle>(new GetVotacionDetailQuery(this.votacionId)).subscribe({
      next: async (detail) => {
        this.detail.set(detail);
        this.loading.set(false);

        if (detail.estado === 'Abierta') {
          try {
            await this.live.connect(detail.id);
            this.liveConnected.set(true);
          } catch {
            this.liveConnected.set(false);
          }
        } else {
          void this.live.disconnect(detail.id);
          this.liveConnected.set(false);
        }

        if (detail.estado === 'Cerrada' || detail.estado === 'ActaGenerada') {
          this.loadResultados(detail.id);
        } else {
          this.resultados.set(null);
        }
      },
      error: (error: Error) => {
        this.loading.set(false);
        this.error.set(error.message || 'No fue posible cargar la votacion.');
      }
    });
  }

  private loadResultados(id: string): void {
    this.queryBus.execute<ResultadosVotacion>(new GetVotacionResultadosQuery(id)).subscribe({
      next: (resultados) => {
        this.resultados.set(resultados);
        this.resultadosError.set('');
      },
      error: (error: Error) => {
        this.resultados.set(null);
        this.resultadosError.set(error.message || 'Los resultados aun no estan disponibles.');
      }
    });
  }

  private observeRealtime(): void {
    this.subscriptions.push(
      this.live.nuevoVoto$.subscribe(() => {
        if (this.shouldShowResults()) {
          this.loadResultados(this.votacionId);
        }
      }),
      this.live.cambioEstado$.subscribe((event) => {
        if (event.votacionId === this.votacionId) {
          this.load();
        }
      }),
      this.live.connected$.subscribe((status) => this.liveConnected.set(status))
    );
  }

  private readReceipt(votacionId: string): boolean {
    try {
      const raw = localStorage.getItem(RECEIPT_KEY);
      if (!raw) {
        return false;
      }

      const receipts = JSON.parse(raw) as Record<string, string>;
      return !!receipts[votacionId];
    } catch {
      return false;
    }
  }

  private writeReceipt(votacionId: string): void {
    try {
      const raw = localStorage.getItem(RECEIPT_KEY);
      const current = raw ? (JSON.parse(raw) as Record<string, string>) : {};
      current[votacionId] = new Date().toISOString();
      localStorage.setItem(RECEIPT_KEY, JSON.stringify(current));
    } catch {
      localStorage.setItem(RECEIPT_KEY, JSON.stringify({ [votacionId]: new Date().toISOString() }));
    }
  }
}
