import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { ConjuntoDetalle, ConjuntoDraft, ConjuntoResumen } from '../../domain/conjuntos/conjunto';
import { ConjuntoRepository } from '../../domain/repositories/conjunto.repository';

@Injectable()
export class HttpConjuntoRepository extends ConjuntoRepository {
  private readonly http = inject(HttpClient);

  getAll(): Promise<ConjuntoResumen[]> {
    return firstValueFrom(this.http.get<ConjuntoResumen[]>('/api/conjuntos'));
  }

  getById(id: string): Promise<ConjuntoDetalle> {
    return firstValueFrom(this.http.get<ConjuntoDetalle>(`/api/conjuntos/${id}`));
  }

  async create(draft: ConjuntoDraft): Promise<string> {
    const response = await firstValueFrom(this.http.post<{ id: string }>('/api/conjuntos', draft));
    return response.id;
  }

  update(id: string, draft: Omit<ConjuntoDraft, 'nit'>): Promise<void> {
    return firstValueFrom(this.http.put<void>(`/api/conjuntos/${id}`, { id, ...draft }));
  }
}
