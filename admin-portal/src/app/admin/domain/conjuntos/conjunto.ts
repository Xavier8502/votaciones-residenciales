export interface ConjuntoResumen {
  id: string;
  nombre: string;
  nit: string;
  direccion: string;
  ciudad: string;
  telefono?: string | null;
  email?: string | null;
  creadoEn: string;
}

export interface ConjuntoDetalle extends ConjuntoResumen {
  totalInmuebles: number;
  totalCoeficientes: number;
}

export interface ConjuntoDraft {
  nombre: string;
  nit: string;
  direccion: string;
  ciudad: string;
  telefono?: string | null;
  email?: string | null;
}
