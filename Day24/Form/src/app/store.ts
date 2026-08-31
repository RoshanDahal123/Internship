import { configureStore } from "@reduxjs/toolkit";
import { setupListeners } from "@reduxjs/toolkit/query";
import { baseApi } from "../app/base-api";
import authReducer from "../features/authSlice";
import formReducer from "../features/formSlice";

// authApi and studentApi are just injected endpoints on baseApi —
// import them so their endpoints get registered, but don't add
// separate reducer/middleware entries for them.
import "../features/authApiSlice";
import "../features/studentApiSlice";

export const store = configureStore({
  reducer: {
    // Stores RTK Query's API state and cached data in Redux.
    [baseApi.reducerPath]: baseApi.reducer,
    form: formReducer,
    auth: authReducer,
  },
  // Add RTK Query middleware alongside Redux Toolkit's default middleware.
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(baseApi.middleware),
});

// Enables refetchOnFocus / refetchOnReconnect.
setupListeners(store.dispatch);

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;