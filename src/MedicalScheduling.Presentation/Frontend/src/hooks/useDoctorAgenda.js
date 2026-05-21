import { useCallback, useEffect, useState } from 'react';
import { fetchDoctorAppointments } from '../api/appointments';

function dayStartIso(date) {
  return new Date(`${date}T00:00:00`).toISOString();
}

function dayEndIso(date) {
  return new Date(`${date}T23:59:59.999`).toISOString();
}

export function useDoctorAgenda(doctorId, dateFrom, dateTo) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const load = useCallback(async () => {
    if (!doctorId) {
      setData(null);
      setLoading(false);
      return;
    }

    setLoading(true);
    setError(null);
    try {
      const result = await fetchDoctorAppointments(doctorId)({
        page: 1,
        pageSize: 100,
        startFrom: dayStartIso(dateFrom),
        startTo: dayEndIso(dateTo),
        sortBy: 'start',
        sortDirection: 'Asc',
      });
      setData(result);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao carregar agenda');
      setData(null);
    } finally {
      setLoading(false);
    }
  }, [doctorId, dateFrom, dateTo]);

  useEffect(() => {
    void load();
  }, [load]);

  return { appointments: data?.items ?? [], loading, error, reload: load };
}
