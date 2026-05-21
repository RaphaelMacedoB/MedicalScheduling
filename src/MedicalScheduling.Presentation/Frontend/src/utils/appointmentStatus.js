export const STATUS_LABELS = {
  Scheduled: 'Agendada',
  Confirmed: 'Confirmada',
  Completed: 'Concluída',
  Cancelled: 'Cancelada',
  NoShow: 'Falta',
};

export const STATUS_VARIANTS = {
  Scheduled: 'info',
  Confirmed: 'neutral',
  Completed: 'success',
  Cancelled: 'danger',
  NoShow: 'warning',
};

export function canConfirm(status) {
  return status === 'Scheduled';
}

export function canComplete(status) {
  return status === 'Confirmed';
}

export function canCancel(status) {
  return status === 'Scheduled' || status === 'Confirmed';
}
