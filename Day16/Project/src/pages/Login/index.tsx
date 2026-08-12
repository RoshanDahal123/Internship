import { useForm } from "react-hook-form";
import { useNavigate } from "react-router";
import { submitForm } from "../../store/action";
import { useAppDispatch } from "../../store/hooks";

interface FormData {
  name:string,
  email:string,
  age:number,
  address:string,
  description:string

}



const Login = () => {
const dispatch= useAppDispatch();
const navigate= useNavigate();
   
   const {register,formState:{errors},
  handleSubmit}= useForm<FormData>({
    defaultValues:{
      name:"",
      email:"",
      age:1,
      address:"",
      description:""
    }
  });
 
   
   

    const onSubmit=(data:FormData)=>{
    dispatch(submitForm(data));
    navigate("/display");
    }

  return (
   <div className="max-w-md mx-auto p-6 bg-white rounded-xl shadow-md border border-gray-100">
      <h2 className="text-2xl font-bold text-gray-800 mb-6 text-center">
        User Information Form
      </h2>

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        {/* Name Input */}
        <div>
          <label htmlFor="Name" className="block text-sm font-medium text-gray-700 mb-1">
            Full Name
          </label>
          <input
            type="text"
            id="Name"
            {...register("name",{required:true})}
             aria-invalid={errors.name ? "true" : "false"}
            placeholder="John Doe"
            className="w-full px-3 py-2 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-gray-800 placeholder-gray-400"
          />
          {errors.name?.type === "required" && (
        <p role="alert" className="text-red-500">Name is required</p>
      )}
        </div>

        {/* Email Input */}
        <div>
          <label htmlFor="Email" className="block text-sm font-medium text-gray-700 mb-1">
            Email Address
          </label>
          <input
            type="email"
          {...register("email",{required:"Email Address is Required"})}
          aria-invalid={errors.email ? "true" : "false"}
            placeholder="john@example.com"
            className="w-full px-3 py-2 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-gray-800 placeholder-gray-400"
          />
        </div>
         {errors.email&& <p role="alert">
          {errors.email && <p role="alert">
            {errors.email.message}</p>}</p>}
        {/* Age Input */}
        <div>
          <label htmlFor="Age" className="block text-sm font-medium text-gray-700 mb-1">
            Age
          </label>
          <input
            type="number"
          {...register("age",{required:true,min:1,max:100})}
            
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
   {...register("address",{required:true})}
            
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
          {...register("description",{required:true})}
          rows={4}
            placeholder="Tell us a bit about yourself..."
            className="w-full px-3 py-2 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-gray-800 placeholder-gray-400 resize-y"
            area-invalude={errors.description?"true":"false"}
          />
           {errors.description && <p role="alert">{errors.description.message}</p>}

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