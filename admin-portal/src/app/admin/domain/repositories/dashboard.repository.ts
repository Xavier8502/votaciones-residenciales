import { DashboardConjunto } from '../dashboard/dashboard';

export abstract class DashboardRepository {
  abstract getDashboard(): Promise<DashboardConjunto[]>;
}