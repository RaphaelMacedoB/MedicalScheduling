export const API_BASE = import.meta.env.VITE_API_URL ?? '/api/v1';

function buildQuery(params) {
  const searchParams = new URLSearchParams();
  for (const [key, value] of Object.entries(params)) {
    if (value === undefined || value === null || value === '') continue;
    searchParams.set(key, String(value));
  }
  const query = searchParams.toString();
  return query ? `?${query}` : '';
}

async function handleResponse(response) {
  if (response.ok) {
    if (response.status === 204) return undefined;
    return response.json();
  }

  let error = { code: 'Unknown', message: response.statusText };
  try {
    error = await response.json();
  } catch {
    /* empty body */
  }

  throw new Error(error.message || error.code);
}

export async function apiRequest(path, init = {}) {
  const response = await fetch(`${API_BASE}${path}`, {
    headers: { 'Content-Type': 'application/json', ...init.headers },
    ...init,
  });
  return handleResponse(response);
}

export async function getPaged(path, params = {}) {
  return apiRequest(`${path}${buildQuery(params)}`);
}
