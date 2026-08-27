// src/pages/Login/index.tsx
import { zodResolver } from "@hookform/resolvers/zod";
import { LogIn, Mail } from "lucide-react";
import { Controller, useForm } from "react-hook-form";
import { Link, useLocation, useNavigate, type Location } from "react-router";
import { toast } from "sonner";
import { PasswordInput } from "../../components/password-input";
import { useLoginMutation } from "../../features/authApiSlice";
import { setCredentials } from "../../features/authSlice";
import { useAppDispatch } from "../../hooks/reducer-hook";
import { getErrorStatus } from "../../lib/api-error";
import { loginSchema, type LoginFormValues } from "../../lib/authSchemas";
import { Button } from "../../components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "../../components/ui/card";
import { Field, FieldError, FieldGroup, FieldLabel } from "../../components/ui/field";
import { Input } from "../../components/ui/input";

export default function Login() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const location = useLocation();

  const [login, { isLoading }] = useLoginMutation();

  const form = useForm<LoginFormValues>({
    resolver: zodResolver(loginSchema),
    defaultValues: { email: "", password: "" },
  });

  async function onSubmit(values: LoginFormValues) {
    try {
      const result = await login(values).unwrap();
      dispatch(setCredentials(result));
      toast.success(`Welcome back, ${result.email}`);

      // Send the user back to whatever protected page sent them to /login
      // (RequireAdmin sets this), otherwise the homepage.
      const from = (location.state as { from?: Location } | null)?.from;
      navigate(from?.pathname ?? "/", { replace: true });
    } catch (err) {
      const status = getErrorStatus(err);
      toast.error(
        status === 401
          ? "Incorrect email or password."
          : "Something went wrong. Please try again.",
      );
    }
  }

  return (
    <div className="flex min-h-[calc(100vh-3.5rem)] w-full items-center justify-center p-4">
      <Card className="w-full sm:max-w-sm">
        <CardHeader className="text-center">
          <CardTitle className="text-2xl font-bold tracking-tight">Admin Login</CardTitle>
          <CardDescription>Sign in to manage the student directory.</CardDescription>
        </CardHeader>
        <CardContent>
          <form id="login-form" onSubmit={form.handleSubmit(onSubmit)}>
            <FieldGroup>
              <Controller
                name="email"
                control={form.control}
                render={({ field, fieldState }) => (
                  <Field data-invalid={fieldState.invalid}>
                    <FieldLabel htmlFor="login-email" className="flex items-center gap-1.5">
                      <Mail className="size-4 text-muted-foreground" />
                      Email
                    </FieldLabel>
                    <Input
                      {...field}
                      id="login-email"
                      type="email"
                      autoComplete="email"
                      placeholder="admin@example.com"
                      aria-invalid={fieldState.invalid}
                    />
                    {fieldState.invalid && <FieldError errors={[fieldState.error]} />}
                  </Field>
                )}
              />

              <Controller
                name="password"
                control={form.control}
                render={({ field, fieldState }) => (
                  <Field data-invalid={fieldState.invalid}>
                    <FieldLabel htmlFor="login-password">Password</FieldLabel>
                    <PasswordInput
                      {...field}
                      id="login-password"
                      autoComplete="current-password"
                      aria-invalid={fieldState.invalid}
                    />
                    {fieldState.invalid && <FieldError errors={[fieldState.error]} />}
                  </Field>
                )}
              />
            </FieldGroup>
          </form>
        </CardContent>
        <CardFooter className="flex-col gap-4">
          <Button type="submit" form="login-form" className="w-full gap-2" disabled={isLoading}>
            <LogIn className="size-4" />
            {isLoading ? "Signing in..." : "Sign in"}
          </Button>
          <p className="text-sm text-muted-foreground">
            Need an admin account?{" "}
            <Link to="/register" className="font-medium text-primary hover:underline">
              Register
            </Link>
          </p>
        </CardFooter>
      </Card>
    </div>
  );
}