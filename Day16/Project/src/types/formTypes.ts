export interface Education{
  degree:string;
  institutionName:string;
  year:number;
}

export interface FormData {
  name: string;
  email: string;
  address: string;
  age: number;
  description: string;
  education:Education[];

}
export interface FormEntry extends FormData{
id:string;
}