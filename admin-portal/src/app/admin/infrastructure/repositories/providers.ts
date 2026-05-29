import { EnvironmentProviders, makeEnvironmentProviders } from '@angular/core';

import { AuthRepository } from '../../domain/repositories/auth.repository';
import { ConjuntoRepository } from '../../domain/repositories/conjunto.repository';
import { InmuebleRepository } from '../../domain/repositories/inmueble.repository';
import { ResidenteRepository } from '../../domain/repositories/residente.repository';
import { VotacionRepository } from '../../domain/repositories/votacion.repository';
import { HttpAuthRepository } from './http-auth.repository';
import { HttpConjuntoRepository } from './http-conjunto.repository';
import { HttpInmuebleRepository } from './http-inmueble.repository';
import { HttpResidenteRepository } from './http-residente.repository';
import { HttpVotacionRepository } from './http-votacion.repository';

export function provideAdminRepositories(): EnvironmentProviders {
  return makeEnvironmentProviders([
    { provide: AuthRepository, useClass: HttpAuthRepository },
    { provide: ConjuntoRepository, useClass: HttpConjuntoRepository },
    { provide: InmuebleRepository, useClass: HttpInmuebleRepository },
    { provide: ResidenteRepository, useClass: HttpResidenteRepository },
    { provide: VotacionRepository, useClass: HttpVotacionRepository }
  ]);
}
