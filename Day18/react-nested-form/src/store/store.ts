import {configureStore} from "@reduxjs/toolkit";

import employerHistoryReducer from "./employerHistorySlice";


export const store=configureStore({

    reducer:{
        employerHistory:employerHistoryReducer,
    }
}
)

export type RootState= ReturnType<typeof store.getState>;
export type AppDispatch= typeof store.dispatch;