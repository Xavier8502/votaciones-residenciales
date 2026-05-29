import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LucideDownload, LucideKeyRound, LucideLogOut, LucideShieldCheck } from '@lucide/angular';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { InstallPromptService } from '../../../core/pwa/install-prompt.service';
import { CommandBus } from '../../application/cqrs/command-bus';
import { UpdatePinCommand } from '../../application/perfil/perfil.cqrs';

@Component({
  selector: 'app-perfil-page',
  imports: [CommonModule, ReactiveFormsModule, LucideDownload, LucideKeyRound, LucideLogOut, LucideShieldCheck],
  templateUrl: './perfil-page.component.html',
  styleUrl: './perfil-page.component.scss'
})
export class PerfilPageComponent {
  private readonly auth = inject(AuthSessionService);
  private readonly commandBus = inject(CommandBus);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);

  readonly install = inject(InstallPromptService);
  readonly saving = signal(false);
  readonly error = signal('');
  readonly feedback = signal('');
  readonly session = this.auth.session;

  readonly form = this.fb.nonNullable.group({
    pinActual: ['', [Validators.required, Validators.minLength(4)]],
    nuevoPin: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(6)]],
    confirmarPin: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(6)]]
  });

  async installApp(): Promise<void> {
    await this.install.promptInstall();
  }

  logout(): void {
    this.auth.clear();
    void this.router.navigateByUrl('/login');
  }

  save(): void {
    if (this.form.invalid || this.saving()) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.getRawValue();
    if (raw.nuevoPin !== raw.confirmarPin) {
      this.error.set('La confirmacion del PIN no coincide.');
      return;
    }

    this.saving.set(true);
    this.error.set('');
    this.feedback.set('');

    this.commandBus.execute(new UpdatePinCommand({
      pinActual: raw.pinActual,
      nuevoPin: raw.nuevoPin
    })).subscribe({
      next: () => {
        this.saving.set(false);
        this.feedback.set('PIN actualizado correctamente.');
        this.form.reset({
          pinActual: '',
          nuevoPin: '',
          confirmarPin: ''
        });
      },
      error: (error: { error?: { error?: string }; message?: string }) => {
        this.saving.set(false);
        this.error.set(error?.error?.error || error?.message || 'No fue posible actualizar el PIN.');
      }
    });
  }
}
