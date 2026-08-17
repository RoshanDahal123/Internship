import { Calendar } from "lucide-react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { useController, type Control, type FieldValues, type Path } from "react-hook-form";

interface DatePickerFieldProps<T extends FieldValues> {
  name: Path<T>;
  control: Control<T>;
  label: string;
  error?: string;
}

function DatePickerField<T extends FieldValues>({ name, control, label, error }: DatePickerFieldProps<T>) {
  const { field } = useController({
    name,
    control,
    rules: { required: "Date of birth is required" },
  });

  return (
    <div>
      <label className="block text-sm font-semibold text-gray-700 mb-1.5">{label}</label>
      <div className="relative">
        <Calendar className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400 z-10" />
        <DatePicker
          selected={field.value as Date | null}
          onChange={(date:any) => field.onChange(date)}
          onBlur={field.onBlur}
          maxDate={new Date()} // can't be born in the future
          placeholderText="Select date of birth"
          dateFormat="yyyy-MM-dd"
          className={`w-full pl-10 pr-3 py-2.5 border rounded-lg shadow-sm text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 ${
            error ? "border-red-400" : "border-gray-300"
          }`}
        />
      </div>
      {error && <p role="alert" className="text-red-500 text-xs mt-1.5">{error}</p>}
    </div>
  );
}

export default DatePickerField;