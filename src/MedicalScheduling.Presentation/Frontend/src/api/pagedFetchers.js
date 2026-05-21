import { getPaged } from './client';

export const fetchPatients = (params) => getPaged('/patients', params);

export const fetchDoctors = (params) => getPaged('/doctors', params);

export const fetchSpecialities = (params) => getPaged('/specialities', params);

export const fetchAppointments = (params) => getPaged('/appointments', params);
