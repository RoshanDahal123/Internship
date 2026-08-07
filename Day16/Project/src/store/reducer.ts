import type { FormEntry } from "../types/formTypes";
import type { FormActionTypes } from "./action";
import { CLEAR_FORM, DELETE_ENTRY } from "./actionTypes";

export interface FormState{
  entries:FormEntry[]
}

const initialState: FormState={
entries:[]
}

export const formReducer=(
    state:FormState=initialState,
    action:FormActionTypes
):FormState=>{
    switch(action.type){
        case "SUBMIT_FORM":
        const newEntry:FormEntry={...action.payload, id:crypto.randomUUID()}
        return {...state,entries:[...state.entries,newEntry]};
        
        case CLEAR_FORM:
            return{...state, entries:[]};
        case DELETE_ENTRY:
            return{...state,
                entries:state.entries.filter((entry)=>entry.id!==action.payload),
    }

         default:
            return state;
    }

}

