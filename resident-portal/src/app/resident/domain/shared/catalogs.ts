export const residentVoteFilters = [
  { id: 'all', label: 'Todas' },
  { id: 'open', label: 'Abiertas' },
  { id: 'closed', label: 'Cerradas' }
] as const;

export type ResidentVoteFilter = (typeof residentVoteFilters)[number]['id'];
