
export interface AuthSession {
  token: string;
  nombreCompleto: string;
  rol: string;
  conjuntoId: string;
  actorId: string;
}

export interface ConjuntoLoginOption {
  id: string;
  nombre: string;
}

export interface LoginResult {
  token: string | null;
  nombreCompleto: string;
  rol: string;
  conjuntoId: string;
  residenteId: string;
  conjuntos?: ConjuntoLoginOption[] | null;
  requiereSeleccionConjunto?: boolean;
}

export interface LoginCredentials {
  cedula: string;
  pin: string;
  conjuntoId?: string;
}
