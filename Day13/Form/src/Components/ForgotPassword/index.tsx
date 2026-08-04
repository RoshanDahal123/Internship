
import { useForm } from "@tanstack/react-form";
import { useEffect, useState } from "react";
import toast from "react-hot-toast";
import { useNavigate } from "react-router-dom";
import z from "zod";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { Label } from "../ui/label";

const REQUEST_OTP_URL = import.meta.env.VITE_API_URL_FORGOT_PASSWORD ?? "https://localhost:7239/api/auth/forgot-password";
const VERIFY_OTP_URL = import.meta.env.VITE_API_URL_VERIFY_OTP ?? "https://localhost:7239/api/auth/verify-otp";
const RESET_PASSWORD_URL = import.meta.env.VITE_API_URL_RESET_PASSWORD ?? "https://localhost:7239/api/auth/reset-password";

const RSEND_COLLDOWN_SECONDS=60;

type Step = "email"|"otp"|"reset";


async function postJson<T>(url:string, body:unknown):Promise<T>{
    const response = await fetch(url,{

        method:"POST",
        headers:{"Content-Type":"application/json"},
        
        body:JSON.stringify(body),})
        
        if(!response.ok){
            const message= await response 
            .json()
            .then(data=>data?.message)
            .catch(()=>null);
            throw new Error(message?? `Request failed with status${response.status}`) 
        }
        return response.json();
    }

