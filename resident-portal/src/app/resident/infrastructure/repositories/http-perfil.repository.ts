import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { ChangePinDraft } from '../../domain/perfil/change-pin';
import { PerfilRepository } from '../../domain/repositories/perfil.repository';

@Injectable()
export class HttpPerfilRepository implements PerfilRepository {
  private readonly http = inject(HttpClient);

  changePin(draft: ChangePinDraft) {
    return this.http.patch<void>('/api/residentes/pin', draft);
  }
}
