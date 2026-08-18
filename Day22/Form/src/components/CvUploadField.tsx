import { FileText, Upload, X } from "lucide-react";
import { useRef } from "react";
import { useController, type Control, type FieldValues, type Path } from "react-hook-form";


interface CvUploadFieldProps<T extends FieldValues> {
  name: Path<T>;
  control: Control<T>;
  label: string;
}


function CvUploadField<T extends FieldValues>({ name, control, label }: CvUploadFieldProps<T>) {
     const inputRef = useRef<HTMLInputElement>(null);

     const { field, fieldState: { error } } = useController({
    name,
    control,
    rules: {
      // custom validate function — checks the File object's own properties,
      // since "required"/"min"/"max" don't mean anything for a File
      validate: (file: File | null) => {
        if (!file) return true; // CV is optional — remove this line to make it required
        const allowed = ["application/pdf", "application/msword",
          "application/vnd.openxmlformats-officedocument.wordprocessingml.document"];
        if (!allowed.includes(file.type)) return "Only PDF or Word documents are allowed";
        if (file.size > 5 * 1024 * 1024) return "File must be under 5MB";
        return true;
      },
    },
  });

   const selectedFile = field.value as File | null;

   return (
    <div>
            <label className="block text-sm font-semibold text-gray-700 mb-1.5">{label}</label>
            {!selectedFile ? (
        <button
          type="button"
          onClick={() => inputRef.current?.click()}
          className={`w-full  py-4 flex flex-col items-center justify-center gap-2 border-2 border-dashed rounded-lg text-sm text-gray-500 hover:border-blue-400 hover:text-blue-600 transition-colors ${
            error ? "border-red-400" : "border-gray-300"
          }`}
        >
          <Upload className="w-4 h-4" />
          Click to upload CV (PDF or Word, max 5MB)
        </button>
      ) : (
        <div className="flex items-center justify-between px-3 py-2.5 border border-gray-300 rounded-lg bg-slate-50">
          <div className="flex items-center gap-2 text-sm text-gray-700 truncate">
            <FileText className="w-4 h-4 text-blue-600 flex-none" />
            <span className="truncate">{selectedFile.name}</span>
          </div>
          <button
            type="button"
            onClick={() => field.onChange(null)}
            aria-label="Remove file"
            className="text-gray-400 hover:text-red-600 flex-none"
          >
            <X className="w-4 h-4" />
          </button>
        </div>
      )}
 <input
        ref={inputRef}
        type="file"
        accept=".pdf,.doc,.docx"
        className="hidden"
        onChange={(e) => {
          const file = e.target.files?.[0] ?? null;
          field.onChange(file); // manually push the File object into RHF state
          e.target.value = "";  // reset so selecting the SAME file again still fires onChange
        }}
      />

      {error && <p role="alert" className="text-red-500 text-xs mt-1.5">{error.message}</p>}


    </div>
   )
}

export default CvUploadField;