import { apiPatch, apiPost, getPaged } from './client';

export async function scheduleAppointment(command, config) {
  return apiPost('/appointments', command, config);
}

export async function confirmAppointment(id, config) {
  await apiPatch(`/appointments/${id}/confirm`, undefined, config);
}

export async function completeAppointment(id, notes, config) {
  await apiPatch(`/appointments/${id}/complete`, notes ?? '', config);
}

export async function cancelAppointment(id, config) {
  await apiPatch(`/appointments/${id}/cancel`, undefined, config);
}

export function fetchDoctorAppointments(doctorId) {
  return (params, config) =>
    getPaged(`/appointments/doctor/${doctorId}`, {
      ...params,
      sortBy: params.sortBy ?? 'start',
      sortDirection: params.sortDirection ?? 'Asc',
    }, config);
}
