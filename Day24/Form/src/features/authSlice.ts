import { createSlice } from "@reduxjs/toolkit";
import { UserRole } from "../types/authTypes";


interface AuthState {
  email: string | null;
  role: UserRole | null;
  accessToken: string | null;
  accessTokenExpiresAt: string | null;
  refreshToken: string | null;
  refreshTokenExpiresAt: string | null;
}

const emptyAuthState: AuthState = {
  email: null,
  role: null,
  accessToken: null,
  accessTokenExpiresAt: null,
  refreshToken: null,
  refreshTokenExpiresAt: null,
};


const authSlice= createSlice({
    
})