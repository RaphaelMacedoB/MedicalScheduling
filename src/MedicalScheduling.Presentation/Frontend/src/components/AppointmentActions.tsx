import { useState } from 'react';
import {
  cancelAppointment,
  completeAppointment,
  confirmAppointment,
} from '../api/appointments';
import type { AppointmentDto } from '../types/api';
import { canCancel, canComplete, canConfirm } from '../utils/appointmentStatus';
import { Button } from './ui/Button';
import { ConfirmModal, Modal } from './ui/Modal';
import { FormField, TextArea } from './ui/FormField';

interface AppointmentActionsProps {
  appointment: AppointmentDto;
  onUpdated: () => void;
  compact?: boolean;
}

export function AppointmentActions({ appointment, onUpdated, compact }: AppointmentActionsProps) {
  const [loading, setLoading] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [cancelOpen, setCancelOpen] = useState(false);
  const [completeOpen, setCompleteOpen] = useState(false);
  const [notes, setNotes] = useState('');

  const run = async (action: string, fn: () => Promise<void>) => {
    setLoading(action);
    setError(null);
    try {
      await fn();
      onUpdated();
      setConfirmOpen(false);
      setCancelOpen(false);
      setCompleteOpen(false);
      setNotes('');
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Operação falhou');
    } finally {
      setLoading(null);
    }
  };

  const size = compact ? 'sm' : 'md';

  return (
    <div className={`appointment-actions ${compact ? 'appointment-actions--compact' : ''}`}>
      {error && <p className="error error--inline">{error}</p>}
      <div className="appointment-actions__buttons">
        {canConfirm(appointment.status) && (
          <Button size={size} variant="primary" onClick={() => setConfirmOpen(true)}>
            Confirmar
          </Button>
        )}
        {canComplete(appointment.status) && (
          <Button size={size} variant="success" onClick={() => setCompleteOpen(true)}>
            Concluir
          </Button>
        )}
        {canCancel(appointment.status) && (
          <Button size={size} variant="danger" onClick={() => setCancelOpen(true)}>
            Cancelar
          </Button>
        )}
      </div>

      <ConfirmModal
        open={confirmOpen}
        title="Confirmar consulta"
        message={`Confirmar a consulta de ${appointment.patientName} com ${appointment.doctorName}?`}
        confirmLabel="Confirmar"
        loading={loading === 'confirm'}
        onClose={() => setConfirmOpen(false)}
        onConfirm={() => run('confirm', () => confirmAppointment(appointment.id))}
      />

      <ConfirmModal
        open={cancelOpen}
        title="Cancelar consulta"
        message={`Cancelar a consulta de ${appointment.patientName}? Esta ação não pode ser desfeita.`}
        confirmLabel="Cancelar consulta"
        variant="danger"
        loading={loading === 'cancel'}
        onClose={() => setCancelOpen(false)}
        onConfirm={() => run('cancel', () => cancelAppointment(appointment.id))}
      />

      <Modal
        open={completeOpen}
        title="Concluir consulta"
        onClose={() => setCompleteOpen(false)}
        footer={
          <>
            <Button variant="ghost" onClick={() => setCompleteOpen(false)} disabled={!!loading}>
              Voltar
            </Button>
            <Button
              variant="success"
              loading={loading === 'complete'}
              onClick={() => run('complete', () => completeAppointment(appointment.id, notes))}
            >
              Concluir
            </Button>
          </>
        }
      >
        <FormField label="Observações (opcional)" hint="Máx. 500 caracteres">
          <TextArea
            value={notes}
            onChange={(e) => setNotes(e.target.value)}
            maxLength={500}
            rows={4}
            placeholder="Resumo da consulta..."
          />
        </FormField>
      </Modal>
    </div>
  );
}
