// src/components/auth/auth-bootstrap.tsx
import { Loader2 } from "lucide-react";
import * as React from "react";
import { useGetMeQuery } from "../../features/authApiSlice";
import { clearCredentials, setCredentials } from "../../features/authSlice";
import { useAppDispatch } from "../../hooks/reducer-hook";

export function AuthBootstrap({ children }: { children: React.ReactNode }) {
  const dispatch = useAppDispatch();
  const { data, isSuccess, isError, isLoading, isUninitialized } =
    useGetMeQuery();

  // Tracks whether we've resolved the auth state at least once. Once true,
  // it stays true forever — later refetches (e.g. cache reset on logout)
  // should never bring back the full-page spinner.
  const [hasCheckedAuth, setHasCheckedAuth] = React.useState(false);

  React.useEffect(() => {
    if (isSuccess) {
      dispatch(setCredentials(data));
      setHasCheckedAuth(true);
    }
    if (isError) {
      dispatch(clearCredentials());
      setHasCheckedAuth(true);
    }
  }, [data, isSuccess, isError, dispatch]);

  if (!hasCheckedAuth && (isLoading || isUninitialized)) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <Loader2 className="size-6 animate-spin text-muted-foreground" />
      </div>
    );
  }

  return <>{children}</>;
}