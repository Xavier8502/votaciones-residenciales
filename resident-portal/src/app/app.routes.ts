import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { residentGuard } from './core/guards/resident.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./resident/presentation/auth/login-page.component').then(
        (m) => m.LoginPageComponent
      )
  },
  {
    path: 'app',
    canActivate: [authGuard, residentGuard],
    loadComponent: () =>
      import('./resident/presentation/layout/resident-shell.component').then(
        (m) => m.ResidentShellComponent
      ),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'inicio' },
      {
        path: 'inicio',
        loadComponent: () =>
          import('./resident/presentation/home/home-page.component').then(
            (m) => m.HomePageComponent
          )
      },
      {
        path: 'votaciones',
        loadComponent: () =>
          import('./resident/presentation/votaciones/votaciones-page.component').then(
            (m) => m.VotacionesPageComponent
          )
      },
      {
        path: 'votaciones/:id',
        loadComponent: () =>
          import('./resident/presentation/votaciones/votacion-detail-page.component').then(
            (m) => m.VotacionDetailPageComponent
          )
      },
      {
        path: 'perfil',
        loadComponent: () =>
          import('./resident/presentation/perfil/perfil-page.component').then(
            (m) => m.PerfilPageComponent
          )
      }
    ]
  },
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  { path: '**', redirectTo: 'login' }
];
