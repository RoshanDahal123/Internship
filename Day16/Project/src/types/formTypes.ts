export interface Education{
  degree:string;
  institutionName:string;
  year:number;
}

export interface FormValues {
  name: string;
  email: string;
  address: string;
  age: number;
  description: string;
  dateOfBirth: Date | null;
  cvFile: File | null;
  education:Education[];

}
export interface FormEntry {
  id: number;
  name: string;
  email: string;
  age: number;
  address: string;
  description: string;
  dateOfBirth: string; // ISO string, as the backend returns it
  cvFileUrl: string | null;
  education: Education[];
}