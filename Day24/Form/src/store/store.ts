
import { configureStore } from "@reduxjs/toolkit";
import formReducer from "../features/formSlice";
import { studentApi } from "../features/studentApiSlice";


export const store= configureStore({
    reducer:{
       // Stores RTK Query's API state and cached data in Redux.
    [studentApi.reducerPath]:studentApi.reducer,    // Stores our application's form-related state.
    form:formReducer,  
    },
  // Add RTK Query middleware alongside Redux Toolkit's default middleware.
// This enables API request handling, caching, refetching, polling,
// cache invalidation, and other RTK Query features.
  middleware:(getDefaultMiddleware)=>getDefaultMiddleware().concat(studentApi.middleware)}
);

export type AppDispatch = typeof store.dispatch;
export type RootState=ReturnType<typeof store.getState>