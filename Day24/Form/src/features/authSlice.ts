import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { UserRole } from "../types/authTypes";

export interface Credentials {
  email: string;
  role: UserRole | null;
}
interface AuthState {
  email: string | null;
  role: UserRole | null;
  isAuthenticated:Boolean;
}

const initialState: AuthState = {
  email: null,
  role: null,
  isAuthenticated:false
};


const authSlice= createSlice({
 name: "auth",
  initialState,
  reducers: {
    setCredentials: (state, action: PayloadAction<Credentials>) => {
      state.email = action.payload.email;
      state.role = action.payload.role;
      state.isAuthenticated=true
    },
    clearCredentials: (state) => {
      state.email = null;
      state.role = null;
      state.isAuthenticated=false;
    },
  },
})

export const {setCredentials, clearCredentials}= authSlice.actions;

export default authSlice.reducer;


export const selectIsAuthenticated = (
  state: {auth:AuthState}
) =>state.auth.isAuthenticated;
export const selectIsAdmin= (state:{auth:AuthState})=>state.auth.role==="Admin";
export const selectAuthEmail=(state:{auth:AuthState})=>state.auth.email;
