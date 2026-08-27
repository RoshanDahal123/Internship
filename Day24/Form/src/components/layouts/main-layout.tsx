
import { Outlet } from "react-router";
import { Navbar } from "./navbar";

export function MainLayout() {
  return (
    <div className="flex min-h-screen flex-col bg-background">
      <Navbar />
      <main className="flex-1">
        <Outlet />
      </main>
    </div>
  );
}