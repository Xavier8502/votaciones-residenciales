import { Type } from '@angular/core';

import { LoginCommandHandler } from '../auth/auth.cqrs';
import {
  CreateConjuntoCommandHandler,
  GetConjuntoDetailQueryHandler,
  GetConjuntosQueryHandler,
  UpdateConjuntoCommandHandler
} from '../conjuntos/conjuntos.cqrs';
import {
  CreateInmuebleCommandHandler,
  GetInmueblesQueryHandler,
  UpdateInmuebleCommandHandler
} from '../inmuebles/inmuebles.cqrs';
import {
  CreateResidenteCommandHandler,
  GetResidentesQueryHandler
} from '../residentes/residentes.cqrs';
import {
  AbrirVotacionCommandHandler,
  CerrarVotacionCommandHandler,
  CreateVotacionCommandHandler,
  GetVotacionDetailQueryHandler,
  GetVotacionResultadosQueryHandler,
  GetVotacionesQueryHandler
} from '../votaciones/votaciones.cqrs';

export const COMMAND_HANDLER_REGISTRY: Record<string, Type<unknown>> = {
  '[Auth] Login': LoginCommandHandler,
  '[Conjuntos] Create': CreateConjuntoCommandHandler,
  '[Conjuntos] Update': UpdateConjuntoCommandHandler,
  '[Inmuebles] Create': CreateInmuebleCommandHandler,
  '[Inmuebles] Update': UpdateInmuebleCommandHandler,
  '[Residentes] Create': CreateResidenteCommandHandler,
  '[Votaciones] Create': CreateVotacionCommandHandler,
  '[Votaciones] Abrir': AbrirVotacionCommandHandler,
  '[Votaciones] Cerrar': CerrarVotacionCommandHandler
};

export const QUERY_HANDLER_REGISTRY: Record<string, Type<unknown>> = {
  '[Conjuntos] Get All': GetConjuntosQueryHandler,
  '[Conjuntos] Get Detail': GetConjuntoDetailQueryHandler,
  '[Inmuebles] Get By Conjunto': GetInmueblesQueryHandler,
  '[Residentes] Get By Conjunto': GetResidentesQueryHandler,
  '[Votaciones] Get By Conjunto': GetVotacionesQueryHandler,
  '[Votaciones] Get Detail': GetVotacionDetailQueryHandler,
  '[Votaciones] Get Resultados': GetVotacionResultadosQueryHandler
};
