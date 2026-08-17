
import { configureStore } from "@reduxjs/toolkit";
import { formApi } from "../features/formApiSlice";
import formReducer from "../features/formSlice";


export const store= configureStore({
    reducer:{
       // Stores RTK Query's API state and cached data in Redux.
    [formApi.reducerPath]:formApi.reducer,
    // Stores our application's form-related state.
    form:formReducer,
  },
  // Add RTK Query middleware alongside Redux Toolkit's default middleware.
// This enables API request handling, caching, refetching, polling,
// cache invalidation, and other RTK Query features.
  middleware:(getDefaultMiddleware)=>getDefaultMiddleware().concat(formApi.middleware)}
);

export type AppDispatch = typeof store.dispatch;
export type RootState=ReturnType<typeof store.getState>