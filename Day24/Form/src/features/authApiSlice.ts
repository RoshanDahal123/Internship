import { baseApi } from "../app/base-api";
import {
  AuthResponse,
  LoginRequest,
  RegisterRequest,
  UserRole,
} from "../types/authTypes";
import { clearCredentials } from "./authSlice";

export interface MeResponse {
  email: string;
  role: UserRole | null;
}

export const authApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    login: builder.mutation<AuthResponse, LoginRequest>({
      query: (body) => ({ url: "/auth/login", method: "POST", data: body }),
      invalidatesTags: ["Auth"],
    }),
    register: builder.mutation<AuthResponse, RegisterRequest>({
      query: (body) => ({ url: "/auth/register", method: "POST", data: body }),
    }),
    // refresh: builder.mutation<AuthResponse, void>({
    //   query: () => ({ url: "/auth/refresh", method: "POST" }),
    // }),
    logout: builder.mutation<void, void>({
      query: () => ({ url: "/auth/logout", method: "POST" }),
      
  async onQueryStarted(_arg,{dispatch,queryFulfilled}) {
    try{
      await queryFulfilled;
      dispatch(clearCredentials());
      dispatch(baseApi.util.resetApiState());
    }catch{
      dispatch(clearCredentials());
    }
  }
 }),
    getMe: builder.query<MeResponse, void>({
      query: () => ({url:"/auth/me"}),
      providesTags: ["Auth"],
    }),
  }),
  overrideExisting: false,
});

export const {
  useLoginMutation,
  useRegisterMutation,
  // useRefreshMutation,
  useLogoutMutation,
  useGetMeQuery,
} = authApi;