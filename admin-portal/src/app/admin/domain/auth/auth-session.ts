export type UserRole = 'AdminConjunto' | 'SuperAdmin';

export interface ConjuntoLoginOption {
  id: string;
  nombre: string;
}

export interface AuthSession {
  token: string;
  nombreCompleto: string;
  rol: UserRole;
  conjuntoId: string;
  actorId: string;
}

export interface LoginCredentials {
  cedula: string;
  pin: string;
  conjuntoId?: string;
}

export interface LoginResult extends AuthSession {
  requiereSeleccionConjunto: boolean;
  conjuntos: ConjuntoLoginOption[];
}