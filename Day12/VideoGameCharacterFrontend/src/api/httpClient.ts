//single place to configure base Url + error handling
const BASE_URL= import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5279';

export class ApiError extends Error{
    constructor(public status:number, message:string){
        super(message);
        this.name='ApiError';
    }
}

async function request<T>(path:string, options:RequestInit={}) :Promise<T>{
    const response = await fetch(`${BASE_URL}${path}`,{
        headers:{'Content-Type':'application/json',...options.headers},
        ...options,
    })
    if(!response.ok){
        // Your API returns plain-text NotFound messages, so try text first

        const message= await response.text().catch(()=> response.statusText);
        throw new ApiError(response.status,message||`Request failed with ${response.status}`)
    }

     if (response.status === 204) {
    return undefined as T;
  }

  return response.json() as Promise<T>;

}

export const httpClient={
    get:<T>(path:string)=> request<T>(path),
    post:<T>(path:string,body:unknown)=>
        request<T>(path,{method:'POST',body:JSON.stringify(body)}),
    put:<T>(path:string, body:unknown)=>
        request<T>(path,{method:'PUT',body:JSON.stringify(body)}),
    delete: <T>(path: string) => request<T>(path, { method: 'DELETE' }),
}

