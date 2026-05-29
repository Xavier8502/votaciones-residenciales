import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LucideArrowRight, LucideBadgeCheck, LucideLockKeyhole, LucideSmartphone } from '@lucide/angular';

import { LoginCommand } from '../../application/auth/auth.cqrs';
import { CommandBus } from '../../application/cqrs/command-bus';

@Component({
  selector: 'app-login-page',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    LucideArrowRight,
    LucideBadgeCheck,
    LucideLockKeyhole,
    LucideSmartphone
  ],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss'
})
export class LoginPageComponent {
  private readonly fb = inject(FormBuilder);
  private readonly commandBus = inject(CommandBus);
  private readonly router = inject(Router);

  readonly submitting = signal(false);
  readonly error = signal('');

  readonly form = this.fb.nonNullable.group({
    cedula: ['1000000001', [Validators.required, Validators.minLength(5)]],
    pin: ['1234', [Validators.required, Validators.minLength(4), Validators.maxLength(6)]]
  });

  submit(): void {
    if (this.form.invalid || this.submitting()) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting.set(true);
    this.error.set('');

    this.commandBus.execute(new LoginCommand(this.form.getRawValue())).subscribe({
      next: async () => {
        this.submitting.set(false);
        await this.router.navigateByUrl('/app/inicio');
      },
      error: (error: Error) => {
        this.submitting.set(false);
        this.error.set(error.message || 'No fue posible iniciar sesion.');
      }
    });
  }
}
