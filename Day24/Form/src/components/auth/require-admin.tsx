import { Loader2 } from "lucide-react";
import { Navigate, Outlet, useLocation } from "react-router";
import { useGetMeQuery } from "../../features/authApiSlice";
import { selectIsAuthenticated } from "../../features/authSlice";
import { useAppSelector } from "../../hooks/reducer-hook";





export function RequireAdmin(){
    const isAuthenticated= useAppSelector(selectIsAuthenticated);
    const location= useLocation();

      const { data, isLoading, isError } = useGetMeQuery(undefined, {
    skip: !isAuthenticated, // no token at all → don't bother asking, just redirect
  });
  if(!isAuthenticated){
    return <Navigate to="/login" state={{from:location}} replace/>
  }

  if(isLoading){
    return (
<div className="flex min-h-[50vh] items-center justify-center">
        <Loader2 className="size-6 animate-spin text-muted-foreground" />
      </div>
    )
  }
 if (isError || data?.role !== "Admin") {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return <Outlet/>
}