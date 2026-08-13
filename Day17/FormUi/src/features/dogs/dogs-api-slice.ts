import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";


const DOGS_API_KEY='RGVLYpp0e8XcAepb80Pp1dy1m4QbL6hgMjbgz9bY'


interface Breed{
    id:string,
    name:string,
    image_link:string
}
// what the form sends — no `id`, the server assigns that
type NewBreedPayload = Omit<Breed, 'id'>;

export const apiSlice= createApi({
        reducerPath:'api',
        baseQuery:fetchBaseQuery({
            baseUrl:'https://api.api-ninjas.com/v1',
            prepareHeaders(headers){
                headers.set('x-api-key',DOGS_API_KEY);
                return headers;
            }
        }),
        tagTypes: ['Breed'], // enables cache invalidation below
        endpoints(builder){
            return {
                fetchBreeds:builder.query<Breed[],string | void>({
                    query(breedName){
                        //@ts-ignore
                        return`/dogs?name=${encodeURIComponent(breedName)}`;
                    },
                    providesTags: ['Breed'],
                }),
                //New: the post mutation
                addBread:builder.mutation<Breed, NewBreedPayload>({
                    query(newBreed){
                        return{
                            url:'/dogs',
                            method:'POST',
                            body:newBreed
                        }
                    },
                    //after a successful POST, tell RTK query t breed list is statle
                    //so it automatically refetcheds, no manual refresh needed 
                    invalidatesTags:['Breed']
                })
            }
        }
    })

    
    export const {useFetchBreedsQuery ,useAddBreadMutation}=apiSlice;