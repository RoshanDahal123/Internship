import { createSlice, type PayloadAction } from "@reduxjs/toolkit";

interface FormUIState{
   searchTerm:string; // filters the entries list in Display.tsx
   isDeleteAllModalOpen:boolean;//controls the confirm dialog
   entryPendingDeleteId:number|null;
}

const initialState:FormUIState={
   searchTerm:"",
   isDeleteAllModalOpen:false,
   entryPendingDeleteId:null

}


const formSlice= createSlice({
   name:"form",
   initialState,
   reducers:{
    setSearchTerm:(state:FormUIState, action:PayloadAction<string>)=>{
      state.searchTerm=action.payload;
    },
    clearSearchTerm:(state)=>{
      state.searchTerm=""
    },
    openDeleteAllModal: (state) => {
      state.isDeleteAllModalOpen = true;
    },

    closeDeleteAllModal:(state)=>{
      state.isDeleteAllModalOpen=false;
    },
    setEntryPendingDelete:(state,action:PayloadAction<number |null>)=>{
      state.entryPendingDeleteId= action.payload;
    },
    
   }
})

export const {
  setSearchTerm,
  clearSearchTerm,
  openDeleteAllModal,
  closeDeleteAllModal,
  setEntryPendingDelete,
} = formSlice.actions;

export default formSlice.reducer;