import { Injectable, OnDestroy, inject } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';

import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { NuevoVotoNotificacion } from '../../domain/votaciones/votacion';

@Injectable({ providedIn: 'root' })
export class VotacionLiveService implements OnDestroy {
  private readonly auth = inject(AuthSessionService);
  private connection?: signalR.HubConnection;

  readonly nuevoVoto$ = new Subject<NuevoVotoNotificacion>();
  readonly cambioEstado$ = new Subject<{ votacionId: string; estado: string }>();
  readonly connected$ = new Subject<boolean>();

  async connect(votacionId: string): Promise<void> {
    if (this.connection) {
      await this.disconnect(votacionId);
    }

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/hubs/votacion', {
        accessTokenFactory: () => this.auth.token() ?? ''
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Warning)
      .build();

    this.connection.on('NuevoVoto', (data: NuevoVotoNotificacion) => this.nuevoVoto$.next(data));
    this.connection.on('CambioEstado', (data: { votacionId: string; estado: string }) => this.cambioEstado$.next(data));
    this.connection.onreconnected(async () => {
      this.connected$.next(true);
      await this.join(votacionId);
    });

    await this.connection.start();
    await this.join(votacionId);
    this.connected$.next(true);
  }

  async disconnect(votacionId?: string): Promise<void> {
    if (votacionId && this.connection) {
      await this.connection.invoke('SalirDeVotacion', votacionId).catch(() => undefined);
    }

    await this.connection?.stop();
    this.connection = undefined;
    this.connected$.next(false);
  }

  private async join(votacionId: string): Promise<void> {
    await this.connection?.invoke('UnirseAVotacion', votacionId).catch(() => undefined);
  }

  ngOnDestroy(): void {
    void this.connection?.stop();
  }
}
