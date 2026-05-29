export interface Inmueble {
  id: string;
  conjuntoId: string;
  numero: string;
  tipo: string;
  coeficiente: number;
  torre?: string | null;
  piso?: string | null;
  totalResidentes: number;
  creadoEn: string;
}

export interface InmuebleDraft {
  conjuntoId: string;
  numero: string;
  tipo: number;
  coeficiente: number;
  torre?: string | null;
  piso?: string | null;
}

export interface InmuebleUpdateDraft {
  id: string;
  tipo: number;
  coeficiente: number;
  torre?: string | null;
  piso?: string | null;
}
