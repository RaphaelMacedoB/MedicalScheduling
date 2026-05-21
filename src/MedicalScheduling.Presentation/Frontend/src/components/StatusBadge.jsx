import { Badge } from './ui/Badge';
import { STATUS_LABELS, STATUS_VARIANTS } from '../utils/appointmentStatus';

export function StatusBadge({ status }) {
  return <Badge variant={STATUS_VARIANTS[status]}>{STATUS_LABELS[status]}</Badge>;
}
