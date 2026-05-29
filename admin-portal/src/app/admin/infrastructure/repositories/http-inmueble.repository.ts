import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { Inmueble, InmuebleDraft, InmuebleUpdateDraft } from '../../domain/inmuebles/inmueble';
import { InmuebleRepository } from '../../domain/repositories/inmueble.repository';

@Injectable()
export class HttpInmuebleRepository extends InmuebleRepository {
  private readonly http = inject(HttpClient);

  getByConjunto(conjuntoId: string, tipo?: number | null): Promise<Inmueble[]> {
    let params = new HttpParams();
    if (tipo) {
      params = params.set('tipo', tipo);
    }

    return firstValueFrom(
      this.http.get<Inmueble[]>(`/api/inmuebles/conjunto/${conjuntoId}`, { params })
    );
  }

  async create(draft: InmuebleDraft): Promise<string> {
    const response = await firstValueFrom(this.http.post<{ id: string }>('/api/inmuebles', draft));
    return response.id;
  }

  update(draft: InmuebleUpdateDraft): Promise<void> {
    return firstValueFrom(this.http.put<void>(`/api/inmuebles/${draft.id}`, draft));
  }
}
