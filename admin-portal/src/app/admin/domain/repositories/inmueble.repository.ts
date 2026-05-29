import { Inmueble, InmuebleDraft, InmuebleUpdateDraft } from '../inmuebles/inmueble';

export abstract class InmuebleRepository {
  abstract getByConjunto(conjuntoId: string, tipo?: number | null): Promise<Inmueble[]>;
  abstract create(draft: InmuebleDraft): Promise<string>;
  abstract update(draft: InmuebleUpdateDraft): Promise<void>;
}
