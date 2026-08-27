// src/types/authTypes.ts
export type UserRole = "Admin";

/** Shape returned by /auth/register, /auth/login, and /auth/refresh — identical on all three. */
export interface AuthResponse {
  accessToken: string;
  accessTokenExpiresAt: string; // ISO string string
  email: string;
  role: UserRole;
}

export interface LoginRequest {
  email: string;
  password: string;
}

// Deliberately does NOT include confirmPassword — that's a UI-only
// concern (see authSchemas.ts), not something the API needs.
export interface RegisterRequest {
  email: string;
  password: string;
  setupKey: string;
}