export function ForgotPassoword(){
    const navigate= useNavigate();
    const [ step, setStep]=useState<Step>("email");
    const [isSubmitting,setIsSubmitting]= useState(false);

    const [cooldown, setCooldown]= useState(0);

  // registeredEmail is carried forward from step 1 into steps 2 and 3,
  // since the OTP verify + reset calls both need it and we don't want
  // the user to retype it.

  const[registeredEmail, setRegisteredEmail]=useState("");

 useEffect(() => {
    if (cooldown <= 0) return;
    const timer = setInterval(() => setCooldown((s) => s - 1), 1000);
    return () => clearInterval(timer);
  }, [cooldown]);
  // countdown ticker for the resend button


  //-----step1-- request otp-----

  const emailForm= useForm({
    defaultValues:{email:""},
    onSubmit:async({value})=>{
        setIsSubmitting(true);
        try{
            await postJson(REQUEST_OTP_URL,{email:value.email})
            setRegisteredEmail(value.email);
            setCooldown(RSEND_COLLDOWN_SECONDS);
            setStep('otp');
            toast.success("Otp send to your email");
        }
        catch(error){
            toast.error(error instanceof Error? error.message:"Couldn't send OTP" );
        }
        finally{
            setIsSubmitting(false);
        }
    }
})

// ---step2----verifyOtp

const otpForm = useForm({
    defaultValues:{
        otp:""
    },

    onSubmit:async({value})=>{
     setIsSubmitting(true);
     try{
       await postJson(VERIFY_OTP_URL,{email:registeredEmail,otp:value.otp})
       setStep("reset");
       toast.success("OTP confirmed ");
     }
     catch(error){
      toast.error(error instanceof Error ? error.message:"Invalid or expired Otp");
     }finally{
        setIsSubmitting(false);
     }
    }
})


const handleResend= async ()=>{
    if(cooldown>0)return;
    setIsSubmitting(true);
    try{
        await postJson(REQUEST_OTP_URL,{email:registeredEmail});
        setCooldown(RSEND_COLLDOWN_SECONDS);
        toast.success("OTP resent");
    }
    catch(error){
        toast.error(error instanceof Error? error.message:"Couldn't resend OTP");
    }
    finally{
        setIsSubmitting(false);
    }
}


const resetForm = useForm({
    defaultValues:{newPassword:"", confirmPassword:""},
    onSubmit:async({value})=>{
        if(value.newPassword !==value.confirmPassword){
            toast.error("Password do not match");
        }
        setIsSubmitting(true);

        try{
            //server should re-validate the OTP server-side (short-lived token or the OTP itself) rather thatn trusting the client step state
            await postJson(RESET_PASSWORD_URL,{email:registeredEmail, otp:otpForm.state.values.otp,newPassword:value.newPassword});
            toast.success("Passowrd updated-please log in");
            navigate("/login");
        }
        catch(error){

            toast.error(error instanceof Error? error.message:"Couldn't reset password")
        }finally{
            setIsSubmitting(false);
        }
    }
})

return (
  <div
    className="w-full max-w-md rounded-2xl border border-white/10 bg-[#1e1b2e]/80
                 backdrop-blur-sm p-8 shadow-[0_0_60px_-15px_rgba(242,166,90,0.15)]"
  >

<div className="mb-6 text-center">
        <h2 className="text-2xl font-semibold text-white">
          {step === "email" && "Reset your password"}
          {step === "otp" && "Check your inbox"}
          {step === "reset" && "Choose a new password"}
        </h2>
        <p className="mt-1 text-sm text-neutral-400">
          {step === "email" && "Enter the email tied to your account."}
          {step === "otp" && `We sent a code to ${registeredEmail}.`}
          {step === "reset" && "Make it something you haven't used before."}
        </p>
      </div>

 {/* Step indicator */}
      <div className="mb-6 flex items-center justify-center gap-2">
        {(["email", "otp", "reset"] as Step[]).map((s, i) => (
          <div
            key={s}
            className={`h-1.5 w-8 rounded-full transition-colors ${
              step === s || (["otp", "reset"] as Step[]).indexOf(step) > i
                ? "bg-[#f2a65a]"
                : "bg-white/10"
            }`}
          />
        ))}
      </div>
{/* ---------- Step 1: email ---------- */}
{step === "email" && (
        <form
          onSubmit={(e) => {
            e.preventDefault();
            e.stopPropagation();
            emailForm.handleSubmit();
          }}
          className="flex flex-col gap-4"
        >
          <emailForm.Field
            name="email"
            validators={{ onChange: z.string().email("Enter a valid email address") }}
          >
            {(field) => (
              <div className="flex flex-col gap-1.5">
                <Label htmlFor={field.name} className="text-xs font-medium text-neutral-400">
                  Email
                </Label>
                <Input
                  id={field.name}
                  name={field.name}
                  type="email"
                  autoComplete="email"
                  placeholder="name@company.com"
                  value={field.state.value}
                  onBlur={field.handleBlur}
                  onChange={(e) => field.handleChange(e.target.value)}
                  className="border-white/10 bg-black/30 text-white placeholder:text-neutral-600
                           focus-visible:ring-2 focus-visible:ring-[#f2a65a]/60 focus-visible:border-[#f2a65a]/50"
                />
                {field.state.meta.isTouched && field.state.meta.errors.length ? (
                  <span className="text-xs text-red-400">
                    {field.state.meta.errors.map((error) => error?.message).join(", ")}
                  </span>
                ) : null}
              </div>
            )}
          </emailForm.Field>

          <Button
            type="submit"
            disabled={isSubmitting}
            className="mt-2 w-full rounded-lg bg-[#f2a65a] py-2.5 font-semibold text-[#2a1a08]
                       transition-colors hover:bg-[#f4b876] disabled:cursor-not-allowed disabled:opacity-60"
          >
            {isSubmitting ? "Sending..." : "Send OTP"}
          </Button>
        </form>
      )}

 {/* ---------- Step 2: OTP ---------- */}
      {step === "otp" && (
        <form
          onSubmit={(e) => {
            e.preventDefault();
            e.stopPropagation();
            otpForm.handleSubmit();
          }}
          className="flex flex-col gap-4"
        >
          <otpForm.Field
            name="otp"
            validators={{
              onChange: z
                .string()
                .length(6, "Enter the 6-digit code")
                .regex(/^\d+$/, "Digits only"),
            }}
          >
            {(field) => (
              <div className="flex flex-col gap-1.5">
                <Label htmlFor={field.name} className=" text-xs font-medium text-neutral-400">
                  6-digit code
                </Label>
                <Input
                  id={field.name}
                  name={field.name}
                  inputMode="numeric"
                  maxLength={6}
                  placeholder="123456"
                  value={field.state.value}
                  onBlur={field.handleBlur}
                  onChange={(e) => field.handleChange(e.target.value.replace(/\D/g, ""))}
                  className="border-white/10 bg-black/30 text-center text-lg tracking-[0.5em] text-white
                             placeholder:text-neutral-600 focus-visible:ring-2 focus-visible:ring-[#f2a65a]/60
                             focus-visible:border-[#f2a65a]/50"
                />
                {field.state.meta.isTouched && field.state.meta.errors.length ? (
                  <span className="text-xs text-red-400">
                    {field.state.meta.errors.map((error) => error?.message).join(", ")}
                  </span>
                ) : null}
              </div>
            )}
          </otpForm.Field>

          <Button
            type="submit"
            disabled={isSubmitting}
            className="mt-1 w-full rounded-lg bg-[#f2a65a] py-2.5 font-semibold text-[#2a1a08]
                       transition-colors hover:bg-[#f4b876] disabled:cursor-not-allowed disabled:opacity-60"
          >
            {isSubmitting ? "Verifying..." : "Verify code"}
          </Button>

          <button
            type="button"
            onClick={handleResend}
            disabled={cooldown > 0 || isSubmitting}
            className="text-center text-sm text-neutral-400 transition-colors hover:text-[#f2a65a]
                       disabled:cursor-not-allowed disabled:opacity-50 disabled:hover:text-neutral-400"
          >
            {cooldown > 0 ? `Resend code in ${cooldown}s` : "Resend code"}
          </button>
        </form>
      )}

      {/* ---------- Step 3: new password ---------- */}
      {step === "reset" && (
        <form
          onSubmit={(e) => {
            e.preventDefault();
            e.stopPropagation();
            resetForm.handleSubmit();
          }}
          className="flex flex-col gap-4"
        >
          <resetForm.Field
            name="newPassword"
            validators={{ onChange: z.string().min(8, "At least 8 characters") }}
          >
            {(field) => (
              <div className="flex flex-col gap-1.5">
                <Label htmlFor={field.name} className="text-xs font-medium text-neutral-400">
                  New password
                </Label>
                <Input
                  id={field.name}
                  name={field.name}
                  type="password"
                  autoComplete="new-password"
                  value={field.state.value}
                  onBlur={field.handleBlur}
                  onChange={(e) => field.handleChange(e.target.value)}
                  className="border-white/10 bg-black/30 text-white placeholder:text-neutral-600
                             focus-visible:ring-2 focus-visible:ring-[#f2a65a]/60 focus-visible:border-[#f2a65a]/50"
                />
                {field.state.meta.isTouched && field.state.meta.errors.length ? (
                  <span className="text-xs text-red-400">
                    {field.state.meta.errors.map((error) => error?.message).join(", ")}
                  </span>
                ) : null}
              </div>
            )}
          </resetForm.Field>

          <resetForm.Field
            name="confirmPassword"
            validators={{ onChange: z.string().min(8, "At least 8 characters") }}
          >
            {(field) => (
              <div className="flex flex-col gap-1.5">
                <Label htmlFor={field.name} className="text-xs font-medium text-neutral-400">
                  Confirm password
                </Label>
                <Input
                  id={field.name}
                  name={field.name}
                  type="password"
                  autoComplete="new-password"
                  value={field.state.value}
                  onBlur={field.handleBlur}
                  onChange={(e) => field.handleChange(e.target.value)}
                  className="border-white/10 bg-black/30 text-white placeholder:text-neutral-600
                             focus-visible:ring-2 focus-visible:ring-[#f2a65a]/60 focus-visible:border-[#f2a65a]/50"
                />
                {field.state.meta.isTouched && field.state.meta.errors.length ? (
                  <span className="text-xs text-red-400">
                    {field.state.meta.errors.map((error) => error?.message).join(", ")}
                  </span>
                ) : null}
              </div>
            )}
          </resetForm.Field>

          <Button
            type="submit"
            disabled={isSubmitting}
            className="mt-2 w-full rounded-lg bg-[#f2a65a] py-2.5 font-semibold text-[#2a1a08]
                       transition-colors hover:bg-[#f4b876] disabled:cursor-not-allowed disabled:opacity-60"
          >
            {isSubmitting ? "Updating..." : "Change password"}
          </Button>
        </form>
      )}
  </div>
);
}



