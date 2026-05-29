import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import {
  LucideCirclePlus,
  LucidePlay,
  LucideRefreshCw,
  LucideSave,
  LucideSquare
} from '@lucide/angular';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { CommandBus } from '../../application/cqrs/command-bus';
import { QueryBus } from '../../application/cqrs/query-bus';
import {
  AbrirVotacionCommand,
  CerrarVotacionCommand,
  CreateVotacionCommand,
  GetVotacionDetailQuery,
  GetVotacionResultadosQuery,
  GetVotacionesQuery
} from '../../application/votaciones/votaciones.cqrs';
import { estadoVotacionOptions, tipoPesoOptions } from '../../domain/shared/catalogs';
import {
  ResultadosVotacion,
  VotacionDetalle,
  VotacionResumen
} from '../../domain/votaciones/votacion';

@Component({
  selector: 'app-votaciones-page',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    LucideCirclePlus,
    LucidePlay,
    LucideRefreshCw,
    LucideSave,
    LucideSquare
  ],
  templateUrl: './votaciones-page.component.html',
  styleUrl: './votaciones-page.component.scss'
})
export class VotacionesPageComponent {
  private readonly chartPalette = ['#0f766e', '#b7791f', '#9f3560', '#2563eb', '#8b5cf6', '#d97706'];
  private readonly auth = inject(AuthSessionService);
  private readonly queryBus = inject(QueryBus);
  private readonly commandBus = inject(CommandBus);
  private readonly fb = inject(FormBuilder);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly actioning = signal(false);
  readonly error = signal('');
  readonly feedback = signal('');
  readonly resultsError = signal('');
  readonly estado = signal<number | null>(null);
  readonly votaciones = signal<VotacionResumen[]>([]);
  readonly selected = signal<VotacionDetalle | null>(null);
  readonly resultados = signal<ResultadosVotacion | null>(null);
  readonly tipoPesoOptions = tipoPesoOptions;
  readonly estadoOptions = estadoVotacionOptions;

  readonly form = this.fb.nonNullable.group({
    titulo: ['', [Validators.required, Validators.maxLength(200)]],
    descripcion: [''],
    tipoPeso: [1, [Validators.required]],
    quorumRequerido: [51, [Validators.required, Validators.min(1), Validators.max(100)]],
    fechaInicio: ['', [Validators.required]],
    fechaFin: ['', [Validators.required]],
    mostrarResultadosParciales: [true],
    preguntas: this.fb.array([this.createQuestionGroup()])
  });

  constructor() {
    this.addOption(0);
    this.load();
  }

  get preguntas(): FormArray {
    return this.form.controls.preguntas;
  }

  optionsAt(index: number): FormArray {
    return this.preguntas.at(index).get('opciones') as FormArray;
  }

  setEstado(value: string): void {
    this.estado.set(value ? Number(value) : null);
    this.load();
  }

  load(): void {
    const conjuntoId = this.auth.currentConjuntoId();
    if (!conjuntoId) {
      this.error.set('La sesión no tiene un conjunto asociado.');
      return;
    }

    this.loading.set(true);
    this.error.set('');

    this.queryBus.execute<VotacionResumen[]>(new GetVotacionesQuery(conjuntoId, this.estado())).subscribe({
      next: (items) => {
        this.votaciones.set(items);
        this.loading.set(false);
      },
      error: (error: Error) => {
        this.error.set(error.message || 'No fue posible cargar las votaciones.');
        this.loading.set(false);
      }
    });
  }

  select(item: VotacionResumen): void {
    this.loading.set(true);
    this.resultsError.set('');

    this.queryBus.execute<VotacionDetalle>(new GetVotacionDetailQuery(item.id)).subscribe({
      next: (detail) => {
        this.selected.set(detail);
        this.loading.set(false);
      },
      error: (error: Error) => {
        this.error.set(error.message || 'No fue posible cargar el detalle.');
        this.loading.set(false);
      }
    });

    this.queryBus.execute<ResultadosVotacion>(new GetVotacionResultadosQuery(item.id)).subscribe({
      next: (resultados) => {
        this.resultados.set(resultados);
      },
      error: (error: Error) => {
        this.resultados.set(null);
        this.resultsError.set(error.message || 'Los resultados no están disponibles todavía.');
      }
    });
  }

  addQuestion(): void {
    const question = this.createQuestionGroup();
    this.preguntas.push(question);
    this.addOption(this.preguntas.length - 1);
  }

  removeQuestion(index: number): void {
    if (this.preguntas.length <= 1) {
      return;
    }
    this.preguntas.removeAt(index);
  }

