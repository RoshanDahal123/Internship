

import { z } from 'zod';

export const loginSchema= z.object({
    email:z.string().min(1,"Email is required"). email("Enter a valid email address"),
     password: z.string().min(1, "Password is required")
})

export type LoginFormValues = z.infer<typeof loginSchema>;

export const registerSchema = z
  .object({
    email: z.string().min(1, "Email is required").email("Enter a valid email address"),
    password: z.string().min(8, "Password must be at least 8 characters"),
    confirmPassword: z.string().min(1, "Please confirm your password"),
    setupKey: z.string().min(1, "Setup key is required"),
  })
  .refine((data)=>data.password=== data.confirmPassword,{
    message:"Passwords do not match",
    path:["confirmPassword"]
  })

  export type RegisterFormValues= z.infer<typeof registerSchema>;