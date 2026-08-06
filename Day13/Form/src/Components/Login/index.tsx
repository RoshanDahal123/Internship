import { useForm } from "@tanstack/react-form";
import { useState } from "react";
import toast from "react-hot-toast";
import { z } from "zod";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import { Link, useNavigate } from "react-router-dom";
import { postJson } from "../../api/client";



export function Login() {
  const [isSubmitting, setIsSubmitting] = useState(false);
    
        const navigate = useNavigate();

  const form = useForm({
    defaultValues: {
      email: "",
      password: "",
    },

    onSubmit: async ({ value }) => {
      setIsSubmitting(true);
      try {
       await postJson("/auth/login",value);
       toast.success("Welcome Back");
       navigate("/dashboard");
      } catch (error) {
        console.error("Login error:", error);
        const message = error instanceof Error ? error.message : "Login failed. Please try again.";
        toast.error(message);
      } finally {
        setIsSubmitting(false);
      }
    },
  });

  return (
    <div
      className="w-full max-w-md rounded-2xl border border-white/10 bg-[#1e1b2e]/80
                 backdrop-blur-sm p-8 shadow-[0_0_60px_-15px_rgba(242,166,90,0.15)]"
    >
      <div className="mb-6 text-center">
        <h2 className="text-2xl font-semibold text-white">Welcome back</h2>
        <p className="mt-1 text-sm text-neutral-400">
          Log in to continue to your account.
        </p>
      </div>

      <form
        onSubmit={(e) => {
          e.preventDefault();
          e.stopPropagation();
          form.handleSubmit();
        }}
        className="flex flex-col gap-4"
      >
        {/* Email */}
        <form.Field
          name="email"
          validators={{
            onChange: z.string().email("Enter a valid email address"),
          }}
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
        </form.Field>

        {/* Password */}
        <form.Field
          name="password"
          validators={{
            onChange: z.string().min(1, "Password is required"),
          }}
        >
          {(field) => (
            
            <div className="flex flex-col gap-1.5">
              <div className="flex items-center justify-between">
                <Label htmlFor={field.name} className="text-xs font-medium text-neutral-400">
                  Password
                </Label>
               <Link to="/forgot-password"
                  className="text-xs text-[#f2a65a] hover:text-[#f4b876] transition-colors"
                >
                  Forgot password?
                </Link>
              </div>
              <Input
                id={field.name}
                name={field.name}
                type="password"
                autoComplete="current-password"
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
        </form.Field>

        <Button
          type="submit"
          disabled={isSubmitting}
          className="mt-2 w-full rounded-lg bg-[#f2a65a] py-2.5 font-semibold text-[#2a1a08]
                     transition-colors hover:bg-[#f4b876] disabled:cursor-not-allowed disabled:opacity-60"
        >
          {isSubmitting ? "Logging in..." : "Log in"}
        </Button>

        <p className="mt-1 text-center text-sm text-neutral-400">
          Don't have an account?{" "}
          <Link to="/signup" className="text-[#f2a65a] hover:text-[#f4b876] transition-colors">
            Sign up
          </Link>
        </p>
      </form>
    </div>
  );
}