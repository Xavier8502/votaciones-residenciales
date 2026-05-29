import {
  ResultadosVotacion,
  VotacionDetalle,
  VotacionDraft,
  VotacionResumen
} from '../votaciones/votacion';

export abstract class VotacionRepository {
  abstract getByConjunto(conjuntoId: string, estado?: number | null): Promise<VotacionResumen[]>;
  abstract getById(id: string): Promise<VotacionDetalle>;
  abstract getResultados(id: string): Promise<ResultadosVotacion>;
  abstract create(draft: VotacionDraft): Promise<string>;
  abstract abrir(id: string): Promise<void>;
  abstract cerrar(id: string): Promise<void>;
}
