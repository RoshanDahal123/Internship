

import {
  Calendar,
  FileText,
  GraduationCap,
  Mail,
  MapPin,
  Plus,
  Trash2,
  User,
} from "lucide-react"
import * as React from "react"
import { Controller, useFieldArray, useForm } from "react-hook-form"
import { useNavigate, useParams } from "react-router"
import { toast } from "sonner"

import { Button } from "../../components/ui/button"
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
} from "../../components/ui/card"
import {
  Field,
  FieldDescription,
  FieldError,
  FieldGroup,
  FieldLabel,
} from "../../components/ui/field"
import { Input } from "../../components/ui/input"
import {
  InputGroup,
  InputGroupAddon,
  InputGroupText,
  InputGroupTextarea,
} from "../../components/ui/input-group"
import { Skeleton } from "../../components/ui/skeleton"
import {
  useCreateStudentMutation,
  useGetStudentByIdQuery,
  useUpdateStudentMutation,
} from "../../features/studentApiSlice"
import { Student } from "../../types/formTypes"

interface StudentFormProps {
  mode: "create" | "edit"
}

const emptyDefaults: Student = {
  name: "",
  email: "",
  age: 1,
  address: "",
  description: "",
  dateOfBirth: null,
  cvFile: null,
  education: [],
}

