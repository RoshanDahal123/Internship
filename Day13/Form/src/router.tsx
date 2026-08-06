import { createBrowserRouter } from "react-router-dom";
import { AuthLayout } from "./Components/AuthLayout";
import Dashboard from "./Components/Dashboard";
import { ForgotPassoword } from "./Components/ForgotPassword";
import { Login } from "./Components/Login";
import { SignUp } from "./Components/Signup";


export const router= createBrowserRouter([
{
    element:<AuthLayout/>,
    children:[

        {path:"/signup", element:<SignUp/>},
        {path:"/login", element:<Login/>},
          // send any unmatched auth route to login by default
        {path:'/forgot-password',element:<ForgotPassoword/>},
        {path:'/dashboard',element:<Dashboard/>},
        
    ]
}
])