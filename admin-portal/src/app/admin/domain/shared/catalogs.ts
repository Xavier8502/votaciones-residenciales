export interface OptionItem<TValue extends string | number> {
  label: string;
  value: TValue;
}

export const tipoInmuebleOptions: OptionItem<number>[] = [
  { label: 'Apartamento', value: 1 },
  { label: 'Casa', value: 2 },
  { label: 'Local', value: 3 },
  { label: 'Parqueadero', value: 4 },
  { label: 'Deposito', value: 5 }
];

export const tipoPesoOptions: OptionItem<number>[] = [
  { label: 'Igualitario', value: 1 },
  { label: 'Por coeficiente', value: 2 }
];

export const estadoVotacionOptions: OptionItem<number>[] = [
  { label: 'Borrador', value: 1 },
  { label: 'Abierta', value: 2 },
  { label: 'Cerrada', value: 3 },
  { label: 'Acta generada', value: 4 },
  { label: 'Anulada', value: 5 }
];
