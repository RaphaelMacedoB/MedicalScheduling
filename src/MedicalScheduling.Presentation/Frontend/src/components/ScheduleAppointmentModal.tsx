import { useEffect, useState, type FormEvent } from 'react';
import { scheduleAppointment } from '../api/appointments';
import { useActiveDoctors, useActivePatients } from '../hooks/useActiveOptions';
import { defaultEndFromStart, localInputToIso, minScheduleLocal } from '../utils/datetime';
import { Button } from './ui/Button';
import { FormField, Input, Select } from './ui/FormField';
import { Modal } from './ui/Modal';
import { Spinner } from './ui/Spinner';

interface ScheduleAppointmentModalProps {
  open: boolean;
  onClose: () => void;
  onScheduled: () => void;
  defaultDoctorId?: string;
}

export function ScheduleAppointmentModal({
  open,
  onClose,
  onScheduled,
  defaultDoctorId,
}: ScheduleAppointmentModalProps) {
  const { patients, loading: loadingPatients, error: patientsError } = useActivePatients();
  const { doctors, loading: loadingDoctors, error: doctorsError } = useActiveDoctors();

  const [patientId, setPatientId] = useState('');
  const [doctorId, setDoctorId] = useState(defaultDoctorId ?? '');
  const [start, setStart] = useState(minScheduleLocal());
  const [end, setEnd] = useState(defaultEndFromStart(minScheduleLocal()));
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (open && defaultDoctorId) {
      setDoctorId(defaultDoctorId);
    }
  }, [open, defaultDoctorId]);

  const loadingOptions = loadingPatients || loadingDoctors;
  const optionsError = patientsError ?? doctorsError;

  const handleStartChange = (value: string) => {
    setStart(value);
    setEnd(defaultEndFromStart(value));
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError(null);
    if (!patientId || !doctorId) {
      setError('Selecione paciente e médico.');
      return;
    }
    if (new Date(end) <= new Date(start)) {
      setError('O horário de término deve ser após o início.');
      return;
    }

    setSubmitting(true);
    try {
      await scheduleAppointment({
        patientId,
        doctorId,
        start: localInputToIso(start),
        end: localInputToIso(end),
      });
      onScheduled();
      onClose();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Não foi possível agendar a consulta.');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Modal
      open={open}
      title="Agendar consulta"
      onClose={onClose}
      wide
      footer={
        <>
          <Button variant="ghost" onClick={onClose} disabled={submitting}>
            Cancelar
          </Button>
          <Button type="submit" form="schedule-form" loading={submitting}>
            Agendar
          </Button>
        </>
      }
    >
      {loadingOptions ? (
        <Spinner label="Carregando opções..." />
      ) : (
        <form id="schedule-form" className="form-grid" onSubmit={handleSubmit}>
          {optionsError && <p className="error">{optionsError}</p>}
          {error && <p className="error">{error}</p>}

          <FormField label="Paciente">
            <Select
              value={patientId}
              onChange={(e) => setPatientId(e.target.value)}
              required
            >
              <option value="">Selecione o paciente</option>
              {patients.map((p) => (
                <option key={p.id} value={p.id}>
                  {p.name} — {p.cpf}
                </option>
              ))}
            </Select>
          </FormField>

          <FormField label="Médico">
            <Select
              value={doctorId || defaultDoctorId || ''}
              onChange={(e) => setDoctorId(e.target.value)}
              required
            >
              <option value="">Selecione o médico</option>
              {doctors.map((d) => (
                <option key={d.id} value={d.id}>
                  {d.name} — {d.specialityName} (CRM {d.crm})
                </option>
              ))}
            </Select>
          </FormField>

          <FormField label="Início" hint="Horário local">
            <Input
              type="datetime-local"
              value={start}
              min={minScheduleLocal()}
              onChange={(e) => handleStartChange(e.target.value)}
              required
            />
          </FormField>

          <FormField label="Término" hint="Horário local">
            <Input
              type="datetime-local"
              value={end}
              min={start}
              onChange={(e) => setEnd(e.target.value)}
              required
            />
          </FormField>
        </form>
      )}
    </Modal>
  );
}
