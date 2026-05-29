import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import {
  LucideCirclePlus,
  LucideRefreshCw,
  LucideSave
} from '@lucide/angular';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { CommandBus } from '../../application/cqrs/command-bus';
import { QueryBus } from '../../application/cqrs/query-bus';
import {
  CreateInmuebleCommand,
  GetInmueblesQuery,
  UpdateInmuebleCommand
} from '../../application/inmuebles/inmuebles.cqrs';
import { Inmueble } from '../../domain/inmuebles/inmueble';
import { tipoInmuebleOptions } from '../../domain/shared/catalogs';

@Component({
  selector: 'app-inmuebles-page',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    LucideCirclePlus,
    LucideRefreshCw,
    LucideSave
  ],
  templateUrl: './inmuebles-page.component.html',
  styleUrl: './inmuebles-page.component.scss'
})
export class InmueblesPageComponent {
  private readonly auth = inject(AuthSessionService);
  private readonly queryBus = inject(QueryBus);
  private readonly commandBus = inject(CommandBus);
  private readonly fb = inject(FormBuilder);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly error = signal('');
  readonly feedback = signal('');
  readonly items = signal<Inmueble[]>([]);
  readonly selected = signal<Inmueble | null>(null);
  readonly createMode = signal(true);
  readonly filterTipo = signal<number | null>(null);
  readonly tipoOptions = tipoInmuebleOptions;

  readonly form = this.fb.nonNullable.group({
    numero: ['', [Validators.required, Validators.maxLength(20)]],
    tipo: [1, [Validators.required]],
    coeficiente: [1, [Validators.required, Validators.min(0.01)]],
    torre: [''],
    piso: ['']
  });

  constructor() {
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

    this.queryBus.execute<Inmueble[]>(new GetInmueblesQuery(conjuntoId, this.filterTipo())).subscribe({
      next: (items) => {
        this.items.set(items);
        this.loading.set(false);
      },
      error: (error: Error) => {
        this.error.set(error.message || 'No fue posible cargar los inmuebles.');
        this.loading.set(false);
      }
    });
  }

  setFilterTipo(value: string): void {
    this.filterTipo.set(value ? Number(value) : null);
    this.load();
  }

  startCreate(): void {
    this.createMode.set(true);
    this.selected.set(null);
    this.feedback.set('');
    this.form.reset({
      numero: '',
      tipo: 1,
      coeficiente: 1,
      torre: '',
      piso: ''
    });
    this.form.controls.numero.enable();
  }

  select(item: Inmueble): void {
    this.createMode.set(false);
    this.selected.set(item);
    this.feedback.set('');
    this.form.reset({
      numero: item.numero,
      tipo: this.tipoCodeFromLabel(item.tipo),
      coeficiente: item.coeficiente,
      torre: item.torre ?? '',
      piso: item.piso ?? ''
    });
    this.form.controls.numero.disable();
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

    this.saving.set(true);
    this.error.set('');
    this.feedback.set('');

    const raw = this.form.getRawValue();

    if (this.createMode()) {
      this.commandBus
        .execute<string>(
          new CreateInmuebleCommand({
            conjuntoId,
            numero: raw.numero,
            tipo: Number(raw.tipo),
            coeficiente: Number(raw.coeficiente),
            torre: raw.torre || null,
            piso: raw.piso || null
          })
        )
        .subscribe({
          next: () => {
            this.saving.set(false);
            this.feedback.set('Inmueble creado.');
            this.load();
            this.startCreate();
          },
          error: (error: Error) => {
            this.saving.set(false);
            this.error.set(error.message || 'No fue posible guardar el inmueble.');
          }
        });
      return;
    }

    this.commandBus
      .execute<void>(
        new UpdateInmuebleCommand({
          id: this.selected()!.id,
          tipo: Number(raw.tipo),
          coeficiente: Number(raw.coeficiente),
          torre: raw.torre || null,
          piso: raw.piso || null
        })
      )
      .subscribe({
        next: () => {
          this.saving.set(false);
          this.feedback.set('Inmueble actualizado.');
          this.load();
        },
        error: (error: Error) => {
          this.saving.set(false);
          this.error.set(error.message || 'No fue posible guardar el inmueble.');
        }
      });
  }

  private tipoCodeFromLabel(label: string): number {
    const match = this.tipoOptions.find((item) => item.label.toLowerCase() === label.toLowerCase());
    return match?.value ?? 1;
  }
}
