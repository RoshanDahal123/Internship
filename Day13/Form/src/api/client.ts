const BASE_URL =
  import.meta.env.VITE_API_BASE_URL ?? "https://localhost:7239/api";

// Custom error type so callers can distinguish "server rejected this"
// from a network failure, and read the status code if they need to.
export class ApiError extends Error {
  status: number;
  constructor(message: string, status: number) {
    super(message);
    this.name = "ApiError";
    this.status = status;
  }
}
// a 401 at the same moment, we call /refresh-token ONCE, not 5 times.
let refreshPromise: Promise<boolean> | null = null;

async function refreshAccessToken(): Promise<boolean> {
  if (!refreshPromise) {
    refreshPromise = fetch(`${BASE_URL}/auth/refresh-token`, {
      method: "POST",
      credentials: "include",
    })
      .then((res) => res.ok)
      .finally(() => {
        refreshPromise = null; // reset so the next 401 can trigger a fresh attempt
      });
  }
  return refreshPromise;
}

async function rawRequest(
  path: string,
  options: RequestInit,
): Promise<Response> {
  return fetch(`${BASE_URL}${path}`, {
    ...options,
    credentials: "include",
    headers: { "Content-Type": "application/json", ...options.headers },
  });
}
export async function postJson<T>(
  path: string,
  body: unknown,
  _isRetry = false,
): Promise<T> {
  const response = await rawRequest(path, {
    method: "POST",
    body: JSON.stringify(body),
  });
  // Don't try to refresh when the failing call IS the refresh call, or
  // when the failing call is login/register (a 401 there just means
  // wrong credentials, not an expired session).
  const isAuthEndpoint =
    path === "/auth/refresh-token" || path === "/auth/login" || path;
  if (response.status === 401 && !isAuthEndpoint && !_isRetry) {
    const refreshed = await refreshAccessToken();
    if (refreshed) {
      return postJson<T>(path, body, true); // retry once with the new access token cookie
    }
    // refresh failed too → session is genuinely dead
    throw new ApiError("Session expired. Please log in again.", 401);
  }
  if (!response.ok) {
    const message = await response
      .json()
      .then((data) => data?.message)
      .catch(() => null);
    throw new ApiError(
      message ?? `Request failed with status ${response.status}`,
      response.status,
    );
  }

  return response.json();
}
