export interface FormData {
  name: string;
  email: string;
  address: string;
  age: number;
  description: string;

}
export interface FormEntry extends FormData{
id:string;
}