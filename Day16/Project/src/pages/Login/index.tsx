import { useState } from "react";
import { useNavigate } from "react-router";
import { submitForm } from "../../store/action";
import { useAppDispatch } from "../../store/hooks";
import type { FormData } from "../../types/formTypes";





const Login = () => {
const dispatch= useAppDispatch();
const navigate= useNavigate();
    const [formData, setFormData]=useState<FormData>({
        name:"",
        email:"",
        address:"",
        age:1,
        description:""
    })
     const [error, setError] = useState<string>("");
 
   
    const handleChange=(e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>)=>{

        const{name,value, type}= e.target;
       setFormData((prev) => ({
      ...prev,
      [name]: type === 'number' ? (value === '' ? '' : Number(value)) : value,
    }));
    }

    const handleSubmit=(e:React.FormEvent<HTMLFormElement>)=>{
        e.preventDefault();

    if(!formData.name.trim()||!formData.email.trim()){
        setError("Name and Email are required");
        return;
    }

    setError("");
    dispatch(submitForm(formData));
     setFormData({ name: "", email: "", address: "", age: 1, description: "" });
    navigate("/display");

    }

  return (
   <div className="max-w-md mx-auto p-6 bg-white rounded-xl shadow-md border border-gray-100">
      <h2 className="text-2xl font-bold text-gray-800 mb-6 text-center">
        User Information Form
      </h2>

      <form onSubmit={handleSubmit} className="space-y-4">
        {/* Name Input */}
        <div>
          <label htmlFor="Name" className="block text-sm font-medium text-gray-700 mb-1">
            Full Name
          </label>
          <input
            type="text"
            id="Name"
            name="name"
            value={formData.name}
            onChange={handleChange}
            required
            placeholder="John Doe"
            className="w-full px-3 py-2 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-gray-800 placeholder-gray-400"
          />
        </div>

        {/* Email Input */}
        <div>
          <label htmlFor="Email" className="block text-sm font-medium text-gray-700 mb-1">
            Email Address
          </label>
          <input
            type="email"
            id="Email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
            placeholder="john@example.com"
            className="w-full px-3 py-2 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-gray-800 placeholder-gray-400"
          />
        </div>

        {/* Age Input */}
        <div>
          <label htmlFor="Age" className="block text-sm font-medium text-gray-700 mb-1">
            Age
          </label>
          <input
            type="number"
            id="Age"
            name="age"
            value={formData.age}
            onChange={handleChange}
            min={1}
            max={120}
            required
            className="w-full px-3 py-2 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-gray-800"
          />
        </div>

        {/* Address Input */}
        <div>
          <label htmlFor="Address" className="block text-sm font-medium text-gray-700 mb-1">
            Address
          </label>
          <input
            type="text"
            id="Address"
            name="address"
            value={formData.address}
            onChange={handleChange}
            required
            placeholder="123 Main St, City, Country"
            className="w-full px-3 py-2 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-gray-800 placeholder-gray-400"
          />
        </div>

        {/* Description Textarea */}
        <div>
          <label htmlFor="Description" className="block text-sm font-medium text-gray-700 mb-1">
            Description
          </label>
          <textarea
            id="Description"
            name="description"
            value={formData.description}
            onChange={handleChange}
            rows={4}
            placeholder="Tell us a bit about yourself..."
            className="w-full px-3 py-2 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-gray-800 placeholder-gray-400 resize-y"
          />
        </div>

        {/* Submit Button */}
        <button
          type="submit"
          className="w-full mt-2 py-2.5 px-4 bg-blue-600 hover:bg-blue-700 text-white font-medium rounded-lg shadow transition duration-150 ease-in-out focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
        >
          Submit
        </button>
      </form>
    </div>
  )
}

export default Login;