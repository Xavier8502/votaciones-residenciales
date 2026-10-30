import { ConjuntoDetalle } from '../conjuntos/conjunto';

export interface DashboardConjunto {
  conjunto: ConjuntoDetalle;
  totalInmuebles: number;
  totalResidentes: number;
  totalVotaciones: number;
}