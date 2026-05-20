import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import {
  fetchAppointments,
  fetchDoctors,
  fetchPatients,
  fetchSpecialities,
} from '../api/pagedFetchers';
import { StatusBadge } from '../components/StatusBadge';
import { Card } from '../components/ui/Card';
import { PageHeader } from '../components/ui/PageHeader';
import { Spinner } from '../components/ui/Spinner';
import type { AppointmentDto } from '../types/api';
import { formatDateTime } from '../utils/datetime';

interface Stats {
  patients: number;
  doctors: number;
  specialities: number;
  appointments: number;
  scheduled: number;
}

export function DashboardPage() {
  const [stats, setStats] = useState<Stats | null>(null);
  const [recent, setRecent] = useState<AppointmentDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;
    (async () => {
      setLoading(true);
      setError(null);
      try {
        const [patients, doctors, specialities, appointments, scheduled] = await Promise.all([
          fetchPatients({ page: 1, pageSize: 1 }),
          fetchDoctors({ page: 1, pageSize: 1 }),
          fetchSpecialities({ page: 1, pageSize: 1 }),
          fetchAppointments({ page: 1, pageSize: 1 }),
          fetchAppointments({ page: 1, pageSize: 5, status: 'Scheduled', sortBy: 'start', sortDirection: 'Asc' }),
        ]);
        if (cancelled) return;
        setStats({
          patients: patients.totalItems,
          doctors: doctors.totalItems,
          specialities: specialities.totalItems,
          appointments: appointments.totalItems,
          scheduled: scheduled.totalItems,
        });
        const upcoming = await fetchAppointments({
          page: 1,
          pageSize: 6,
          sortBy: 'start',
          sortDirection: 'Asc',
        });
        if (!cancelled) setRecent(upcoming.items);
      } catch (err) {
        if (!cancelled) setError(err instanceof Error ? err.message : 'Erro ao carregar painel');
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();
    return () => {
      cancelled = true;
    };
  }, []);

  return (
    <section>
      <PageHeader
        title="Painel administrativo"
        subtitle="Visão geral do sistema de agendamento médico"
        actions={
          <>
            <Link to="/appointments" className="btn btn--primary">
              Gerenciar consultas
            </Link>
            <Link to="/agenda" className="btn btn--secondary">
              Ver agendas
            </Link>
          </>
        }
      />

      {loading && <Spinner />}
      {error && <p className="error">{error}</p>}

      {stats && (
        <>
          <div className="stats-grid">
            <Card className="stat-card stat-card--patients">
              <span className="stat-card__label">Pacientes</span>
              <span className="stat-card__value">{stats.patients}</span>
              <Link to="/patients" className="stat-card__link">
                Ver lista →
              </Link>
            </Card>
            <Card className="stat-card stat-card--doctors">
              <span className="stat-card__label">Médicos</span>
              <span className="stat-card__value">{stats.doctors}</span>
              <Link to="/doctors" className="stat-card__link">
                Ver lista →
              </Link>
            </Card>
            <Card className="stat-card stat-card--specialities">
              <span className="stat-card__label">Especialidades</span>
              <span className="stat-card__value">{stats.specialities}</span>
              <Link to="/specialities" className="stat-card__link">
                Ver lista →
              </Link>
            </Card>
            <Card className="stat-card stat-card--appointments">
              <span className="stat-card__label">Consultas</span>
              <span className="stat-card__value">{stats.appointments}</span>
              <span className="stat-card__hint">{stats.scheduled} agendadas</span>
            </Card>
          </div>

          <Card className="recent-card" padding="lg">
            <div className="recent-card__header">
              <h3>Próximas consultas</h3>
              <Link to="/appointments">Ver todas</Link>
            </div>
            {recent.length === 0 ? (
              <p className="muted">Nenhuma consulta cadastrada.</p>
            ) : (
              <ul className="recent-list">
                {recent.map((a) => (
                  <li key={a.id} className="recent-list__item">
                    <div>
                      <strong>{a.patientName}</strong>
                      <span className="recent-list__meta">
                        {a.doctorName} · {a.specialityName}
                      </span>
                      <span className="recent-list__time">{formatDateTime(a.start)}</span>
                    </div>
                    <StatusBadge status={a.status} />
                  </li>
                ))}
              </ul>
            )}
          </Card>
        </>
      )}
    </section>
  );
}
