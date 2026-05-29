import { Type } from '@angular/core';

import { LoginCommandHandler } from '../auth/auth.cqrs';
import { UpdatePinCommandHandler } from '../perfil/perfil.cqrs';
import {
  EmitVotoCommandHandler,
  GetVotacionDetailQueryHandler,
  GetVotacionResultadosQueryHandler,
  GetVotacionesQueryHandler
} from '../votaciones/votaciones.cqrs';

export const COMMAND_HANDLER_REGISTRY: Record<string, Type<unknown>> = {
  '[Auth] Login': LoginCommandHandler,
  '[Perfil] Update Pin': UpdatePinCommandHandler,
  '[Votaciones] Emitir Voto': EmitVotoCommandHandler
};

export const QUERY_HANDLER_REGISTRY: Record<string, Type<unknown>> = {
  '[Votaciones] Get By Conjunto': GetVotacionesQueryHandler,
  '[Votaciones] Get Detail': GetVotacionDetailQueryHandler,
  '[Votaciones] Get Resultados': GetVotacionResultadosQueryHandler
};
