
import { configureStore } from "@reduxjs/toolkit";
import { authApi } from "../features/authApiSlice";
import authReducer from "../features/authSlice";
import formReducer from "../features/formSlice";
import { studentApi } from "../features/studentApiSlice";
import { setupListeners } from "@reduxjs/toolkit/query";

export const store= configureStore({
    reducer:{
       // Stores RTK Query's API state and cached data in Redux.
    [studentApi.reducerPath]:studentApi.reducer, 
    [authApi.reducerPath]:authApi.reducer, // Stores our application's form-related state.
    form:formReducer,  
    auth:authReducer,
    },
  // Add RTK Query middleware alongside Redux Toolkit's default middleware.
// This enables API request handling, caching, refetching, polling,
// cache invalidation, and other RTK Query features.
  middleware:(getDefaultMiddleware)=>getDefaultMiddleware().concat(studentApi.middleware,authApi.middleware)}
);
//enables refethconFocus/ refetchOnReconnect 
//RTK Query can automatically refire queries on window focus/reconnect, but only if you enable listeners once, at the store level
setupListeners(store.dispatch);

export type AppDispatch = typeof store.dispatch;
export type RootState=ReturnType<typeof store.getState>