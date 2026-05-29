import { computed, Injectable, signal } from '@angular/core';

import { AuthSession } from '../../admin/domain/auth/auth-session';

const STORAGE_KEY = 'votaciones.admin.session';

@Injectable({ providedIn: 'root' })
export class AuthSessionService {
  private readonly sessionState = signal<AuthSession | null>(this.readFromStorage());

  readonly session = this.sessionState.asReadonly();
  readonly isAuthenticated = computed(() => !!this.sessionState()?.token);
  readonly isAdmin = computed(() => this.sessionState()?.rol === 'AdminConjunto');
  readonly currentConjuntoId = computed(() => this.sessionState()?.conjuntoId ?? null);

  setSession(session: AuthSession): void {
    this.sessionState.set(session);
    localStorage.setItem(STORAGE_KEY, JSON.stringify(session));
  }

  clear(): void {
    this.sessionState.set(null);
    localStorage.removeItem(STORAGE_KEY);
  }

  token(): string | null {
    return this.sessionState()?.token ?? null;
  }

  private readFromStorage(): AuthSession | null {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) {
      return null;
    }

    try {
      return JSON.parse(raw) as AuthSession;
    } catch {
      localStorage.removeItem(STORAGE_KEY);
      return null;
    }
  }
}
