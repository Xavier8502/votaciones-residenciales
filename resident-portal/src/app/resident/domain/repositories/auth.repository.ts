import { Observable } from 'rxjs';

import { AuthSession } from '../auth/auth-session';

export interface LoginDraft {
  cedula: string;
  pin: string;
}

export abstract class AuthRepository {
  abstract login(draft: LoginDraft): Observable<AuthSession>;
}
