import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { Residente, ResidenteDraft } from '../../domain/residentes/residente';
import { ResidenteRepository } from '../../domain/repositories/residente.repository';

@Injectable()
export class HttpResidenteRepository extends ResidenteRepository {
  private readonly http = inject(HttpClient);

  getByConjunto(conjuntoId: string, soloPropietarios?: boolean | null): Promise<Residente[]> {
    let params = new HttpParams();
    if (soloPropietarios !== undefined && soloPropietarios !== null) {
      params = params.set('soloPropietarios', soloPropietarios);
    }

    return firstValueFrom(
      this.http.get<Residente[]>(`/api/residentes/conjunto/${conjuntoId}`, { params })
    );
  }

  async create(draft: ResidenteDraft): Promise<string> {
    const response = await firstValueFrom(this.http.post<{ id: string }>('/api/residentes', draft));
    return response.id;
  }
}
