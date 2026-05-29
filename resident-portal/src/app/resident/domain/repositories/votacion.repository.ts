import { Observable } from 'rxjs';

import {
  EmitirVotoDraft,
  ResultadosVotacion,
  VotacionDetalle,
  VotacionResumen
} from '../votaciones/votacion';

export abstract class VotacionRepository {
  abstract getByConjunto(conjuntoId: string): Observable<VotacionResumen[]>;
  abstract getDetail(id: string): Observable<VotacionDetalle>;
  abstract getResultados(id: string): Observable<ResultadosVotacion>;
  abstract emitirVoto(votacionId: string, draft: EmitirVotoDraft): Observable<{ id: string }>;
}
