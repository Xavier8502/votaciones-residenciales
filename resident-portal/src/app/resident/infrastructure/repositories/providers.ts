import { EnvironmentProviders, makeEnvironmentProviders } from '@angular/core';

import { AuthRepository } from '../../domain/repositories/auth.repository';
import { PerfilRepository } from '../../domain/repositories/perfil.repository';
import { VotacionRepository } from '../../domain/repositories/votacion.repository';
import { HttpAuthRepository } from './http-auth.repository';
import { HttpPerfilRepository } from './http-perfil.repository';
import { HttpVotacionRepository } from './http-votacion.repository';

export function provideResidentRepositories(): EnvironmentProviders {
  return makeEnvironmentProviders([
    { provide: AuthRepository, useClass: HttpAuthRepository },
    { provide: PerfilRepository, useClass: HttpPerfilRepository },
    { provide: VotacionRepository, useClass: HttpVotacionRepository }
  ]);
}
