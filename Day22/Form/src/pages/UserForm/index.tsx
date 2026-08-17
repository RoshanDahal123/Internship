import {
  Calendar,
  FileText,
  GraduationCap,
  Mail,
  MapPin,
  Plus,
  Send,
  User
} from "lucide-react";
import { useState } from "react";
import { useFieldArray, useForm } from "react-hook-form";
import { useNavigate } from "react-router";
import CvUploadField from "../../components/CvUploadField";
import DatePickerField from "../../components/DatePickerField";
import EducationRow from "../../components/Education";
import FormInput from "../../components/FormInput";
import FormTextArea from "../../components/FormTextArea";
import { useCreateFormEntryMutation } from "../../features/formApiSlice";
import type { FormValues } from "../../types/formTypes";

const UserForm = () => {
  // const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const [submitError, setSubmitError] = useState("");
  
  const [createFormEntry, { isLoading: isSubmitting }] =
    useCreateFormEntryMutation();

  const {
    register,
    control,
    formState: { errors },
    handleSubmit,
  } = useForm<FormValues>({
    defaultValues: {
      name: "",
      email: "",
      age: 1,
      address: "",
      description: "",
      dateOfBirth: null,
      cvFile: null,
      education: [],
    },
  });

  const { fields, append, remove } = useFieldArray({
    control,
    name: "education",
  });

  const onSubmit = async (data: FormValues) => {
    setSubmitError("");

    try {
      // .unwrap() converts RTK Query's { data } | { error } result shape
      // into a plain Promise that resolves on success or throws on failure —
      // lets you keep using try/catch the same way you already do
      await createFormEntry(data).unwrap();

      navigate("/display");
    } catch (err) {
      setSubmitError("Failed to submit form. Please try again.");
    }
  };

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

          <FormInput<FormValues>
            name="name"
            register={register}
            rules={{ required: "Name is required" }}
            error={errors.name}
            icon={User}
            label="Full Name"
            placeholder="John Doe"
          />

          {/* Email */}
          <FormInput<FormValues>
            name="email"
            register={register}
            rules={{ required: "Email is required" }}
            error={errors.email}
            icon={Mail}
            label="Email Address"
            type="email"
            placeholder="john@example.com"
          />

          {/* Age + Adddress share a row on larger screens — saves vertical space */}

          <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <div className="sm:col-span-1">
              <FormInput<FormValues>
                name="age"
                register={register}
                rules={{
                  required: "Required",
                  valueAsNumber: true,
                  min: { value: 1, message: "Min 1" },
                  max: { value: 100, message: "Max 100" },
                }}
                error={errors.age}
                icon={Calendar}
                label="Age"
                type="number"
              />

              <div className="sm:col-span-2">
                <FormInput<FormValues>
                  name="address"
                  register={register}
                  rules={{ required: "Address is required" }}
                  error={errors.address}
                  icon={MapPin}
                  label="Address"
                  placeholder="123 Main St, City, Country"
                />
              </div>
            </div>
          </div>

          <DatePickerField
            name="dateOfBirth"
            control={control}
            label="Date of Birth"
            error={errors.dateOfBirth?.message}
          />

          <CvUploadField name="cvFile" control={control} label="CV / Resume" />

          {/* Description */}

          <FormTextArea<FormValues>
            label="Description"
            name="description"
            register={register}
            rules={{ required: "Description is required" }}
            icon={FileText}
            placeholder="Tell us a bit about yourself"
            rows={4}
            error={errors.description}
          />

 {/* Education */}
          <div className="pt-2">
            <div className="flex justify-between items-center mb-3">
              <div className="flex items-center gap-2">
                <GraduationCap className="w-5 h-5 text-blue-600" />
                <label className="text-sm font-semibold text-gray-700">Education</label>
              </div>
              <button
                type="button"
                onClick={() => append({ degree: "", institutionName: "", year: new Date().getFullYear() })}
                className="flex items-center gap-1 text-sm font-medium text-blue-600 hover:text-white hover:bg-blue-600 border border-blue-200 hover:border-blue-600 px-3 py-1.5 rounded-lg transition-colors duration-150"
              >
                <Plus className="w-4 h-4" /> Add Education
              </button>
            </div>

            {fields.length === 0 && (
              <p className="text-sm text-gray-400 italic border border-dashed border-gray-200 rounded-lg py-4 text-center">
                No education added yet — click "Add Education" to include your qualifications.
              </p>
            )}

            <div className="space-y-3">
              {fields.map((field, index) => (
                <EducationRow
                  key={field.id}
                  index={index}
                  register={register}
                  errors={errors}
                  onRemove={() => remove(index)}
                />
              ))}
            </div>
          </div>

          {submitError && (
            <p role="alert" className="text-red-500 text-sm text-center">{submitError}</p>
          )}

          <button
            type="submit"
            disabled={isSubmitting}
            className="w-full mt-4 flex items-center justify-center gap-2 py-3 px-4 bg-blue-600 hover:bg-blue-700 active:bg-blue-800 text-white font-semibold rounded-lg shadow-md hover:shadow-lg transition-all duration-150 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-60"
          >
            <Send className="w-4 h-4" />
            {isSubmitting ? "Submitting..." : "Submit"}
          </button>
        </form>
      </div>
    </div>
  );
};

export default UserForm;
         



  