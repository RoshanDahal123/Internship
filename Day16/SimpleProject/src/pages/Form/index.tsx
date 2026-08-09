

import React, { type SetStateAction } from 'react';
import { useNavigate } from 'react-router';

type FormProps={
   formData:{
      name:string;
   },
   setFormData:React.Dispatch<SetStateAction<{name:string}>>
}

function Form({formData,setFormData}:FormProps) {
//  const [formData, setFormData] = useState({
//     name:" "
//   })
  const navigate=useNavigate();
    
    const handleSubmit =(e:React.SubmitEvent<HTMLFormElement>)=>{
       e.preventDefault();
       navigate("/display");
    }
    const handleChange=(e:React.ChangeEvent<HTMLInputElement>)=>{
       const {name,value}=e.target;
       setFormData((prev)=>({
        ...prev,
        [name]:value
       }))
    }
return <>

<form onSubmit={handleSubmit}>
        <label htmlFor="name">Name</label>
        <input type="text"
        name="name"
        value={formData.name} 
        onChange={handleChange}/>
        <button type='submit'>submit</button>
       </form>
</>
 
   
       

  
}

export default Form;