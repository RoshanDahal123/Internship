import { BadgeCheck, Calendar, School, Trash2 } from "lucide-react";
import type {
    FieldErrors,
    UseFormRegister,
} from "react-hook-form";
import type { FormValues } from "../types/formTypes";
import FormInput from "./FormInput";

interface EducationRowProps {
  index: number;
  register: UseFormRegister<FormValues>;
  errors: FieldErrors<FormValues>;
  onRemove: () => void;
}

const EducationRow = ({ index, register, errors, onRemove }: EducationRowProps) => {
  return (
    <div className="p-3.5 bg-slate-50 border border-gray-200 rounded-xl hover:border-blue-200 transition-colors duration-150">
      <div className="flex flex-col sm:flex-row sm:items-start gap-2.5">
        <div className="flex-1">
          <FormInput<FormValues>
            name={`education.${index}.degree`}
            register={register}
            rules={{ required: "Required" }}
            error={errors.education?.[index]?.degree}
            icon={BadgeCheck}
            placeholder="Degree"
          />
        </div>

        <div className="flex-1">
          <FormInput<FormValues>
            name={`education.${index}.institutionName`}
            register={register}
            rules={{ required: "Required" }}
            error={errors.education?.[index]?.institutionName}
            icon={School}
            placeholder="Institution Name"
          />
        </div>

        <div className="sm:w-28 flex-none">
          <FormInput<FormValues>
            name={`education.${index}.year`}
            register={register}
            rules={{
              required: "Required",
              valueAsNumber: true,
              min: { value: 1950, message: "Invalid" },
              max: { value: new Date().getFullYear(), message: "Future" },
            }}
            error={errors.education?.[index]?.year}
            icon={Calendar}
            type="number"
            placeholder="Year"
          />
        </div>

        <button
          type="button"
          onClick={onRemove}
          aria-label="Remove education entry"
          className="flex-none self-start sm:mt-0.5 w-9 h-9 flex items-center justify-center rounded-full text-gray-400 hover:text-red-600 hover:bg-red-50 transition-colors duration-150"
        >
          <Trash2 className="w-4 h-4" />
        </button>
      </div>
    </div>
  );
};

export default EducationRow;