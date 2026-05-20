import type { AppointmentDto } from '../types/api';
import { formatDate, formatTime } from '../utils/datetime';
import { AppointmentActions } from './AppointmentActions';
import { StatusBadge } from './StatusBadge';
import { Card } from './ui/Card';
import { EmptyState } from './ui/EmptyState';

interface AgendaTimelineProps {
  appointments: AppointmentDto[];
  onUpdated: () => void;
}

function groupByDate(appointments: AppointmentDto[]): Map<string, AppointmentDto[]> {
  const groups = new Map<string, AppointmentDto[]>();
  for (const a of appointments) {
    const key = formatDate(a.start);
    const list = groups.get(key) ?? [];
    list.push(a);
    groups.set(key, list);
  }
  return groups;
}

export function AgendaTimeline({ appointments, onUpdated }: AgendaTimelineProps) {
  if (appointments.length === 0) {
    return (
      <EmptyState
        title="Nenhuma consulta no período"
        description="Altere as datas ou agende uma nova consulta."
      />
    );
  }

  const groups = groupByDate(appointments);

  return (
    <div className="agenda-timeline">
      {[...groups.entries()].map(([date, items]) => (
        <section key={date} className="agenda-day">
          <h3 className="agenda-day__title">{date}</h3>
          <div className="agenda-day__slots">
            {items.map((a) => (
              <Card key={a.id} className="agenda-slot" padding="md">
                <div className="agenda-slot__time">
                  <span>{formatTime(a.start)}</span>
                  <span className="agenda-slot__sep">—</span>
                  <span>{formatTime(a.end)}</span>
                </div>
                <div className="agenda-slot__body">
                  <div className="agenda-slot__main">
                    <strong>{a.patientName}</strong>
                    <span className="agenda-slot__meta">{a.specialityName}</span>
                  </div>
                  <StatusBadge status={a.status} />
                </div>
                {a.notes && <p className="agenda-slot__notes">{a.notes}</p>}
                <AppointmentActions appointment={a} onUpdated={onUpdated} compact />
              </Card>
            ))}
          </div>
        </section>
      ))}
    </div>
  );
}
