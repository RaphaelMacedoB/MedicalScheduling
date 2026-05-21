import { useState } from 'react';
import { fetchAppointments } from '../api/pagedFetchers';
import { APPOINTMENT_STATUSES } from '../constants/api';
import { AppointmentActions } from '../components/AppointmentActions';
import { ListToolbar } from '../components/ListToolbar';
import { Pagination } from '../components/Pagination';
import { ScheduleAppointmentModal } from '../components/ScheduleAppointmentModal';
import { StatusBadge } from '../components/StatusBadge';
import { Button } from '../components/ui/Button';
import { Card } from '../components/ui/Card';
import { PageHeader } from '../components/ui/PageHeader';
import { Select } from '../components/ui/FormField';
import { Spinner } from '../components/ui/Spinner';
import { usePagedList } from '../hooks/usePagedList';
import { STATUS_LABELS } from '../utils/appointmentStatus';
import { formatDateTime } from '../utils/datetime';

export function AppointmentsPage() {
  const [scheduleOpen, setScheduleOpen] = useState(false);
  const list = usePagedList({
    fetcher: fetchAppointments,
    initialPageSize: 10,
  });

  return (
    <section>
      <PageHeader
        title="Consultas"
        subtitle="Agende, confirme, conclua ou cancele consultas"
        actions={
          <Button variant="primary" onClick={() => setScheduleOpen(true)}>
            + Nova consulta
          </Button>
        }
      />

      <Card padding="md" className="toolbar-card">
        <ListToolbar
          search={list.search}
          onSearchChange={list.setSearch}
          onSearchSubmit={list.reload}
          sortBy={list.sortBy}
          sortDirection={list.sortDirection}
          sortOptions={[
            { value: 'start', label: 'Início' },
            { value: 'status', label: 'Status' },
            { value: 'patient', label: 'Paciente' },
            { value: 'doctor', label: 'Médico' },
          ]}
          onSortByChange={list.setSortBy}
          onSortDirectionChange={list.setSortDirection}
        >
          <Select
            value={String(list.filters.status ?? '')}
            onChange={(e) =>
              list.setFilter('status', e.target.value || undefined)
            }
          >
            <option value="">Todos os status</option>
            {APPOINTMENT_STATUSES.map((s) => (
              <option key={s} value={s}>
                {STATUS_LABELS[s]}
              </option>
            ))}
          </Select>
        </ListToolbar>
      </Card>

      {list.loading && <Spinner />}
      {list.error && <p className="error">{list.error}</p>}

      {list.data && (
        <>
          <div className="table-wrap">
            <table className="data-table">
              <thead>
                <tr>
                  <th>Paciente</th>
                  <th>Médico</th>
                  <th>Especialidade</th>
                  <th>Início</th>
                  <th>Fim</th>
                  <th>Status</th>
                  <th>Ações</th>
                </tr>
              </thead>
              <tbody>
                {list.data.items.map((a) => (
                  <tr key={a.id}>
                    <td>{a.patientName}</td>
                    <td>{a.doctorName}</td>
                    <td>{a.specialityName}</td>
                    <td>{formatDateTime(a.start)}</td>
                    <td>{formatDateTime(a.end)}</td>
                    <td>
                      <StatusBadge status={a.status} />
                    </td>
                    <td>
                      <AppointmentActions appointment={a} onUpdated={() => void list.refresh()} compact />
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
          <Pagination
            currentPage={list.data.currentPage}
            totalPages={list.data.totalPages}
            totalItems={list.data.totalItems}
            pageSize={list.data.pageSize}
            hasNextPage={list.data.hasNextPage}
            hasPreviousPage={list.data.hasPreviousPage}
            onPageChange={list.setPage}
            onPageSizeChange={list.setPageSizeAndReset}
          />
        </>
      )}

      <ScheduleAppointmentModal
        open={scheduleOpen}
        onClose={() => setScheduleOpen(false)}
        onScheduled={() => void list.refresh()}
      />
    </section>
  );
}
