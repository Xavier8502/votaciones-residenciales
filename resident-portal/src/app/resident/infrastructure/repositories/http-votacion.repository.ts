import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { VotacionRepository } from '../../domain/repositories/votacion.repository';
import {
  EmitirVotoDraft,
  ResultadosVotacion,
  VotacionDetalle,
  VotacionResumen
} from '../../domain/votaciones/votacion';

@Injectable()
export class HttpVotacionRepository implements VotacionRepository {
  private readonly http = inject(HttpClient);
  private readonly base = '/api/votaciones';

  getByConjunto(conjuntoId: string) {
    return this.http.get<VotacionResumen[]>(`${this.base}/conjunto/${conjuntoId}`);
  }

  getDetail(id: string) {
    return this.http.get<VotacionDetalle>(`${this.base}/${id}`);
  }

  getResultados(id: string) {
    return this.http.get<ResultadosVotacion>(`${this.base}/${id}/resultados`);
  }

  emitirVoto(votacionId: string, draft: EmitirVotoDraft) {
    return this.http.post<{ id: string }>(`${this.base}/${votacionId}/votar`, draft);
  }
}
