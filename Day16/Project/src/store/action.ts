
import type { FormEntry } from '../types/formTypes';
import { ADD_ENTRY_LOCALLY, FETCH_FAILURE, FETCH_START, FETCH_SUCCESS, REMOVE_ALL_ENTRIES, REMOVE_ENTRY } from './actionTypes';

interface FetchStartAction {
  type: typeof FETCH_START;
}
interface FetchSuccessAction {
  type: typeof FETCH_SUCCESS;
  payload: FormEntry[];
}
interface FetchFailureAction {
  type: typeof FETCH_FAILURE;
  payload: string;
}
interface AddEntryLocallyAction {
  type: typeof ADD_ENTRY_LOCALLY;
  payload: FormEntry;
}
interface RemoveAllEntriesAction{
    type: typeof REMOVE_ALL_ENTRIES
}
interface RemoveEntryAction{
    type: typeof REMOVE_ENTRY;
    payload:number;
}


export type FormActionTypes =
  | FetchStartAction
  | FetchSuccessAction
  | FetchFailureAction
  | AddEntryLocallyAction
  |RemoveEntryAction
  |RemoveAllEntriesAction


export const fetchStart=():FetchStartAction=>({
    type:FETCH_START
})

export const fetchSuccess = (entries: FormEntry[]): FetchSuccessAction => ({
  type: FETCH_SUCCESS,
  payload: entries,
});

export const fetchFailure = (message: string): FetchFailureAction => ({
  type: FETCH_FAILURE,
  payload: message,
});


export const addEntryLocally=(entry:FormEntry):AddEntryLocallyAction=>(
{
type:ADD_ENTRY_LOCALLY,
payload:entry,
});

export const removeEntry=(id:number):RemoveEntryAction=>({
    type:REMOVE_ENTRY,
    payload: id,
})

export const removeAllEntries=():RemoveAllEntriesAction=>({
type:REMOVE_ALL_ENTRIES
})

