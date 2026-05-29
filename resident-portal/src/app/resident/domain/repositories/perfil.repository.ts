import { Observable } from 'rxjs';

import { ChangePinDraft } from '../perfil/change-pin';

export abstract class PerfilRepository {
  abstract changePin(draft: ChangePinDraft): Observable<void>;
}
