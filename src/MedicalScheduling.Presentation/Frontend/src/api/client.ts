import type { ApiError, PagedQueryParams, PagedResponse } from '../types/api';

export const API_BASE = import.meta.env.VITE_API_URL ?? '/api/v1';

function buildQuery(params: Record<string, string | number | boolean | undefined | null>): string {
  const searchParams = new URLSearchParams();
  for (const [key, value] of Object.entries(params)) {
    if (value === undefined || value === null || value === '') continue;
    searchParams.set(key, String(value));
  }
  const query = searchParams.toString();
  return query ? `?${query}` : '';
}

async function handleResponse<T>(response: Response): Promise<T> {
  if (response.ok) {
    if (response.status === 204) return undefined as T;
    return (await response.json()) as T;
  }

  let error: ApiError = { code: 'Unknown', message: response.statusText };
  try {
    error = (await response.json()) as ApiError;
  } catch {
    /* empty body */
  }

  throw new Error(error.message || error.code);
}

export async function apiRequest<T>(
  path: string,
  init: RequestInit = {},
): Promise<T> {
  const response = await fetch(`${API_BASE}${path}`, {
    headers: { 'Content-Type': 'application/json', ...init.headers },
    ...init,
  });
  return handleResponse<T>(response);
}

export async function getPaged<T>(
  path: string,
  params: PagedQueryParams & Record<string, string | number | boolean | undefined | null> = {},
): Promise<PagedResponse<T>> {
  return apiRequest<PagedResponse<T>>(`${path}${buildQuery(params)}`);
}
