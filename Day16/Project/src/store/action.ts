
import type { FormData } from '../types/formTypes';
import {SUBMIT_FORM,CLEAR_FORM,DELETE_ENTRY} from './actionTypes';

interface SubmitFormAction{
    type:typeof SUBMIT_FORM,
    payload:FormData
}

interface ClearFormAction{
    type:typeof CLEAR_FORM,
}
interface DeleteEntryAction{
    type:typeof DELETE_ENTRY;
    payload:string
}


export type FormActionTypes= SubmitFormAction| ClearFormAction|DeleteEntryAction;

export const submitForm=(data:FormData):SubmitFormAction=>({
    type:SUBMIT_FORM,
    payload:data,
})

export const clearForm =():ClearFormAction=>({
    type:CLEAR_FORM
})

export const deleteEntry=(id:string):DeleteEntryAction=>({
    type: DELETE_ENTRY,
    payload:id
})

