import { createApi } from "@reduxjs/toolkit/query/react";
import { baseQueryWithReauth } from "../lib/base-query-with-reauth";
import {
    AuthResponse,
    LoginRequest,
    RegisterRequest,
    UserRole,
} from "../types/authTypes";

export interface MeResponse {
  email: string;
  role: UserRole | null;
}

export const authApi = createApi({
  reducerPath: "authApi",
  baseQuery: baseQueryWithReauth,
  endpoints: (builder) => ({
    login: builder.mutation<AuthResponse, LoginRequest>({
      query: (body) => ({ url: "/auth/login", method: "POST", body }),
    }),
    register: builder.mutation<AuthResponse, RegisterRequest>({
      query: (body) => ({ url: "/auth/register", method: "POST", body }),
    }),
    refresh: builder.mutation<AuthResponse, void>({
      query: () => ({ url: "/auth/refresh", method: "POST" }), // no body — cookie does the work
    }),
    logout: builder.mutation<void, void>({
      query: () => ({ url: "/auth/logout", method: "POST" }),
    }),
    getMe: builder.query<MeResponse, void>({
      query: () => "/auth/me",
    }),
  }),
});

export const {
  useLoginMutation,
  useRegisterMutation,
  useRefreshMutation,
  useLogoutMutation,
  useGetMeQuery
} = authApi;
