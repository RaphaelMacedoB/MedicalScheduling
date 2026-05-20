import type { AppointmentStatus } from '../types/api';

export const STATUS_LABELS: Record<AppointmentStatus, string> = {
  Scheduled: 'Agendada',
  Confirmed: 'Confirmada',
  Completed: 'Concluída',
  Cancelled: 'Cancelada',
  NoShow: 'Falta',
};

export const STATUS_VARIANTS: Record<
  AppointmentStatus,
  'neutral' | 'info' | 'success' | 'warning' | 'danger'
> = {
  Scheduled: 'info',
  Confirmed: 'neutral',
  Completed: 'success',
  Cancelled: 'danger',
  NoShow: 'warning',
};

export function canConfirm(status: AppointmentStatus): boolean {
  return status === 'Scheduled';
}

export function canComplete(status: AppointmentStatus): boolean {
  return status === 'Confirmed';
}

export function canCancel(status: AppointmentStatus): boolean {
  return status === 'Scheduled' || status === 'Confirmed';
}
