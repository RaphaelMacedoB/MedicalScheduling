import axios from 'axios';
import { useCallback, useEffect, useRef, useState } from 'react';
import { getApiErrorMessage } from '../api/client';
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
  const abortRef = useRef(null);

  const load = useCallback(async () => {
    abortRef.current?.abort();

    if (!doctorId) {
      setData(null);
      setLoading(false);
      setError(null);
      return;
    }

    const controller = new AbortController();
    abortRef.current = controller;

    setLoading(true);
    setError(null);
    try {
      const result = await fetchDoctorAppointments(doctorId)(
        {
          page: 1,
          pageSize: 100,
          startFrom: dayStartIso(dateFrom),
          startTo: dayEndIso(dateTo),
          sortBy: 'start',
          sortDirection: 'Asc',
        },
        { signal: controller.signal },
      );
      if (!controller.signal.aborted) {
        setData(result);
      }
    } catch (err) {
      if (axios.isCancel(err)) return;
      const message = getApiErrorMessage(err);
      if (message && !controller.signal.aborted) {
        setError(message);
        setData(null);
      }
    } finally {
      if (!controller.signal.aborted) {
        setLoading(false);
      }
    }
  }, [doctorId, dateFrom, dateTo]);

  useEffect(() => {
    void load();
    return () => abortRef.current?.abort();
  }, [load]);

  return { appointments: data?.items ?? [], loading, error, reload: load };
}
