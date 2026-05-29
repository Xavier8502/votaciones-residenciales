import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { VotacionRepository } from '../../domain/repositories/votacion.repository';
import {
  ResultadosVotacion,
  VotacionDetalle,
  VotacionDraft,
  VotacionResumen
} from '../../domain/votaciones/votacion';

@Injectable()
export class HttpVotacionRepository extends VotacionRepository {
  private readonly http = inject(HttpClient);

  getByConjunto(conjuntoId: string, estado?: number | null): Promise<VotacionResumen[]> {
    let params = new HttpParams();
    if (estado) {
      params = params.set('estado', estado);
    }

    return firstValueFrom(
      this.http.get<VotacionResumen[]>(`/api/votaciones/conjunto/${conjuntoId}`, { params })
    );
  }

  getById(id: string): Promise<VotacionDetalle> {
    return firstValueFrom(this.http.get<VotacionDetalle>(`/api/votaciones/${id}`));
  }

  getResultados(id: string): Promise<ResultadosVotacion> {
    return firstValueFrom(this.http.get<ResultadosVotacion>(`/api/votaciones/${id}/resultados`));
  }

  async create(draft: VotacionDraft): Promise<string> {
    const response = await firstValueFrom(this.http.post<{ id: string }>('/api/votaciones', draft));
    return response.id;
  }

  abrir(id: string): Promise<void> {
    return firstValueFrom(this.http.patch<void>(`/api/votaciones/${id}/abrir`, {}));
  }

  cerrar(id: string): Promise<void> {
    return firstValueFrom(this.http.patch<void>(`/api/votaciones/${id}/cerrar`, {}));
  }
}
