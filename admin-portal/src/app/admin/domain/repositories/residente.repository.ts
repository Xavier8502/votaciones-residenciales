import { Residente, ResidenteDraft } from '../residentes/residente';

export abstract class ResidenteRepository {
  abstract getByConjunto(conjuntoId: string, soloPropietarios?: boolean | null): Promise<Residente[]>;
  abstract create(draft: ResidenteDraft): Promise<string>;
}
