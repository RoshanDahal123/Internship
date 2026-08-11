import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";


const DOGS_API_KEY='RGVLYpp0e8XcAepb80Pp1dy1m4QbL6hgMjbgz9bY'


interface Breed{
    id:string,
    name:string,
    image_link:string
}

export const apiSlice= createApi({
        reducerPath:'api',
        baseQuery:fetchBaseQuery({
            baseUrl:'https://api.api-ninjas.com/v1',
            prepareHeaders(headers){
                headers.set('x-api-key',DOGS_API_KEY);
                return headers;
            }
        }),
        endpoints(builder){
            return {
                fetchBreeds:builder.query<Breed[],string | void>({
                    query(breedName){
                        //@ts-ignore
                        return`/dogs?name=${encodeURIComponent(breedName)}`;
                    }
                })
            }
        }
    })

    export const {useFetchBreedsQuery }=apiSlice;