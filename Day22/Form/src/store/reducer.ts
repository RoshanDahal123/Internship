import type { FormEntry } from "../types/formTypes";
import type { FormActionTypes } from "./action";
import { ADD_ENTRY_LOCALLY, FETCH_FAILURE, FETCH_SUCCESS, REMOVE_ALL_ENTRIES, REMOVE_ENTRY } from "./actionTypes";

export interface FormState{
  entries:FormEntry[];
  loading:boolean;
  error:string|null;
}

const initialState: FormState={
entries:[],
loading:false,
error:null
}

export const formReducer=(
    state:FormState=initialState,
    action:FormActionTypes
):FormState=>{
    switch(action.type){
        case "FETCH_START":
        return {...state,loading:true, error:null};
        
        case FETCH_SUCCESS:
            return{...state,loading:false,entries:action.payload};
        case FETCH_FAILURE:
            return{...state,error:action.payload,loading:false
            }
        
        case ADD_ENTRY_LOCALLY:
            return{...state,entries:[...state.entries,action.payload]}
    
        case REMOVE_ENTRY:
            
            return{...state,entries:
                state.entries.filter(
                    (entry)=>(entry.id   !== action.payload)
                )
            }
        case REMOVE_ALL_ENTRIES:
            return{...state,entries:[]}
         default:
            return state;
    }

}

