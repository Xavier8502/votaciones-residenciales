export interface AdminNavigationItem {
  label: string;
  route: string;
  icon: 'dashboard' | 'conjuntos' | 'inmuebles' | 'residentes' | 'votaciones';
}

export const adminNavigation: AdminNavigationItem[] = [
  { label: 'Dashboard', route: '/admin/dashboard', icon: 'dashboard' },
  { label: 'Conjuntos', route: '/admin/conjuntos', icon: 'conjuntos' },
  { label: 'Inmuebles', route: '/admin/inmuebles', icon: 'inmuebles' },
  { label: 'Residentes', route: '/admin/residentes', icon: 'residentes' },
  { label: 'Votaciones', route: '/admin/votaciones', icon: 'votaciones' }
];
