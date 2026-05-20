import { getPaged } from './client';
import type {
  AppointmentDto,
  DoctorDto,
  PatientDto,
  PagedQueryParams,
  PagedResponse,
  SpecialityDto,
} from '../types/api';

export type PagedListParams = PagedQueryParams &
  Record<string, string | number | boolean | undefined | null>;

export const fetchPatients = (params: PagedListParams): Promise<PagedResponse<PatientDto>> =>
  getPaged<PatientDto>('/patients', params);

export const fetchDoctors = (params: PagedListParams): Promise<PagedResponse<DoctorDto>> =>
  getPaged<DoctorDto>('/doctors', params);

export const fetchSpecialities = (params: PagedListParams): Promise<PagedResponse<SpecialityDto>> =>
  getPaged<SpecialityDto>('/specialities', params);

export const fetchAppointments = (params: PagedListParams): Promise<PagedResponse<AppointmentDto>> =>
  getPaged<AppointmentDto>('/appointments', params);