  addOption(questionIndex: number): void {
    this.optionsAt(questionIndex).push(
      this.fb.nonNullable.group({
        texto: ['', Validators.required]
      })
    );
  }

  removeOption(questionIndex: number, optionIndex: number): void {
    const options = this.optionsAt(questionIndex);
    if (options.length <= 2) {
      return;
    }
    options.removeAt(optionIndex);
  }

  save(): void {
    if (this.form.invalid || this.saving()) {
      this.form.markAllAsTouched();
      return;
    }

    const conjuntoId = this.auth.currentConjuntoId();
    if (!conjuntoId) {
      this.error.set('La sesión no tiene un conjunto asociado.');
      return;
    }

    const raw = this.form.getRawValue();
    this.saving.set(true);
    this.error.set('');
    this.feedback.set('');

    this.commandBus
      .execute<string>(
        new CreateVotacionCommand({
          conjuntoId,
          titulo: raw.titulo,
          descripcion: raw.descripcion || null,
          tipoPeso: Number(raw.tipoPeso),
          quorumRequerido: Number(raw.quorumRequerido),
          fechaInicio: new Date(raw.fechaInicio).toISOString(),
          fechaFin: new Date(raw.fechaFin).toISOString(),
          mostrarResultadosParciales: raw.mostrarResultadosParciales,
          preguntas: raw.preguntas.map((pregunta, preguntaIndex) => ({
            texto: pregunta.texto,
            orden: preguntaIndex + 1,
            opciones: pregunta.opciones.map((opcion, optionIndex) => ({
              texto: opcion.texto,
              orden: optionIndex + 1
            }))
          }))
        })
      )
      .subscribe({
        next: () => {
          this.saving.set(false);
          this.feedback.set('Votación creada.');
          this.resetForm();
          this.load();
        },
        error: (error: Error) => {
          this.saving.set(false);
          this.error.set(error.message || 'No fue posible crear la votación.');
        }
      });
  }

  abrirSeleccionada(): void {
    const selected = this.selected();
    if (!selected) {
      return;
    }

    this.actioning.set(true);
    this.commandBus.execute<void>(new AbrirVotacionCommand(selected.id)).subscribe({
      next: () => {
        this.actioning.set(false);
        this.feedback.set('Votación abierta.');
        this.load();
        this.select({ ...selected, totalPreguntas: selected.preguntas.length });
      },
      error: (error: Error) => {
        this.actioning.set(false);
        this.error.set(error.message || 'No fue posible abrir la votación.');
      }
    });
  }

  cerrarSeleccionada(): void {
    const selected = this.selected();
    if (!selected) {
      return;
    }

    this.actioning.set(true);
    this.commandBus.execute<void>(new CerrarVotacionCommand(selected.id)).subscribe({
      next: () => {
        this.actioning.set(false);
        this.feedback.set('Votación cerrada.');
        this.load();
        this.select({ ...selected, totalPreguntas: selected.preguntas.length });
      },
      error: (error: Error) => {
        this.actioning.set(false);
        this.error.set(error.message || 'No fue posible cerrar la votación.');
      }
    });
  }

  optionBarStyle(optionIndex: number, porcentajeVotos: number): Record<string, string> {
    const bounded = Math.max(0, Math.min(100, porcentajeVotos));
    const visibleWidth = bounded === 0 ? 0 : Math.max(bounded, 6);

    return {
      '--result-bar-width': `${visibleWidth}%`,
      '--result-bar-color': this.chartPalette[optionIndex % this.chartPalette.length]
    };
  }

  leadingOptionText(pregunta: ResultadosVotacion['preguntas'][number]): string {
    if (!pregunta.opciones.length) {
      return 'Sin datos';
    }

    const top = [...pregunta.opciones].sort((left, right) => right.porcentajeVotos - left.porcentajeVotos)[0];
    return `${top.texto} lidera con ${top.porcentajeVotos.toFixed(1)}%`;
  }

  private createQuestionGroup() {
    return this.fb.nonNullable.group({
      texto: ['', Validators.required],
      opciones: this.fb.array([
        this.fb.nonNullable.group({
          texto: ['', Validators.required]
        })
      ])
    });
  }

  private resetForm(): void {
    this.form.reset({
      titulo: '',
      descripcion: '',
      tipoPeso: 1,
      quorumRequerido: 51,
      fechaInicio: '',
      fechaFin: '',
      mostrarResultadosParciales: true,
      preguntas: []
    });
    while (this.preguntas.length) {
      this.preguntas.removeAt(0);
    }
    this.preguntas.push(this.createQuestionGroup());
    this.addOption(0);
  }
}
