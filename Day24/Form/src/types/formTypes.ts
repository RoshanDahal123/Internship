export interface Education{
  degree:string;
  institutionName:string;
  year:number;
}

export interface Student {
  name: string;
  email: string;
  address: string;
  age: number;
  description: string;
  dateOfBirth: Date | null;
  cvFile: File | null;
  education:Education[];

}
export interface StudentEntry{
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

export type UpdateStudentPayload = Student & {
  id: number;
};

export interface PaginatedStudents{
  items:StudentEntry[],
  page:number;
  pageSize:number;
  totalCount:number;
  hasNextPage:boolean;
}