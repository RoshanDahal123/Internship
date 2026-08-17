import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { FormEntry } from "../types/formTypes";


export interface FormState{
  entries:FormEntry[];
  loading:boolean;
  error:string|null;
}
const initialState:FormState={
    entries:[],
    loading:false,
    error:null

}


 const formSlice=createSlice({
    name:"form",
    initialState,
    reducers:{
     fetchStart:(state:FormState)=>{
        state.loading=true;
        state.error=null;
     },
     fetchSuccess:(state:FormState,action:PayloadAction<FormEntry[]>)=>{
        state.loading=false;
        state.entries=action.payload;
     },
     fetchFailure:(state:FormState,action:PayloadAction<string>)=>{
        state.loading=false;
        state.error=action.payload;
     },
     addEntryLocally:(state:FormState, action:PayloadAction<FormEntry>)=>{
        //immer lets yoy write directly
        state.entries.push(action.payload);
     },
     removeEntry:(state:FormState,action:PayloadAction<number>)=>{
        state.entries= state.entries.filter(entry=>entry.id!==action.payload)
     },
     removeAllEntries:(state:FormState)=>{
        state.entries=[];
     },
    },
});

export const {
    fetchStart,
    fetchSuccess,
  fetchFailure,
  addEntryLocally,
  removeEntry,
  removeAllEntries


}= formSlice.actions;

export default formSlice.reducer;