import {
  BadgeCheck,
  Calendar,
  FileText,
  GraduationCap,
  Mail,
  MapPin,
  Plus,
  School,
  Send,
  Trash2,
  User,
} from "lucide-react";
import { useState } from "react";
import { useFieldArray, useForm } from "react-hook-form";
import { useNavigate } from "react-router";
import { createFormEntry } from "../../api/formApi";
import { useAppDispatch } from "../../store/hooks";
import type { FormData } from "../../types/formTypes";
import { addEntryLocally } from "../../store/action";

const Login = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
 const [submitError, setSubmitError]=useState("");
 const[isSubmitting, setIsSubmitting]=useState(false);
  const {
    register,
    control,
    formState: { errors },
    handleSubmit
  } = useForm<FormData>({
    defaultValues: {
      name: "",
      email: "",
      age: 1,
      address: "",
      description: "",
      education: [],
    },
  });

  const { fields, append, remove } = useFieldArray({
    control,
    name: "education",
  });

  const onSubmit =async (data: FormData) => {
    setSubmitError("");
   setIsSubmitting(true);
    try{
      const savedEntry=await createFormEntry(data);
      dispatch(addEntryLocally(savedEntry));
      navigate('/display');
    }
    catch(err){
 setSubmitError("Failed to submit form. Please try again.");
      console.error(err);
    }finally{
      setIsSubmitting(false);
    }
  };

  // Shared classes so every input/textarea stays visually consistent —
  // one source of truth instead of repeating the string 8+ times
  const inputBase =
    "w-full pl-10 pr-3 py-2.5 border rounded-lg shadow-sm text-gray-800 placeholder-gray-400 transition-colors duration-150 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500";
  const inputValid = "border-gray-300";
  const inputError = "border-red-400 bg-red-50/40";

  return (
    <div className="min-h-screen bg-linear-to-br from-slate-50 to-blue-50 py-10 px-4">
      <div className="max-w-2xl mx-auto bg-white rounded-2xl shadow-xl border border-gray-100 overflow-hidden">
        {/* Header banner — gives the form a "product" feel instead of a bare card */}
        <div className="bg-linear-to-r from-blue-600 to-indigo-600 px-8 py-7">
          <h2 className="text-2xl font-bold text-white text-center">
            User Information Form
          </h2>
          <p className="text-blue-100 text-sm text-center mt-1">
            Fill in your details below to get started
          </p>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="p-8 space-y-5">
          {/* Name */}
          <div>
            <label
              htmlFor="Name"
              className="block text-sm font-semibold text-gray-700 mb-1.5"
            >
              Full Name
            </label>
            <div className="relative">
              <User className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
              <input
                type="text"
                id="Name"
                {...register("name", { required: "Name is required" })}
                aria-invalid={errors.name ? "true" : "false"}
                placeholder="John Doe"
                className={`${inputBase} ${errors.name ? inputError : inputValid}`}
              />
            </div>
            {errors.name && (
              <p
                role="alert"
                className="text-red-500 text-xs mt-1.5 flex items-center gap-1"
              >
                {errors.name.message}
              </p>
            )}
          </div>

          {/* Email */}
          <div>
            <label
              htmlFor="Email"
              className="block text-sm font-semibold text-gray-700 mb-1.5"
            >
              Email Address
            </label>
            <div className="relative">
              <Mail className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
              <input
                type="email"
                id="Email"
                {...register("email", {
                  required: "Email Address is required",
                })}
                aria-invalid={errors.email ? "true" : "false"}
                placeholder="john@example.com"
                className={`${inputBase} ${errors.email ? inputError : inputValid}`}
              />
            </div>
            {errors.email && (
              <p role="alert" className="text-red-500 text-xs mt-1.5">
                {errors.email.message}
              </p>
            )}
          </div>

          {/* Age + Address share a row on larger screens — saves vertical space */}
          <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <div className="sm:col-span-1">
              <label
                htmlFor="Age"
                className="block text-sm font-semibold text-gray-700 mb-1.5"
              >
                Age
              </label>
              <div className="relative">
                <Calendar className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
                <input
                  type="number"
                  id="Age"
                  {...register("age", {
                    required: "Required",
                    valueAsNumber: true,
                    min: { value: 1, message: "Min 1" },
                    max: { value: 100, message: "Max 100" },
                  })}
                  aria-invalid={errors.age ? "true" : "false"}
                  className={`${inputBase} ${errors.age ? inputError : inputValid}`}
                />
              </div>
              {errors.age && (
                <p role="alert" className="text-red-500 text-xs mt-1.5">
                  {errors.age.message}
                </p>
              )}
            </div>

            <div className="sm:col-span-2">
              <label
                htmlFor="Address"
                className="block text-sm font-semibold text-gray-700 mb-1.5"
              >
                Address
              </label>
              <div className="relative">
                <MapPin className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
                <input
                  type="text"
                  id="Address"
                  {...register("address", { required: "Address is required" })}
                  aria-invalid={errors.address ? "true" : "false"}
                  placeholder="123 Main St, City, Country"
                  className={`${inputBase} ${errors.address ? inputError : inputValid}`}
                />
              </div>
              {errors.address && (
                <p role="alert" className="text-red-500 text-xs mt-1.5">
                  {errors.address.message}
                </p>
              )}
            </div>
          </div>

          {/* Description */}
          <div>
            <label
              htmlFor="Description"
              className="block text-sm font-semibold text-gray-700 mb-1.5"
            >
              Description
            </label>
            <div className="relative">
              <FileText className="absolute left-3 top-3 w-4 h-4 text-gray-400" />
              <textarea
                id="Description"
                {...register("description", {
                  required: "Description is required",
                })}
                rows={4}
                placeholder="Tell us a bit about yourself..."
                aria-invalid={errors.description ? "true" : "false"}
                className={`${inputBase} ${errors.description ? inputError : inputValid} resize-y`}
              />
            </div>
            {errors.description && (
              <p role="alert" className="text-red-500 text-xs mt-1.5">
                {errors.description.message}
              </p>
            )}
          </div>

          {/* Education — dynamic, inline, icon-led rows */}
          <div className="pt-2">
            <div className="flex justify-between items-center mb-3">
              <div className="flex items-center gap-2">
                <GraduationCap className="w-5 h-5 text-blue-600" />
                <label className="text-sm font-semibold text-gray-700">
                  Education
                </label>
              </div>
              <button
                type="button"
                onClick={() =>
                  append({
                    degree: "",
                    institutionName: "",
                    year: new Date().getFullYear(),
                  })
                }
                className="flex items-center gap-1 text-sm font-medium text-blue-600 hover:text-white hover:bg-blue-600 border border-blue-200 hover:border-blue-600 px-3 py-1.5 rounded-lg transition-colors duration-150"
              >
                <Plus className="w-4 h-4" />
                Add Education
              </button>
            </div>

            {fields.length === 0 && (
              <p className="text-sm text-gray-400 italic border border-dashed border-gray-200 rounded-lg py-4 text-center">
                No education added yet — click "Add Education" to include your
                qualifications.
              </p>
            )}

            <div className="space-y-3">
              {fields.map((field, index) => (
                <div
                  key={field.id}
                  className="p-3.5 bg-slate-50 border border-gray-200 rounded-xl hover:border-blue-200 transition-colors duration-150"
                >
                  <div className="flex flex-col sm:flex-row sm:items-start gap-2.5">
                    <div className="flex-1">
                      <div className="relative">
                        <BadgeCheck className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
                        <input
                          type="text"
                          placeholder="Degree"
                          {...register(`education.${index}.degree`, {
                            required: "Required",
                          })}
                          aria-invalid={
                            errors.education?.[index]?.degree ? "true" : "false"
                          }
                          className={`w-full pl-10 pr-3 py-2 bg-white border rounded-lg shadow-sm text-sm text-gray-800 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 transition-colors ${
                            errors.education?.[index]?.degree
                              ? "border-red-400"
                              : "border-gray-300"
                          }`}
                        />
                      </div>
                      {errors.education?.[index]?.degree && (
                        <p role="alert" className="text-red-500 text-xs mt-1">
                          {errors.education[index]?.degree?.message}
                        </p>
                      )}
                    </div>

                    <div className="flex-1">
                      <div className="relative">
                        <School className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
                        <input
                          type="text"
                          placeholder="Institution Name"
                          {...register(
                            `education.${index}.institutionName`,
                            {
                              required: "Required",
                            },
                          )}
                          aria-invalid={
                            errors.education?.[index]?.institutionName
                              ? "true"
                              : "false"
                          }
                          className={`w-full pl-10 pr-3 py-2 bg-white border rounded-lg shadow-sm text-sm text-gray-800 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 transition-colors ${
                            errors.education?.[index]?.institutionName
                              ? "border-red-400"
                              : "border-gray-300"
                          }`}
                        />
                      </div>
                      {errors.education?.[index]?.institutionName && (
                        <p role="alert" className="text-red-500 text-xs mt-1">
                          {errors.education[index]?.institutionName?.message}
                        </p>
                      )}
                    </div>

                    <div className="sm:w-28 flex-none">
                      <div className="relative">
                        <Calendar className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
                        <input
                          type="number"
                          placeholder="Year"
                          {...register(`education.${index}.year`, {
                            required: "Required",
                            valueAsNumber: true,
                            min: { value: 1950, message: "Invalid" },
                            max: {
                              value: new Date().getFullYear(),
                              message: "Future",
                            },
                          })}
                          aria-invalid={
                            errors.education?.[index]?.year ? "true" : "false"
                          }
                          className={`w-full pl-10 pr-3 py-2 bg-white border rounded-lg shadow-sm text-sm text-gray-800 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 transition-colors ${
                            errors.education?.[index]?.year
                              ? "border-red-400"
                              : "border-gray-300"
                          }`}
                        />
                      </div>
                      {errors.education?.[index]?.year && (
                        <p role="alert" className="text-red-500 text-xs mt-1">
                          {errors.education[index]?.year?.message}
                        </p>
                      )}
                    </div>

                   
                    <button
                      type="button"
                      onClick={() => remove(index)}
                      aria-label="Remove education entry"
                      className="flex-none self-start sm:mt-0.5 w-9 h-9 flex items-center justify-center rounded-full text-gray-400 hover:text-red-600 hover:bg-red-50 transition-colors duration-150"
                    >
                      <Trash2 className="w-4 h-4" />
                    </button>
                  </div>
                </div>
              ))}
            </div>
          </div>
    <p role="alert" className="text-red-500 text-sm text-center">{submitError}</p>
          {/* Submit */}
          <button
            type="submit"
            className="w-full mt-4 flex items-center justify-center gap-2 py-3 px-4 bg-blue-600 hover:bg-blue-700 active:bg-blue-800 text-white font-semibold rounded-lg shadow-md hover:shadow-lg transition-all duration-150 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
            disabled={isSubmitting}
          >
            <Send className="w-4 h-4" />
           {isSubmitting ? "Submitting":"Submit"}
          </button>
          
        </form>
      </div>
    </div>
  );
};

export default Login;
