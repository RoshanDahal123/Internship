import api from './axiosInstance';

import { FormData, FormEntry } from "../types/formTypes";


//POST
export const createFormEntry=async(data:FormData):Promise<FormEntry>=>{
const response = await api.post<FormEntry>("/formentries",data);
return response.data;
}

export const getFormEntries= async():Promise<FormEntry[]>=>{
    const response = await api.get<FormEntry[]>("/formentries");
    return response.data;
}
//GET - fetch all submitted entries

