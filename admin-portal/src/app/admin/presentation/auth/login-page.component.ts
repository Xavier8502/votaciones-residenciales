import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import {
  LucideArrowRight,
  LucideBadgeCheck,
  LucideBuilding2,
  LucideKeyRound
} from '@lucide/angular';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { LoginCommand } from '../../application/auth/auth.cqrs';
import { CommandBus } from '../../application/cqrs/command-bus';
import {
  ConjuntoLoginOption,
  LoginResult
} from '../../domain/auth/auth-session';


@Component({
  selector: 'app-login-page',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    LucideArrowRight,
    LucideBadgeCheck,
    LucideBuilding2,
    LucideKeyRound
  ],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss'
})
export class LoginPageComponent {
  private readonly fb = inject(FormBuilder);
  private readonly commandBus = inject(CommandBus);
  private readonly router = inject(Router);
  private readonly session = inject(AuthSessionService);

  readonly submitting = signal(false);
  readonly error = signal('');
  readonly conjuntos = signal<ConjuntoLoginOption[]>([]);
  readonly requiereSeleccionConjunto = signal(false);
  readonly nombreAdministrador = signal('');

  readonly form = this.fb.nonNullable.group({
    cedula: ['', [Validators.required, Validators.minLength(5)]],
    pin: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(6)]],
    conjuntoId: ['']
  });

  constructor() {
    this.redirectByRole();
  }

  private redirectByRole(): void {
    if (this.session.isSuperAdmin()) {
      void this.router.navigateByUrl('/superadmin/asignaciones');
    } else if (this.session.isAdmin()) {
      void this.router.navigateByUrl('/admin/dashboard');
    }
  }

  submit(): void {
    if (this.form.invalid || this.submitting()) {
      this.form.markAllAsTouched();
      return;
    }

    if (
      this.requiereSeleccionConjunto() &&
      !this.form.controls.conjuntoId.value
    ) {
      this.error.set('Selecciona el conjunto con el que deseas ingresar.');
      return;
    }

    this.submitting.set(true);
    this.error.set('');

    const credentials = this.form.getRawValue();

    this.commandBus
      .execute<LoginResult>(new LoginCommand({
        cedula: credentials.cedula,
        pin: credentials.pin,
        ...(credentials.conjuntoId
          ? { conjuntoId: credentials.conjuntoId }
          : {})
      }))
      .subscribe({
        next: async (result) => {
          this.submitting.set(false);

          if (result.requiereSeleccionConjunto && !result.token) {
            this.conjuntos.set(result.conjuntos);
            this.nombreAdministrador.set(result.nombreCompleto);
            this.requiereSeleccionConjunto.set(true);
            return;
          }

          if (this.session.isSuperAdmin()) {
            await this.router.navigateByUrl('/superadmin/asignaciones');
          } else if (this.session.isAdmin()) {
            await this.router.navigateByUrl('/admin/dashboard');
          } else {
            this.error.set('No se pudo determinar el rol del usuario.');
          }
        },
        error: (error: Error) => {
          this.submitting.set(false);
          this.error.set(error.message || 'No fue posible iniciar sesión.');
        }
      });
  }
}