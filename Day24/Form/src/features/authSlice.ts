import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { AuthResponse, UserRole } from "../types/authTypes";


interface AuthState {
  email: string | null;
  role: UserRole | null;
  accessToken: string | null;
  accessTokenExpiresAt: string | null;
}

const initialState: AuthState = {
  email: null,
  role: null,
  accessToken: null,
  accessTokenExpiresAt: null,
};


const authSlice= createSlice({
 name: "auth",
  initialState,
  reducers: {
    setCredentials: (state, action: PayloadAction<AuthResponse>) => {
      state.email = action.payload.email;
      state.role = action.payload.role;
      state.accessToken = action.payload.accessToken;
      state.accessTokenExpiresAt = action.payload.accessTokenExpiresAt;
    },
    clearCredentials: (state) => {
      state.email = null;
      state.role = null;
      state.accessToken = null;
      state.accessTokenExpiresAt = null;
    },
  },
})

export const {setCredentials, clearCredentials}= authSlice.actions;

export default authSlice.reducer;


export const selectIsAuthenticated=(state:{auth:AuthState})=> !! state.auth.accessToken;
export const selectIsAdmin= (state:{auth:AuthState})=>state.auth.role==="Admin";
export const selectAuthEmail=(state:{auth:AuthState})=>state.auth.email;
export const selectAccessTokenExpiresAt = (state: { auth: AuthState }) =>
  state.auth.accessTokenExpiresAt;