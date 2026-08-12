import type { EmployerEntry, EmployerEntryFormValues } from "../schema/entry-form-schema";

import { createSlice, nanoid, type PayloadAction } from "@reduxjs/toolkit";


interface EmployerHistoryState{
    entries:EmployerEntry[]
}

const initialState:EmployerHistoryState={
    entries:[]
}

const employerHistorySlice = createSlice({
    name:"employerHistory",
    initialState,
    reducers:{
          // RTK uses Immer internally — this LOOKS like mutation but is safe;
    // it produces a new immutable state under the hood. Don't do this
    // with plain useReducer/Redux — only createSlice makes it safe.
    
        addEntry:{
            reducer:(state,action:PayloadAction<EmployerEntry>)=>{
                state.entries.push(action.payload)
            },
              // "prepare" lets the caller dispatch with just form values;
      // the id gets generated here, not left to the component to manage
            prepare:(data:EmployerEntryFormValues)=>({
                payload:{...data,id:nanoid()}
            })
        },

        updateEntry:(state,action:PayloadAction<EmployerEntry>)=>{
            const index = state.entries.findIndex((e)=>e.id === action.payload.id);
            if(index!==-1) state.entries[index]=action.payload;
        },

        removeEntry:(state,action:PayloadAction<string>)=>{
            state.entries= state.entries.filter((e)=>e.id!==action.payload)
        },
    },
});

export const {addEntry, updateEntry, removeEntry}= employerHistorySlice.actions;

export default employerHistorySlice.reducer;
