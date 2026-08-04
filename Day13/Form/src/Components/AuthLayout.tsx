import { Toaster } from "react-hot-toast";
import { Outlet } from 'react-router-dom';


export function AuthLayout(){
    return (
        <div className="relative flex min-h-screen w-full items-center justify-center overflow-hidden bg-[#26213c]  p-4">
         {/* Outlet renders whichever child route matched — Signup or Login */}
       <Outlet/>
       <Toaster position="top-right"/>

        </div>
    )
}