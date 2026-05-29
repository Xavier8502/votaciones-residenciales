export interface Residente {
  id: string;
  inmuebleId: string;
  numeroInmueble: string;
  cedula: string;
  nombreCompleto: string;
  telefono?: string | null;
  email?: string | null;
  esPropietario: boolean;
  activo: boolean;
  creadoEn: string;
}

export interface ResidenteDraft {
  inmuebleId: string;
  cedula: string;
  nombre: string;
  apellido: string;
  pin: string;
  esPropietario: boolean;
  telefono?: string | null;
  email?: string | null;
}
