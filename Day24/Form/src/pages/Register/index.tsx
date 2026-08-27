// src/pages/Register/index.tsx
import { zodResolver } from "@hookform/resolvers/zod";
import { KeyRound, Mail, UserPlus } from "lucide-react";
import { Controller, useForm } from "react-hook-form";
import { Link, useNavigate } from "react-router";
import { toast } from "sonner";
import { PasswordInput } from "../../components/password-input";
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
import { useRegisterMutation } from "../../features/authApiSlice";
import { setCredentials } from "../../features/authSlice";
import { useAppDispatch } from "../../hooks/reducer-hook";
import { getErrorStatus } from "../../lib/api-error";
import { registerSchema, type RegisterFormValues } from "../../lib/authSchemas";

export default function Register() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const [registerAdmin, { isLoading }] = useRegisterMutation();

  const form = useForm<RegisterFormValues>({
    resolver: zodResolver(registerSchema),
    defaultValues: { email: "", password: "", confirmPassword: "", setupKey: "" },
  });

  async function onSubmit(values: RegisterFormValues) {
    try {
      // Explicitly picking fields (rather than spreading `values`) keeps
      // the UI-only confirmPassword field from ever reaching the API.
      const result = await registerAdmin({
        email: values.email,
        password: values.password,
        setupKey: values.setupKey,
      }).unwrap();

      dispatch(setCredentials(result));
      toast.success("Admin account created.");
      navigate("/", { replace: true });
    } catch (err) {
      const status = getErrorStatus(err);
      toast.error(
        status === 401
          ? "Invalid setup key."
          : status === 400
            ? "That email is already registered, or the details are invalid."
            : "Something went wrong. Please try again.",
      );
    }
  }

  return (
    <div className="flex min-h-[calc(100vh-3.5rem)] w-full items-center justify-center p-4">
      <Card className="w-full sm:max-w-sm">
        <CardHeader className="text-center">
          <CardTitle className="text-2xl font-bold tracking-tight">
            Create Admin Account
          </CardTitle>
          <CardDescription>Requires a valid setup key from the backend config.</CardDescription>
        </CardHeader>
        <CardContent>
          <form id="register-form" onSubmit={form.handleSubmit(onSubmit)}>
            <FieldGroup>
              <Controller
                name="email"
                control={form.control}
                render={({ field, fieldState }) => (
                  <Field data-invalid={fieldState.invalid}>
                    <FieldLabel htmlFor="register-email" className="flex items-center gap-1.5">
                      <Mail className="size-4 text-muted-foreground" />
                      Email
                    </FieldLabel>
                    <Input
                      {...field}
                      id="register-email"
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
                    <FieldLabel htmlFor="register-password">Password</FieldLabel>
                    <PasswordInput
                      {...field}
                      id="register-password"
                      autoComplete="new-password"
                      aria-invalid={fieldState.invalid}
                    />
                    {fieldState.invalid && <FieldError errors={[fieldState.error]} />}
                  </Field>
                )}
              />

              <Controller
                name="confirmPassword"
                control={form.control}
                render={({ field, fieldState }) => (
                  <Field data-invalid={fieldState.invalid}>
                    <FieldLabel htmlFor="register-confirm-password">
                      Confirm Password
                    </FieldLabel>
                    <PasswordInput
                      {...field}
                      id="register-confirm-password"
                      autoComplete="new-password"
                      aria-invalid={fieldState.invalid}
                    />
                    {fieldState.invalid && <FieldError errors={[fieldState.error]} />}
                  </Field>
                )}
              />

              <Controller
                name="setupKey"
                control={form.control}
                render={({ field, fieldState }) => (
                  <Field data-invalid={fieldState.invalid}>
                    <FieldLabel
                      htmlFor="register-setup-key"
                      className="flex items-center gap-1.5"
                    >
                      <KeyRound className="size-4 text-muted-foreground" />
                      Setup Key
                    </FieldLabel>
                    <Input
                      {...field}
                      id="register-setup-key"
                      type="password"
                      autoComplete="off"
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
          <Button
            type="submit"
            form="register-form"
            className="w-full gap-2"
            disabled={isLoading}
          >
            <UserPlus className="size-4" />
            {isLoading ? "Creating account..." : "Create account"}
          </Button>
          <p className="text-sm text-muted-foreground">
            Already have an account?{" "}
            <Link to="/login" className="font-medium text-primary hover:underline">
              Sign in
            </Link>
          </p>
        </CardFooter>
      </Card>
    </div>
  );
}