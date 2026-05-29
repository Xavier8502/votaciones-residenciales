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

  readonly form = this.fb.nonNullable.group({
    cedula: ['9000000001', [Validators.required, Validators.minLength(5)]],
    pin: ['1234', [Validators.required, Validators.minLength(4), Validators.maxLength(6)]]
  });

  constructor() {
    if (this.session.isAdmin()) {
      void this.router.navigateByUrl('/admin/dashboard');
    }
  }

  submit(): void {
    if (this.form.invalid || this.submitting()) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting.set(true);
    this.error.set('');

    this.commandBus
      .execute(new LoginCommand(this.form.getRawValue()))
      .subscribe({
        next: async () => {
          this.submitting.set(false);
          await this.router.navigateByUrl('/admin/dashboard');
        },
        error: (error: Error) => {
          this.submitting.set(false);
          this.error.set(error.message || 'No fue posible iniciar sesión.');
        }
      });
  }
}
