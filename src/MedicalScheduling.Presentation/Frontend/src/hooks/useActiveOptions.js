import axios from 'axios';
import { useEffect, useRef, useState } from 'react';
import { getApiErrorMessage } from '../api/client';
import { fetchDoctors, fetchPatients } from '../api/pagedFetchers';

function useActiveList(fetcher, emptyErrorMessage) {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const abortRef = useRef(null);

  useEffect(() => {
    abortRef.current?.abort();
    const controller = new AbortController();
    abortRef.current = controller;

    (async () => {
      setLoading(true);
      setError(null);
      try {
        const result = await fetcher(
          { page: 1, pageSize: 100, isActive: true },
          { signal: controller.signal },
        );
        if (!controller.signal.aborted) {
          setItems(result.items);
        }
      } catch (err) {
        if (axios.isCancel(err)) return;
        const message = getApiErrorMessage(err);
        if (message && !controller.signal.aborted) {
          setError(message || emptyErrorMessage);
        }
      } finally {
        if (!controller.signal.aborted) {
          setLoading(false);
        }
      }
    })();

    return () => abortRef.current?.abort();
  }, [fetcher, emptyErrorMessage]);

  return { items, loading, error };
}

export function useActivePatients() {
  const { items, loading, error } = useActiveList(
    fetchPatients,
    'Erro ao carregar pacientes',
  );
  return { patients: items, loading, error };
}

export function useActiveDoctors() {
  const { items, loading, error } = useActiveList(
    fetchDoctors,
    'Erro ao carregar médicos',
  );
  return { doctors: items, loading, error };
}
