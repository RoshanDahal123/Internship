//DUCKS pattern

import { createSlice, PayloadAction } from "@reduxjs/toolkit";

interface CounterState{
    value:number;
}

const initialState:CounterState={
    value:0
};

const counterSlice = createSlice({

    name:'counter',
    initialState,
    reducers:{
        //increment
        incremented(state){
            state.value++;
        },
        //it uses a library called immer and it wraps our state updates and tracks all the mutations that we try to do. it makes immutable under the hood
  amountAdded(state, action:PayloadAction<number>){
    state.value+= action.payload;
  }
        
        
    }
});
export const{incremented,amountAdded}= counterSlice.actions;

export default counterSlice.reducer;


