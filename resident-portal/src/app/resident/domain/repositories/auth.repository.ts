
import { LoginCredentials, LoginResult } from '../auth/auth-session';

export abstract class AuthRepository {
  abstract login(credentials: LoginCredentials): Promise<LoginResult>;
}
