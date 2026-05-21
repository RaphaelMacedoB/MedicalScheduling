import { apiRequest, getPaged } from './client';

export async function scheduleAppointment(command) {
  return apiRequest('/appointments', {
    method: 'POST',
    body: JSON.stringify(command),
  });
}

export async function confirmAppointment(id) {
  await apiRequest(`/appointments/${id}/confirm`, { method: 'PATCH' });
}

export async function completeAppointment(id, notes) {
  await apiRequest(`/appointments/${id}/complete`, {
    method: 'PATCH',
    body: JSON.stringify(notes ?? ''),
  });
}

export async function cancelAppointment(id) {
  await apiRequest(`/appointments/${id}/cancel`, { method: 'PATCH' });
}

export function fetchDoctorAppointments(doctorId) {
  return (params) =>
    getPaged(`/appointments/doctor/${doctorId}`, {
      ...params,
      sortBy: params.sortBy ?? 'start',
      sortDirection: params.sortDirection ?? 'Asc',
    });
}
