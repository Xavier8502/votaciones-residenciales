export interface VotacionResumen {
  id: string;
  titulo: string;
  descripcion?: string | null;
  estado: EstadoVotacion;
  tipoPeso: string;
  quorumRequerido: number;
  fechaInicio: string;
  fechaFin: string;
  mostrarResultadosParciales: boolean;
  totalPreguntas: number;
  creadoEn: string;
}

export interface VotacionDetalle {
  id: string;
  conjuntoId: string;
  titulo: string;
  descripcion?: string | null;
  estado: EstadoVotacion;
  tipoPeso: string;
  quorumRequerido: number;
  fechaInicio: string;
  fechaFin: string;
  mostrarResultadosParciales: boolean;
  actaUrl?: string | null;
  creadoEn: string;
  preguntas: PreguntaDetalle[];
}

export interface PreguntaDetalle {
  id: string;
  texto: string;
  orden: number;
  opciones: OpcionDetalle[];
}

export interface OpcionDetalle {
  id: string;
  texto: string;
  orden: number;
}

export interface EmitirVotoDraft {
  respuestas: RespuestaVoto[];
}

export interface RespuestaVoto {
  preguntaId: string;
  opcionId: string;
}

export type EstadoVotacion = 'Borrador' | 'Abierta' | 'Cerrada' | 'ActaGenerada' | 'Anulada';

export interface ResultadosVotacion {
  votacionId: string;
  titulo: string;
  estado: string;
  totalVotos: number;
  porcentajeParticipacion: number;
  quorumAlcanzado: boolean;
  preguntas: ResultadoPregunta[];
}

export interface ResultadoPregunta {
  preguntaId: string;
  texto: string;
  opciones: ResultadoOpcion[];
}

export interface ResultadoOpcion {
  opcionId: string;
  texto: string;
  totalVotos: number;
  porcentajeVotos: number;
  pesoTotal: number;
}

export interface NuevoVotoNotificacion {
  votacionId: string;
  totalVotos: number;
  porcentajeParticipacion: number;
}
