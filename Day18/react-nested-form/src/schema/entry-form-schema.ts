import { z } from "zod";
// Nested object schema — reused inside the entry schema below.

export const addressSchema= z.object({
    streetAddress:z.string().min(1,"Street address is required"),
    addressLine2:z.string().optional(),
    city:z.string().min(1,"City is required"),
    state:z.string().min(1,"State is required"),
    zipCode:z.string()
            .min(1,"Zip code is required")
            .regex(/^\d{5}(-\d{4})?$/, "Enter a valid ZIP code (e.g. 12345 or 12345-6789)"),
});


export const employerEntrySchema=z.object({
    employerName:z.string().min(1,"Employer name is required"),
    phone:z.string()
    .min(1,"Phone is required")
    .regex(/^[\d\s\-().+]{7,}$/, "Enter a valid phone number"),

    contactName:z.string().min(1,"Contact name is required"),
    address:addressSchema,
    position:z.string().min(1,"Position is required"),
     from: z.string().min(1, "From date is required"),
    to: z.string().min(1, "To date is required"),
    reasonForLeaving: z.string().min(1, "Reason for leaving is required"),
    //@ts-ignore
    subjectToFMCSR:z.enum(["yes","no"],{
        errorMap:()=>({messge:"Please select an option"}),
    }), 
    //@ts-ignore
    subjectToDrugTesting: z.enum(["yes", "no"], {
      errorMap: () => ({ message: "Please select an option" }),
    }),
})
.refine((data)=>new Date(data.to)>=new Date(data.from),{
         message: "To date must be on or after the From date",
    path: ["to"], // attaches the error to the `to` field specifically
    })


    export type EmployerEntryFormValues= z.infer<typeof employerEntrySchema>;

    export interface EmployerEntry extends EmployerEntryFormValues{
        id:string
    }

    