import { CommonModule } from '@angular/common';
import { Component, computed, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import {
  LucideBadgeCheck,
  LucideHouse,
  LucideLogOut,
  LucideUserRound,
  LucideVote
} from '@lucide/angular';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { InstallPromptService } from '../../../core/pwa/install-prompt.service';
import { OnlineStatusService } from '../../../core/pwa/online-status.service';
import { residentNavigation } from './resident-navigation';

@Component({
  selector: 'app-resident-shell',
  imports: [
    CommonModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    LucideBadgeCheck,
    LucideHouse,
    LucideLogOut,
    LucideUserRound,
    LucideVote
  ],
  templateUrl: './resident-shell.component.html',
  styleUrl: './resident-shell.component.scss'
})
export class ResidentShellComponent {
  private readonly auth = inject(AuthSessionService);
  private readonly router = inject(Router);

  readonly install = inject(InstallPromptService);
  readonly online = inject(OnlineStatusService);
  readonly residentName = this.auth.residentName;
  readonly firstName = computed(() => this.residentName().split(' ')[0] ?? 'Residente');
  readonly navigation = residentNavigation;

  async installApp(): Promise<void> {
    await this.install.promptInstall();
  }

  logout(): void {
    this.auth.clear();
    void this.router.navigateByUrl('/login');
  }
}
