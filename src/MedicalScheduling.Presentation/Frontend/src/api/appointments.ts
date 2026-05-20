import type { PagedListParams } from './pagedFetchers';
import { apiRequest, getPaged } from './client';
import type { AppointmentDto, ScheduleAppointmentCommand } from '../types/api';

export async function scheduleAppointment(
  command: ScheduleAppointmentCommand,
): Promise<AppointmentDto> {
  return apiRequest<AppointmentDto>('/appointments', {
    method: 'POST',
    body: JSON.stringify(command),
  });
}

export async function confirmAppointment(id: string): Promise<void> {
  await apiRequest<void>(`/appointments/${id}/confirm`, { method: 'PATCH' });
}

export async function completeAppointment(id: string, notes?: string): Promise<void> {
  await apiRequest<void>(`/appointments/${id}/complete`, {
    method: 'PATCH',
    body: JSON.stringify(notes ?? ''),
  });
}

export async function cancelAppointment(id: string): Promise<void> {
  await apiRequest<void>(`/appointments/${id}/cancel`, { method: 'PATCH' });
}

export function fetchDoctorAppointments(doctorId: string) {
  return (params: PagedListParams) =>
    getPaged<AppointmentDto>(`/appointments/doctor/${doctorId}`, {
      ...params,
      sortBy: params.sortBy ?? 'start',
      sortDirection: params.sortDirection ?? 'Asc',
    });
}
