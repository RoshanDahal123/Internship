import api from './axiosInstance';

import { FormValues, FormEntry } from "../types/formTypes";


//POST
export const createFormEntry=async(data:FormValues):Promise<FormEntry>=>{
const multipart = new FormData();
 multipart.append("Name", data.name);
  multipart.append("Email", data.email);
  multipart.append("Age", String(data.age));
  multipart.append("Address", data.address);
  multipart.append("Description", data.description);
  multipart.append(
    "DateOfBirth",
    data.dateOfBirth ? data.dateOfBirth.toISOString() : ""
  );
  multipart.append("EducationJson",JSON.stringify(data.education));
  if(data.cvFile){
    multipart.append("CvFile",data.cvFile);
  }
const response = await api.post<FormEntry>("/formentries",multipart,{
    headers:{"Content-Type":"multipart/form-data"},
});
return response.data;
}

//GET - fetch all submitted entries

export const getFormEntries= async():Promise<FormEntry[]>=>{
    const response = await api.get<FormEntry[]>("/formentries");
    return response.data;
}

export const deleteAllEntries= async():Promise<FormEntry>=>{
    const response= await api.delete("/formentries/all");
    return response.data;
}

export const deleteEntry= async(id:number)=>{
    const response = await api.delete(`/formentries/${id}`)
    return response.data;
}


