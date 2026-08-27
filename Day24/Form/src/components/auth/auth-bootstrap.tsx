// src/components/auth/auth-bootstrap.tsx
import { Loader2 } from "lucide-react";
import * as React from "react";
import { useRefreshMutation } from "../../features/authApiSlice";
import {
  clearCredentials,
  selectAccessTokenExpiresAt,
  setCredentials,
} from "../../features/authSlice";
import { useAppDispatch, useAppSelector } from "../../hooks/reducer-hook";

/**
 * Keeps an idle session alive by refreshing the access token ~60s before
 * it expires, instead of waiting for the next API call to 401 and retry.
 * Both work correctly — this just avoids a visible request-retry delay
 * for a user who's been sitting on the page without navigating.
 */
function useScheduledRefresh() {
  const dispatch = useAppDispatch();
  const accessToken = useAppSelector((state) => state.auth.accessToken);
  const accessTokenExpiresAt = useAppSelector(selectAccessTokenExpiresAt);
  const [refresh] = useRefreshMutation();

  React.useEffect(() => {
    if (!accessToken || !accessTokenExpiresAt) return;

    const msUntilExpiry = new Date(accessTokenExpiresAt).getTime() - Date.now();
    const msUntilRefresh = Math.max(msUntilExpiry - 60_000, 5_000);

    const timerId = setTimeout(async () => {
      try {
        dispatch(setCredentials(await refresh().unwrap()));
      } catch {
        dispatch(clearCredentials());
      }
    }, msUntilRefresh);

    return () => clearTimeout(timerId);
  }, [accessToken, accessTokenExpiresAt, dispatch, refresh]);
}

/**
 * Runs once on app load. We have no client-visible way to know whether a
 * valid refresh cookie exists — httpOnly means JS can't read it — so we
 * always attempt one silent /auth/refresh call and let the response (or a
 * 401) tell us the answer, instead of gating the attempt on local state
 * the way the old localStorage version did.
 */
export function AuthBootstrap({ children }: { children: React.ReactNode }) {
  const dispatch = useAppDispatch();
  const [refresh] = useRefreshMutation();
  const [isReady, setIsReady] = React.useState(false);

  React.useEffect(() => {
    let cancelled = false;

    async function bootstrap() {
      try {
        const result = await refresh().unwrap();
        if (!cancelled) dispatch(setCredentials(result));
      } catch {
        // 401 here just means "no valid session cookie" — a normal,
        // expected state for a guest visitor, not a failure to handle specially.
        if (!cancelled) dispatch(clearCredentials());
      } finally {
        if (!cancelled) setIsReady(true);
      }
    }

    bootstrap();
    return () => {
      cancelled = true;
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useScheduledRefresh();

  if (!isReady) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <Loader2 className="size-6 animate-spin text-muted-foreground" />
      </div>
    );
  }

  return <>{children}</>;
}