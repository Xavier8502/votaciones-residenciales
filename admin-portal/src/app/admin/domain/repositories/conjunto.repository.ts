import { ConjuntoDetalle, ConjuntoDraft, ConjuntoResumen } from '../conjuntos/conjunto';

export abstract class ConjuntoRepository {
  abstract getAll(): Promise<ConjuntoResumen[]>;
  abstract getById(id: string): Promise<ConjuntoDetalle>;
  abstract create(draft: ConjuntoDraft): Promise<string>;
  abstract update(id: string, draft: Omit<ConjuntoDraft, 'nit'>): Promise<void>;
}
