import { Routes } from '@angular/router';
import { adminGuard } from './core/guards/admin.guard';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./admin/presentation/auth/login-page.component').then(
        (m) => m.LoginPageComponent
      )
  },
  {
    path: 'admin',
    canActivate: [authGuard, adminGuard],
    loadComponent: () =>
      import('./admin/presentation/layout/admin-shell.component').then(
        (m) => m.AdminShellComponent
      ),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./admin/presentation/dashboard/dashboard-page.component').then(
            (m) => m.DashboardPageComponent
          )
      },
      {
        path: 'conjuntos',
        loadComponent: () =>
          import('./admin/presentation/conjuntos/conjuntos-page.component').then(
            (m) => m.ConjuntosPageComponent
          )
      },
      {
        path: 'inmuebles',
        loadComponent: () =>
          import('./admin/presentation/inmuebles/inmuebles-page.component').then(
            (m) => m.InmueblesPageComponent
          )
      },
      {
        path: 'residentes',
        loadComponent: () =>
          import('./admin/presentation/residentes/residentes-page.component').then(
            (m) => m.ResidentesPageComponent
          )
      },
      {
        path: 'votaciones',
        loadComponent: () =>
          import('./admin/presentation/votaciones/votaciones-page.component').then(
            (m) => m.VotacionesPageComponent
          )
      }
    ]
  },
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  { path: '**', redirectTo: 'login' }
];
