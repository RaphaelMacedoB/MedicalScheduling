import { useMemo, useState } from 'react';
import { AgendaTimeline } from '../components/AgendaTimeline';
import { ScheduleAppointmentModal } from '../components/ScheduleAppointmentModal';
import { Button } from '../components/ui/Button';
import { Card } from '../components/ui/Card';
import { FormField, Input, Select } from '../components/ui/FormField';
import { PageHeader } from '../components/ui/PageHeader';
import { Spinner } from '../components/ui/Spinner';
import { useActiveDoctors } from '../hooks/useActiveOptions';
import { useDoctorAgenda } from '../hooks/useDoctorAgenda';
import { toDateInputValue } from '../utils/datetime';

function defaultWeekRange(): { from: string; to: string } {
  const today = new Date();
  const end = new Date(today);
  end.setDate(end.getDate() + 13);
  return { from: toDateInputValue(today.toISOString()), to: toDateInputValue(end.toISOString()) };
}

export function DoctorAgendaPage() {
  const { doctors, loading: loadingDoctors } = useActiveDoctors();
  const range = useMemo(() => defaultWeekRange(), []);
  const [doctorId, setDoctorId] = useState('');
  const [dateFrom, setDateFrom] = useState(range.from);
  const [dateTo, setDateTo] = useState(range.to);
  const [scheduleOpen, setScheduleOpen] = useState(false);

  const selectedDoctor = doctors.find((d) => d.id === doctorId);
  const { appointments, loading, error, reload } = useDoctorAgenda(doctorId || null, dateFrom, dateTo);

  return (
    <section>
      <PageHeader
        title="Agenda dos médicos"
        subtitle="Visualize e gerencie os horários de cada profissional"
        actions={
          <Button
            variant="primary"
            onClick={() => setScheduleOpen(true)}
            disabled={!doctorId}
          >
            Agendar nesta agenda
          </Button>
        }
      />

      <Card className="agenda-filters" padding="md">
        <div className="form-grid form-grid--3">
          <FormField label="Médico">
            <Select
              value={doctorId}
              onChange={(e) => setDoctorId(e.target.value)}
              disabled={loadingDoctors}
            >
              <option value="">Selecione um médico</option>
              {doctors.map((d) => (
                <option key={d.id} value={d.id}>
                  {d.name} — {d.specialityName}
                </option>
              ))}
            </Select>
          </FormField>
          <FormField label="De">
            <Input type="date" value={dateFrom} onChange={(e) => setDateFrom(e.target.value)} />
          </FormField>
          <FormField label="Até">
            <Input
              type="date"
              value={dateTo}
              min={dateFrom}
              onChange={(e) => setDateTo(e.target.value)}
            />
          </FormField>
        </div>
        {selectedDoctor && (
          <p className="agenda-filters__info">
            <strong>{selectedDoctor.name}</strong> · CRM {selectedDoctor.crm} ·{' '}
            {selectedDoctor.specialityName}
          </p>
        )}
      </Card>

      {!doctorId && (
        <Card padding="lg">
          <p className="muted">Selecione um médico para exibir a agenda.</p>
        </Card>
      )}

      {doctorId && loading && <Spinner />}
      {error && <p className="error">{error}</p>}
      {doctorId && !loading && (
        <AgendaTimeline appointments={appointments} onUpdated={reload} />
      )}

      <ScheduleAppointmentModal
        open={scheduleOpen}
        onClose={() => setScheduleOpen(false)}
        onScheduled={reload}
        defaultDoctorId={doctorId}
      />
    </section>
  );
}
