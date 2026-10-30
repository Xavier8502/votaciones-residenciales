export type UserRole = 'AdminConjunto' | 'SuperAdmin';

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
}
