import axios from 'axios';

export const API_BASE = import.meta.env.VITE_API_URL ?? '/api/v1';

export const http = axios.create({
  baseURL: API_BASE,
  headers: { 'Content-Type': 'application/json' },
});

function cleanParams(params) {
  const result = {};
  for (const [key, value] of Object.entries(params)) {
    if (value === undefined || value === null || value === '') continue;
    result[key] = value;
  }
  return result;
}

export function getApiErrorMessage(err) {
  if (axios.isCancel(err)) return null;
  if (err instanceof Error) return err.message;
  return 'Erro desconhecido';
}

http.interceptors.response.use(
  (response) => {
    if (response.status === 204) return undefined;
    return response.data;
  },
  (error) => {
    if (axios.isCancel(error)) {
      return Promise.reject(error);
    }
    const data = error.response?.data;
    const message = data?.message || data?.code || error.message || 'Erro na requisição';
    return Promise.reject(new Error(message));
  },
);

export async function apiGet(path, config = {}) {
  return http.get(path, config);
}

export async function apiPost(path, data, config = {}) {
  return http.post(path, data, config);
}

export async function apiPatch(path, data, config = {}) {
  return http.patch(path, data, config);
}

export async function getPaged(path, params = {}, config = {}) {
  return http.get(path, {
    params: cleanParams(params),
    ...config,
  });
}
