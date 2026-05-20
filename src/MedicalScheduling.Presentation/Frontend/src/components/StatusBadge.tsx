import { Badge } from './ui/Badge';
import type { AppointmentStatus } from '../types/api';
import { STATUS_LABELS, STATUS_VARIANTS } from '../utils/appointmentStatus';

interface StatusBadgeProps {
  status: AppointmentStatus;
}

export function StatusBadge({ status }: StatusBadgeProps) {
  return <Badge variant={STATUS_VARIANTS[status]}>{STATUS_LABELS[status]}</Badge>;
}
