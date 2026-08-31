import { GraduationCap, LogOut, UserPlus, UserRound } from "lucide-react";
import { useNavigate } from "react-router";
import { useLogoutMutation } from "../../features/authApiSlice";
import {
  selectAuthEmail,
  selectIsAdmin,
  selectIsAuthenticated
} from "../../features/authSlice";
import { useAppSelector } from "../../hooks/reducer-hook";
import { Avatar, AvatarFallback } from "../ui/avatar";
import { Badge } from "../ui/badge";
import { Button } from "../ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "../ui/dropdown-menu";

function initialsFor(email: string | null): string {
  return email ? email.slice(0, 2).toUpperCase() : "?";
}

export function Navbar() {
  const navigate = useNavigate();

  const isAuthenticated = useAppSelector(selectIsAuthenticated);
  const isAdmin = useAppSelector(selectIsAdmin);
  const email = useAppSelector(selectAuthEmail);

  const [logout] = useLogoutMutation();

  async function handleLogout() {
   await logout();
  navigate("/");
  }

  return (
    <header className="sticky top-0 z-40 w-full border-b bg-background/95 backdrop-blur supports-backdrop-blur:bg-background/60">
      <div className="mx-auto flex h-14 max-w-6xl items-center gap-4 px-4 sm:px-6">
        <button
          onClick={() => navigate("/")}
          className="flex shrink-0 items-center gap-2 font-semibold tracking-tight"
        >
          <GraduationCap className="size-5 text-primary" />
          StudentDir
        </button>

        <div className="ml-auto flex items-center gap-2">
          {isAuthenticated ? (
            <DropdownMenu>
              <DropdownMenuTrigger className="inline-flex items-center gap-2 rounded-md px-2 py-1.5 hover:bg-accent text-sm font-medium">
                <Avatar className="size-7">
                  <AvatarFallback className="text-xs">
                    {initialsFor(email)}
                  </AvatarFallback>
                </Avatar>
                <span className="hidden max-w-35 truncate text-sm font-medium sm:inline">
                  {email}
                </span>
                {isAdmin && (
                  <Badge variant="secondary" className="hidden sm:inline-flex">
                    Admin
                  </Badge>
                )}
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end" className="w-56">
                <DropdownMenuGroup>
                  <DropdownMenuLabel className="flex flex-col gap-0.5">
                    <span className="truncate text-sm font-medium">
                      {email}
                    </span>
                    <span className="text-xs font-normal text-muted-foreground">
                      {isAdmin ? "Administrator" : "Member"}
                    </span>
                  </DropdownMenuLabel>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem variant="destructive" onClick={handleLogout}>
                    <LogOut className="size-4" />
                    Log out
                  </DropdownMenuItem>
                </DropdownMenuGroup>
              </DropdownMenuContent>
            </DropdownMenu>
          ) : (
            <>
              <Button variant="ghost" size="sm" onClick={() => navigate("/login")}>
                <UserRound className="size-4" />
                Login
              </Button>
              <Button size="sm" onClick={() => navigate("/register")}>
                <UserPlus className="size-4" />
                Register
              </Button>
            </>
          )}
        </div>
      </div>
    </header>
  );
}