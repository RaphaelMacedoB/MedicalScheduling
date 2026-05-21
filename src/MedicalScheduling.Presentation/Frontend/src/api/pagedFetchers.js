import { getPaged } from './client';

export const fetchPatients = (params, config) => getPaged('/patients', params, config);

export const fetchDoctors = (params, config) => getPaged('/doctors', params, config);

export const fetchSpecialities = (params, config) => getPaged('/specialities', params, config);

export const fetchAppointments = (params, config) => getPaged('/appointments', params, config);
