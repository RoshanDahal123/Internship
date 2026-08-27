import {
    fetchBaseQuery,
    type BaseQueryApi,
    type BaseQueryFn,
    type FetchArgs,
    type FetchBaseQueryError,
} from "@reduxjs/toolkit/query/react";


import { clearCredentials, setCredentials } from "../features/authSlice";
import type { RootState } from "../store/store";
import type { AuthResponse } from "../types/authTypes";


const rawBaseQuery= fetchBaseQuery({
   baseUrl: import.meta.env.VITE_API_BASE_URL,
   credentials:"include" ,
   prepareHeaders:(headers,{getState})=>{
    const {accessToken}= (getState() as RootState).auth;
    if(accessToken)headers.set("Authorization", `Bearer ${accessToken}`);
   }
})

// Only these three should NOT trigger a refresh-and-retry on 401 — a 401
// here means "wrong password" / "dead refresh token", not "expired access
// token". Everything else, including /auth/me, is a normal protected
// endpoint and should get the full reauth treatment.
const AUTH_ENDPOINTS_WITHOUT_REAUTH = ["/auth/login", "/auth/register", "/auth/refresh"];

let refreshPromise:Promise<boolean> |null = null;

async function performRefresh(api:BaseQueryApi):Promise<boolean>{
    const refreshResult = await rawBaseQuery({url:"/auth/refresh", method:"POST"},api, {});
    if(refreshResult.data){
        api.dispatch(setCredentials(refreshResult.data as AuthResponse))
        return true;
    }
    return false;
}


export const baseQueryWithReauth:BaseQueryFn<string| FetchArgs, unknown, FetchBaseQueryError>=
async (args,api, extraOptions)=>{
    let result = await rawBaseQuery(args, api, extraOptions);
    if (result.error?.status === 401) {
    const url = typeof args === "string" ? args : args.url;
    const skipReauth = AUTH_ENDPOINTS_WITHOUT_REAUTH.some((path) => url.startsWith(path));

    if(!skipReauth){
        refreshPromise??= performRefresh(api);
        const refreshed = await refreshPromise;
        refreshPromise = null;
    
    if (refreshed) {
        result = await rawBaseQuery(args, api, extraOptions);
      } else {
                api.dispatch(clearCredentials());
      }
    }
}
    return result;
};

