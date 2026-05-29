import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { forkJoin } from 'rxjs';
import {
  LucideRefreshCw,
  LucideSave
} from '@lucide/angular';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { CommandBus } from '../../application/cqrs/command-bus';
import { QueryBus } from '../../application/cqrs/query-bus';
import { GetInmueblesQuery } from '../../application/inmuebles/inmuebles.cqrs';
import {
  CreateResidenteCommand,
  GetResidentesQuery
} from '../../application/residentes/residentes.cqrs';
import { Inmueble } from '../../domain/inmuebles/inmueble';
import { Residente } from '../../domain/residentes/residente';

@Component({
  selector: 'app-residentes-page',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    LucideRefreshCw,
    LucideSave
  ],
  templateUrl: './residentes-page.component.html',
  styleUrl: './residentes-page.component.scss'
})
export class ResidentesPageComponent {
  private readonly auth = inject(AuthSessionService);
  private readonly queryBus = inject(QueryBus);
  private readonly commandBus = inject(CommandBus);
  private readonly fb = inject(FormBuilder);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly error = signal('');
  readonly feedback = signal('');
  readonly residentes = signal<Residente[]>([]);
  readonly inmuebles = signal<Inmueble[]>([]);
  readonly soloPropietarios = signal<boolean | null>(null);

  readonly form = this.fb.nonNullable.group({
    inmuebleId: ['', [Validators.required]],
    cedula: ['', [Validators.required, Validators.minLength(5)]],
    nombre: ['', [Validators.required]],
    apellido: ['', [Validators.required]],
    pin: ['1234', [Validators.required, Validators.minLength(4), Validators.maxLength(6)]],
    esPropietario: [true],
    telefono: [''],
    email: ['', [Validators.email]]
  });

  constructor() {
    this.load();
  }

  setFilter(value: string): void {
    this.soloPropietarios.set(value === '' ? null : value === 'true');
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

    forkJoin({
      residentes: this.queryBus.execute<Residente[]>(
        new GetResidentesQuery(conjuntoId, this.soloPropietarios())
      ),
      inmuebles: this.queryBus.execute<Inmueble[]>(new GetInmueblesQuery(conjuntoId, null))
    }).subscribe({
      next: ({ residentes, inmuebles }) => {
        this.residentes.set(residentes);
        this.inmuebles.set(inmuebles);
        this.loading.set(false);
      },
      error: (error: Error) => {
        this.error.set(error.message || 'No fue posible cargar la información.');
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

    this.commandBus
      .execute<string>(
        new CreateResidenteCommand({
          inmuebleId: raw.inmuebleId,
          cedula: raw.cedula,
          nombre: raw.nombre,
          apellido: raw.apellido,
          pin: raw.pin,
          esPropietario: raw.esPropietario,
          telefono: raw.telefono || null,
          email: raw.email || null
        })
      )
      .subscribe({
        next: () => {
          this.saving.set(false);
          this.feedback.set('Residente creado.');
          this.form.reset({
            inmuebleId: '',
            cedula: '',
            nombre: '',
            apellido: '',
            pin: '1234',
            esPropietario: true,
            telefono: '',
            email: ''
          });
          this.load();
        },
        error: (error: Error) => {
          this.saving.set(false);
          this.error.set(error.message || 'No fue posible crear el residente.');
        }
      });
  }
}
