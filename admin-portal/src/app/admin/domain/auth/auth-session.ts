export interface AuthSession {
  token: string;
  nombreCompleto: string;
  rol: string;
  conjuntoId: string;
  actorId: string;
}

export interface LoginCredentials {
  cedula: string;
  pin: string;
}
