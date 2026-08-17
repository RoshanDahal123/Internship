import type { LucideIcon } from "lucide-react";

import type {
  FieldError,
  FieldValues,
  Path,
  RegisterOptions,
  UseFormRegister,
} from "react-hook-form";

interface FormTextareaProps<T extends FieldValues> {
  name: Path<T>;
  register: UseFormRegister<T>;
  rules?: RegisterOptions<T>;
  error?: FieldError;
  icon?: LucideIcon;
  label?: string; //omit for education rows, which have no visible label
  type?: string; //"text"|"Email;"|"number"
  placeholder?: string;
  rows?: number;
}

function FormTextArea<T extends FieldValues>({
  name,
  register,
  rules,
  error,
  icon: Icon,
  label,
  placeholder,
  rows,
}: FormTextareaProps<T>) {
  return (
    <div>
      {label && (
        <label className="block text-sm font-semibold text-gray-700 mb-1.5">
          {label}
        </label>
      )}
      <div className="relative">
        {Icon && (
          <Icon className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
        )}
        <textarea {...register(name, rules)} rows={rows}>
          placeholder={placeholder}
          aria-invalid={error ? "true" : "false"}
          className=
          {`w-full pl-10 pr-3 py-2.5 border rounded-lg shadow-sm text-gray-800 placeholder-gray-400 transition-colors duration-150 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 resize-y ${
            error ? "border-red-400 bg-red-50/40" : "border-gray-300"
          }`}
        </textarea>
      </div>
      {error && (
        <p role="alert" className="text-red-500 text-xs mt-1.5">
          {error.message}
        </p>
      )}
    </div>
  );
}

export default FormTextArea;