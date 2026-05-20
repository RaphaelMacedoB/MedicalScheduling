import { useEffect, useState } from 'react';
import { fetchDoctors, fetchPatients } from '../api/pagedFetchers';
import type { DoctorDto, PatientDto } from '../types/api';

export function useActivePatients() {
  const [patients, setPatients] = useState<PatientDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;
    (async () => {
      setLoading(true);
      setError(null);
      try {
        const result = await fetchPatients({ page: 1, pageSize: 100, isActive: true });
        if (!cancelled) setPatients(result.items);
      } catch (err) {
        if (!cancelled) setError(err instanceof Error ? err.message : 'Erro ao carregar pacientes');
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();
    return () => {
      cancelled = true;
    };
  }, []);

  return { patients, loading, error };
}

export function useActiveDoctors() {
  const [doctors, setDoctors] = useState<DoctorDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;
    (async () => {
      setLoading(true);
      setError(null);
      try {
        const result = await fetchDoctors({ page: 1, pageSize: 100, isActive: true });
        if (!cancelled) setDoctors(result.items);
      } catch (err) {
        if (!cancelled) setError(err instanceof Error ? err.message : 'Erro ao carregar médicos');
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();
    return () => {
      cancelled = true;
    };
  }, []);

  return { doctors, loading, error };
}
