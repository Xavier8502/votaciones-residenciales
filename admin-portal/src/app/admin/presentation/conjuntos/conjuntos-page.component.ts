import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import {
  LucideCirclePlus,
  LucidePencil,
  LucideRefreshCw,
  LucideSave
} from '@lucide/angular';

import { CommandBus } from '../../application/cqrs/command-bus';
import { QueryBus } from '../../application/cqrs/query-bus';
import {
  CreateConjuntoCommand,
  GetConjuntoDetailQuery,
  GetConjuntosQuery,
  UpdateConjuntoCommand
} from '../../application/conjuntos/conjuntos.cqrs';
import { ConjuntoDetalle, ConjuntoResumen } from '../../domain/conjuntos/conjunto';

@Component({
  selector: 'app-conjuntos-page',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    LucideCirclePlus,
    LucidePencil,
    LucideRefreshCw,
    LucideSave
  ],
  templateUrl: './conjuntos-page.component.html',
  styleUrl: './conjuntos-page.component.scss'
})
export class ConjuntosPageComponent {
  private readonly queryBus = inject(QueryBus);
  private readonly commandBus = inject(CommandBus);
  private readonly fb = inject(FormBuilder);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly error = signal('');
  readonly feedback = signal('');
  readonly items = signal<ConjuntoResumen[]>([]);
  readonly selected = signal<ConjuntoDetalle | null>(null);
  readonly createMode = signal(true);

  readonly form = this.fb.nonNullable.group({
    nombre: ['', [Validators.required, Validators.maxLength(200)]],
    nit: ['', [Validators.required]],
    direccion: ['', [Validators.required, Validators.maxLength(300)]],
    ciudad: ['', [Validators.required, Validators.maxLength(100)]],
    telefono: [''],
    email: ['', [Validators.email]]
  });

  constructor() {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set('');

    this.queryBus.execute<ConjuntoResumen[]>(new GetConjuntosQuery()).subscribe({
      next: (items) => {
        this.items.set(items);
        this.loading.set(false);
      },
      error: (error: Error) => {
        this.error.set(error.message || 'No fue posible cargar los conjuntos.');
        this.loading.set(false);
      }
    });
  }

  startCreate(): void {
    this.createMode.set(true);
    this.selected.set(null);
    this.feedback.set('');
    this.form.reset({
      nombre: '',
      nit: '',
      direccion: '',
      ciudad: '',
      telefono: '',
      email: ''
    });
    this.form.controls.nit.enable();
  }

  select(item: ConjuntoResumen): void {
    this.createMode.set(false);
    this.feedback.set('');
    this.loading.set(true);

    this.queryBus.execute<ConjuntoDetalle>(new GetConjuntoDetailQuery(item.id)).subscribe({
      next: (detail) => {
        this.selected.set(detail);
        this.form.reset({
          nombre: detail.nombre,
          nit: detail.nit,
          direccion: detail.direccion,
          ciudad: detail.ciudad,
          telefono: detail.telefono ?? '',
          email: detail.email ?? ''
        });
        this.form.controls.nit.disable();
        this.loading.set(false);
      },
      error: (error: Error) => {
        this.error.set(error.message || 'No fue posible cargar el detalle.');
        this.loading.set(false);
      }
    });
  }

  save(): void {
    if (this.form.invalid || this.saving()) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    this.error.set('');
    this.feedback.set('');

    const raw = this.form.getRawValue();

    if (this.createMode()) {
      this.commandBus
        .execute<string>(
          new CreateConjuntoCommand({
            nombre: raw.nombre,
            nit: raw.nit,
            direccion: raw.direccion,
            ciudad: raw.ciudad,
            telefono: raw.telefono || null,
            email: raw.email || null
          })
        )
        .subscribe({
          next: () => {
            this.saving.set(false);
            this.feedback.set('Conjunto creado.');
            this.load();
            this.startCreate();
          },
          error: (error: Error) => {
            this.saving.set(false);
            this.error.set(error.message || 'No fue posible guardar el conjunto.');
          }
        });
      return;
    }

    this.commandBus
      .execute<void>(
        new UpdateConjuntoCommand(this.selected()!.id, {
          nombre: raw.nombre,
          direccion: raw.direccion,
          ciudad: raw.ciudad,
          telefono: raw.telefono || null,
          email: raw.email || null
        })
      )
      .subscribe({
        next: () => {
          this.saving.set(false);
          this.feedback.set('Conjunto actualizado.');
          this.load();
          if (this.selected()) {
            this.select({
              ...this.selected()!,
              nombre: raw.nombre,
              direccion: raw.direccion,
              ciudad: raw.ciudad,
              telefono: raw.telefono,
              email: raw.email
            });
          }
        },
        error: (error: Error) => {
          this.saving.set(false);
          this.error.set(error.message || 'No fue posible guardar el conjunto.');
        }
      });
  }
}
