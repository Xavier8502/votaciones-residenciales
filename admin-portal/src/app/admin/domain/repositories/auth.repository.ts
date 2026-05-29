import { AuthSession, LoginCredentials } from '../auth/auth-session';

export abstract class AuthRepository {
  abstract login(credentials: LoginCredentials): Promise<AuthSession>;
}
