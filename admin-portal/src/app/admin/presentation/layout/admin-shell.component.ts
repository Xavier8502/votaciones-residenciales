import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import {
  LucideBadgeCheck,
  LucideBuilding2,
  LucideDoorOpen,
  LucideHouse,
  LucideLayoutDashboard,
  LucideLogOut,
  LucidePanelLeft,
  LucideUsers,
  LucideVote
} from '@lucide/angular';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { adminNavigation, AdminNavigationItem } from './admin-navigation';

@Component({
  selector: 'app-admin-shell',
  imports: [
    CommonModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    LucideBadgeCheck,
    LucideBuilding2,
    LucideDoorOpen,
    LucideHouse,
    LucideLayoutDashboard,
    LucideLogOut,
    LucidePanelLeft,
    LucideUsers,
    LucideVote
  ],
  templateUrl: './admin-shell.component.html',
  styleUrl: './admin-shell.component.scss'
})
export class AdminShellComponent {
  private readonly auth = inject(AuthSessionService);
  private readonly router = inject(Router);

  readonly compact = signal(false);
  readonly navigation = adminNavigation;
  readonly session = this.auth.session;
  readonly displayName = computed(() => this.session()?.nombreCompleto ?? 'Administrador');

  toggleSidebar(): void {
    this.compact.update((current) => !current);
  }

  async logout(): Promise<void> {
    this.auth.clear();
    await this.router.navigateByUrl('/login');
  }
}
