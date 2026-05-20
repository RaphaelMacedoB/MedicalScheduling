export type SortDirection = 'Asc' | 'Desc';

export interface PagedResponse<T> {
  items: T[];
  totalItems: number;
  totalPages: number;
  currentPage: number;
  pageSize: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface PagedQueryParams {
  page?: number;
  pageSize?: number;
  search?: string;
  sortBy?: string;
  sortDirection?: SortDirection;
}

export interface PatientDto {
  id: string;
  name: string;
  email: string;
  cpf: string;
  phone: string;
  birthDate: string;
  isActive: boolean;
  createdAt: string;
}

export interface DoctorDto {
  id: string;
  name: string;
  crm: string;
  email: string;
  phone: string;
  specialityId: string;
  specialityName: string;
  isActive: boolean;
  createdAt: string;
}

export interface SpecialityDto {
  id: string;
  name: string;
  description: string | null;
  createdAt: string;
}

export type AppointmentStatus =
  | 'Scheduled'
  | 'Confirmed'
  | 'Completed'
  | 'Cancelled'
  | 'NoShow';

export const APPOINTMENT_STATUSES: AppointmentStatus[] = [
  'Scheduled',
  'Confirmed',
  'Completed',
  'Cancelled',
  'NoShow',
];

export interface AppointmentDto {
  id: string;
  patientId: string;
  patientName: string;
  doctorId: string;
  doctorName: string;
  specialityName: string;
  start: string;
  end: string;
  status: AppointmentStatus;
  notes: string | null;
  createdAt: string;
}

export interface ScheduleAppointmentCommand {
  patientId: string;
  doctorId: string;
  start: string;
  end: string;
}

export interface AppointmentFilterParams extends PagedQueryParams {
  patientId?: string;
  doctorId?: string;
  specialityId?: string;
  status?: AppointmentStatus;
  startFrom?: string;
  startTo?: string;
}

export interface ApiError {
  code: string;
  message: string;
}