export default function StudentForm({ mode }: StudentFormProps) {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()
  const isEdit = mode === "edit"

  // Only fires in edit mode, once we have an id
  const {
    data: existingStudent,
    isLoading: isFetchingStudent,
    isError: isFetchError,
  } = useGetStudentByIdQuery(Number(id), {
    skip: !isEdit || !id,
  })

  const [createStudent, { isLoading: isCreating, isError:isCreateError }] =
    useCreateStudentMutation()
  const [updateStudent, { isLoading: isUpdating, isError:isUpdateError }] =
    useUpdateStudentMutation()
  const isSubmitting = isCreating || isUpdating;


  const form = useForm<Student>({
    defaultValues: emptyDefaults,
  })

  const { fields, append, remove } = useFieldArray({
    control: form.control,
    name: "education",
  })
  

  // Fill the form once the student record arrives
  React.useEffect(() => {
    if (isEdit && existingStudent) {
      form.reset({
        name: existingStudent.name,
        email: existingStudent.email,
        age: existingStudent.age,
        address: existingStudent.address,
        description: existingStudent.description,
        dateOfBirth: existingStudent.dateOfBirth
          ? new Date(existingStudent.dateOfBirth)
          : null,
        // The previously uploaded file isn't re-loaded into the <input type="file">;
        // the description note below tells the user their existing CV stays on file
        // unless they choose a new one.
        cvFile: null,
        education: existingStudent.education ?? [],
      })
    }
  }, [isEdit, existingStudent, form])


  //Surface failures
React.useEffect(() => {
  if (isCreateError || isUpdateError) {
    toast.error(`Failed to ${isEdit ? "update" : "create"} student. Please try again.`)
  } else{
    navigate("/students");
  }
}, [isCreateError, isUpdateError, isEdit])


   function onSubmit(data: Student) {
    try {
      if (isEdit && id) {
         updateStudent({id:Number(id),data})
      } else {
       createStudent(data)
      }
    } catch (err) {
      toast.error(
        `Failed to ${isEdit ? "update" : "create"} student. Please try again.`
      )
    }
  }

  const title = isEdit ? "Edit Student" : "Add Student"
  const submitLabel = isEdit ? "Save Changes" : "Create Student"

  // Edit mode, still fetching the record: show a skeleton instead of an empty form
  if (isEdit && isFetchingStudent) {
    return (
      <div className="min-h-screen w-full flex items-center justify-center p-4">
        <Card className="w-full sm:max-w-xl">
          <CardHeader className="text-center">
            <CardTitle className="text-2xl font-bold tracking-tight">
              {title}
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            {Array.from({ length: 5 }).map((_, i) => (
              <Skeleton key={i} className="h-10 w-full" />
            ))}
          </CardContent>
        </Card>
      </div>
    )
  }

  // Edit mode, fetch failed: don't render a form with nothing to save against
  if (isEdit && isFetchError) {
    return (
      <div className="min-h-screen w-full flex items-center justify-center p-4">
        <Card className="w-full sm:max-w-md">
          <CardContent className="pt-6 text-center space-y-4">
            <p className="text-sm text-muted-foreground">
              We couldn't load this student. They may have been removed.
            </p>
            <Button variant="outline" onClick={() => navigate("/students")}>
              Back to Students
            </Button>
          </CardContent>
        </Card>
      </div>
    )
  }

  return (
    <div className="min-h-screen w-full flex items-center justify-center p-4">
      <Card className="w-full sm:max-w-xl">
        <CardHeader className="text-center">
          <CardTitle className="text-2xl font-bold tracking-tight">
            {title}
          </CardTitle>
        </CardHeader>
        <CardContent>
          <form id="form-rhf-demo" onSubmit={form.handleSubmit(onSubmit)}>
            <FieldGroup>
              {/* Name */}
              <Controller
                name="name"
                control={form.control}
                rules={{ required: "Name is required" }}
                render={({ field, fieldState }) => (
                  <Field data-invalid={fieldState.invalid}>
                    <FieldLabel
                      htmlFor="form-rhf-demo-name"
                      className="flex items-center gap-1.5"
                    >
                      <User className="size-4 text-muted-foreground" />
                      Full Name
                    </FieldLabel>
                    <Input
                      {...field}
                      id="form-rhf-demo-name"
                      aria-invalid={fieldState.invalid}
                      placeholder="John Doe"
                      autoComplete="off"
                    />
                    {fieldState.invalid && (
                      <FieldError errors={[fieldState.error]} />
                    )}
                  </Field>
                )}
              />

              {/* Email */}
              <Controller
                name="email"
                control={form.control}
                rules={{ required: "Email is required" }}
                render={({ field, fieldState }) => (
                  <Field data-invalid={fieldState.invalid}>
                    <FieldLabel
                      htmlFor="form-rhf-demo-email"
                      className="flex items-center gap-1.5"
                    >
                      <Mail className="size-4 text-muted-foreground" />
                      Email Address
                    </FieldLabel>
                    <Input
                      {...field}
                      id="form-rhf-demo-email"
                      type="email"
                      aria-invalid={fieldState.invalid}
                      placeholder="john@example.com"
                      autoComplete="off"
                    />
                    {fieldState.invalid && (
                      <FieldError errors={[fieldState.error]} />
                    )}
                  </Field>
                )}
              />

              {/* Age + Address share a row */}
              <div className="grid grid-cols-3 gap-4">
                <div className="col-span-1">
                  <Controller
                    name="age"
                    control={form.control}
                    rules={{
                      required: "Required",
                      valueAsNumber: true,
                      
                      min: { value: 1, message: "Min 1" },
                      max: { value: 100, message: "Max 100" },
                    }}
                    render={({ field, fieldState }) => (
                      <Field data-invalid={fieldState.invalid}>
                        <FieldLabel
                          htmlFor="form-rhf-demo-age"
                          className="flex items-center gap-1.5"
                        >
                          <Calendar className="size-4 text-muted-foreground" />
                          Age
                        </FieldLabel>
                        <Input
                          {...field}
                          id="form-rhf-demo-age"
                          type="number"
                          aria-invalid={fieldState.invalid}
                          onChange={(e) =>
                            field.onChange(
                              e.target.value === ""
                                ? ""
                                : Number(e.target.value)
                            )
                          }
                        />
                        {fieldState.invalid && (
                          <FieldError errors={[fieldState.error]} />
                        )}
                      </Field>
                    )}
                  />
                </div>

                <div className="col-span-2">
                  <Controller
                    name="address"
                    control={form.control}
                    rules={{ required: "Address is required" }}
                    render={({ field, fieldState }) => (
                      <Field data-invalid={fieldState.invalid}>
                        <FieldLabel
                          htmlFor="form-rhf-demo-address"
                          className="flex items-center gap-1.5"
                        >
                          <MapPin className="size-4 text-muted-foreground" />
                          Address
                        </FieldLabel>
                        <Input
                          {...field}
                          id="form-rhf-demo-address"
                          aria-invalid={fieldState.invalid}
                          placeholder="123 Main St, City, Country"
                          autoComplete="off"
                        />
                        {fieldState.invalid && (
                          <FieldError errors={[fieldState.error]} />
                        )}
                      </Field>
                    )}
                  />
                </div>
              </div>

              {/* Date of Birth + CV upload share a row */}
              <div className="grid grid-cols-3 gap-4">
                <div className="col-span-1">
                  <Controller
                    name="dateOfBirth"
                    control={form.control}
                    rules={{ required: "Date of birth is required" }}
                    render={({ field, fieldState }) => (
                      <Field data-invalid={fieldState.invalid}>
                        <FieldLabel
                          htmlFor="form-rhf-demo-dob"
                          className="flex items-center gap-1.5"
                        >
                          <Calendar className="size-4 text-muted-foreground" />
                          Date of Birth
                        </FieldLabel>
                        <Input
                          id="form-rhf-demo-dob"
                          type="date"
                          aria-invalid={fieldState.invalid}
                          value={
                            field.value
                              ? new Date(field.value)
                                  .toISOString()
                                  .slice(0, 10)
                              : ""
                          }
                          onChange={(e) =>
                            field.onChange(
                              e.target.value
                                ? new Date(e.target.value)
                                : null
                            )
                          }
                        />
                        {fieldState.invalid && (
                          <FieldError errors={[fieldState.error]} />
                        )}
                      </Field>
                    )}
                  />
                </div>

                <div className="col-span-2">
                  <Controller
                    name="cvFile"
                    control={form.control}
                    render={({ field, fieldState }) => (
                      <Field data-invalid={fieldState.invalid}>
                        <FieldLabel
                          htmlFor="form-rhf-demo-cv"
                          className="flex items-center gap-1.5"
                        >
                          <FileText className="size-4 text-muted-foreground" />
                          CV / Resume
                        </FieldLabel>
                        <Input
                          id="form-rhf-demo-cv"
                          type="file"
                          accept=".pdf,.doc,.docx"
                          aria-invalid={fieldState.invalid}
                          onChange={(e) =>
                            field.onChange(e.target.files?.[0] ?? null)
                          }
                        />
                        <FieldDescription>
                          {field.value instanceof File
                            ? field.value.name
                            : isEdit
                              ? "Leave empty to keep the current file on record."
                              : "PDF or Word document, up to 5MB."}
                        </FieldDescription>
                        {fieldState.invalid && (
                          <FieldError errors={[fieldState.error]} />
                        )}
                      </Field>
                    )}
                  />
                </div>
              </div>

              {/* Description */}
              <Controller
                name="description"
                control={form.control}
                rules={{ required: "Description is required" }}
                render={({ field, fieldState }) => (
                  <Field data-invalid={fieldState.invalid}>
                    <FieldLabel htmlFor="form-rhf-demo-description">
                      Description
                    </FieldLabel>
                    <InputGroup>
                      <InputGroupTextarea
                        {...field}
                        id="form-rhf-demo-description"
                        placeholder="Tell us a bit about yourself"
                        rows={4}
                        className="min-h-24 resize-none"
                        aria-invalid={fieldState.invalid}
                      />
                      <InputGroupAddon align="block-end">
                        <InputGroupText className="tabular-nums">
                          {field.value.length}/500 characters
                        </InputGroupText>
                      </InputGroupAddon>
                    </InputGroup>
                    {fieldState.invalid && (
                      <FieldError errors={[fieldState.error]} />
                    )}
                  </Field>
                )}
              />

              {/* Education */}
              <Field>
                <div className="flex items-center justify-between">
                  <FieldLabel className="flex items-center gap-2">
                    <GraduationCap className="size-4" />
                    Education
                  </FieldLabel>
                  <Button
                    type="button"
                    variant="outline"
                    size="sm"
                    onClick={() =>
                      append({
                        degree: "",
                        institutionName: "",
                        year: new Date().getFullYear(),
                      })
                    }
                  >
                    <Plus className="size-4" />
                    Add Education
                  </Button>
                </div>

                {fields.length === 0 && (
                  <FieldDescription className="border border-dashed rounded-lg py-4 text-center">
                    No education added yet — click "Add Education" to include
                    your qualifications.
                  </FieldDescription>
                )}

                <div className="space-y-3">
                  {fields.map((item, index) => (
                    <div
                      key={item.id}
                      className="grid grid-cols-1 sm:grid-cols-[1fr_1fr_100px_auto] gap-3 items-start border rounded-lg p-3"
                    >
                      <Controller
                        name={`education.${index}.degree`}
                        control={form.control}
                        rules={{ required: "Required" }}
                        render={({ field, fieldState }) => (
                          <Field data-invalid={fieldState.invalid}>
                            <Input
                              {...field}
                              placeholder="Degree"
                              aria-invalid={fieldState.invalid}
                            />
                            {fieldState.invalid && (
                              <FieldError errors={[fieldState.error]} />
                            )}
                          </Field>
                        )}
                      />

                      <Controller
                        name={`education.${index}.institutionName`}
                        control={form.control}
                        rules={{ required: "Required" }}
                        render={({ field, fieldState }) => (
                          <Field data-invalid={fieldState.invalid}>
                            <Input
                              {...field}
                              placeholder="Institution"
                              aria-invalid={fieldState.invalid}
                            />
                            {fieldState.invalid && (
                              <FieldError errors={[fieldState.error]} />
                            )}
                          </Field>
                        )}
                      />

                      <Controller
                        name={`education.${index}.year`}
                        control={form.control}
                        rules={{ required: "Required" }}
                        render={({ field, fieldState }) => (
                          <Field data-invalid={fieldState.invalid}>
                            <Input
                              {...field}
                              type="number"
                              placeholder="Year"
                              aria-invalid={fieldState.invalid}
                              onChange={(e) =>
                                field.onChange(
                                  e.target.value === ""
                                    ? ""
                                    : Number(e.target.value)
                                )
                              }
                            />
                            {fieldState.invalid && (
                              <FieldError errors={[fieldState.error]} />
                            )}
                          </Field>
                        )}
                      />

                      <Button
                        type="button"
                        variant="ghost"
                        size="icon"
                        onClick={() => remove(index)}
                        aria-label="Remove education entry"
                      >
                        <Trash2 className="size-4" />
                      </Button>
                    </div>
                  ))}
                </div>
              </Field>
            </FieldGroup>
          </form>
        </CardContent>
        <CardFooter>
          <Field orientation="horizontal">
            <Button
              type="button"
              variant="outline"
              onClick={() => navigate("/students")}
            >
              Cancel
            </Button>
            <Button type="submit" form="form-rhf-demo" disabled={isSubmitting}>
              {isSubmitting ? "Saving..." : submitLabel}
            </Button>
          </Field>
        </CardFooter>
      </Card>
    </div>
  )
}