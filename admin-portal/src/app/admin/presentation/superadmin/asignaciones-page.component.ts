import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

interface AdminItem {
  id: string;
  cedula: string;
  nombre: string;
  apellido: string;
  activo: boolean;
  conjuntos: {
    conjuntoId: string;
    nombre?: string;
    activo: boolean;
    conjuntoActivo: boolean;
  }[];
}

interface ConjuntoItem {
  id: string;
  nombre: string;
  activo: boolean;
}

@Component({
  selector: 'app-asignaciones-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './asignaciones-page.component.html',
  styleUrl: './asignaciones-page.component.scss'
})
export class AsignacionesPageComponent {
  private readonly http = inject(HttpClient);

  readonly administradores = signal<AdminItem[]>([]);
  readonly conjuntos = signal<ConjuntoItem[]>([]);
  readonly cargando = signal(false);
  readonly guardando = signal(false);
  readonly error = signal('');
  readonly mensaje = signal('');

  administradorId = '';
  conjuntoId = '';

  constructor() {
    this.cargarDatos();
  }

  cargarDatos(): void {
    this.cargando.set(true);
    this.error.set('');

    let pendientes = 2;
    const finalizar = (): void => {
      pendientes--;
      if (pendientes === 0) this.cargando.set(false);
    };

    this.http.get<AdminItem[]>('/api/adminconjuntos').subscribe({
      next: (data) => this.administradores.set(data ?? []),
      error: () => this.error.update((e) =>
        `${e ? e + ' ' : ''}No fue posible cargar administradores.`),
      complete: finalizar
    });

    this.http.get<ConjuntoItem[]>('/api/conjuntos').subscribe({
      next: (data) => this.conjuntos.set(
        (data ?? []).filter((c) => c.activo)
      ),
      error: () => this.error.update((e) =>
        `${e ? e + ' ' : ''}No fue posible cargar conjuntos.`),
      complete: finalizar
    });
  }

  asignar(): void {
    if (!this.administradorId || !this.conjuntoId || this.guardando()) {
      this.error.set('Selecciona un administrador y un conjunto.');
      return;
    }

    this.guardando.set(true);
    this.error.set('');
    this.mensaje.set('');

    this.http.post('/api/adminconjuntos/asignaciones', {
      adminConjuntoId: this.administradorId,
      conjuntoId: this.conjuntoId
    }).subscribe({
      next: () => {
        this.mensaje.set('Asignación creada correctamente.');
        this.administradorId = '';
        this.conjuntoId = '';
        this.guardando.set(false);
        this.cargarDatos();
      },
      error: (err) => {
        this.error.set(err?.error?.error ?? err?.error?.Error ??
          'No fue posible crear la asignación.');
        this.guardando.set(false);
      }
    });
  }

  nombreAdministrador(admin: AdminItem): string {
    return `${admin.nombre ?? ''} ${admin.apellido ?? ''}`.trim() || admin.cedula;
  }

  conjuntosActivos(admin: AdminItem): string {
    const nombres = (admin.conjuntos ?? [])
      .filter((c) => c.activo && c.conjuntoActivo)
      .map((c) => c.nombre)
      .filter((n): n is string => !!n);

    return nombres.join(', ') || 'Sin conjuntos activos';
  }
}
