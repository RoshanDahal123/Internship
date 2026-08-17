import type { LucideIcon } from "lucide-react";

import type {
    FieldError,
    FieldValues,
    Path,
    RegisterOptions,
    UseFormRegister,
} from "react-hook-form";

interface FormInputProps<T extends FieldValues> {
  name: Path<T>;
  register: UseFormRegister<T>;
  rules?: RegisterOptions<T>;
  error?: FieldError;
  icon?: LucideIcon;
  label?: string; //omit for education rows, which have no visible label
  type?: string; //"text"|"Email;"|"number"
  placeholder?: string;
}

function FormInput<T extends FieldValues>({
  name,
  register,
  rules,
  error,
  icon: Icon,
  label,
  type = "text",
  placeholder,
}: FormInputProps<T>) {
  const inputBase =
    "w-full pl-10 pr-3 py-2.5 border rounded-lg shadow-sm text-gray-800 placeholder-gray-400 transition-colors duration-150 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500";
  const inputValid = "border-gray-300";
  const inputError = "border-red-400 bg-red-50/40";

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
        <input
          type={type}
          {...register(name, rules)}
          aria-invalid={error ? "true" : "false"}
          placeholder={placeholder}
          className={`${inputBase} ${Icon ? "" : "pl-3"} ${error ? inputError : inputValid}`}
        />
        {error && (
          <p role="alert" className="text-red-500 text-xs mt-1.5">
            {error.message}
          </p>
        )}
      </div>
    </div>
  );
}

export default FormInput;