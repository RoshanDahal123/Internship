import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { buildErrorMessage } from "vite";
type Todo = {
  id: string;
  userId: number;
  title: string;
  completed: boolean;
};

export const apiSlice= createApi({
    reducerPath:'api',
    baseQuery:fetchBaseQuery({baseUrl:"http://localhost:3500"}),
    tagTypes:['Todos'],
    endpoints:(builder)=>({
        getTodos:builder.query<Todo[],void>({
            query:()=>'/todos',
            providesTags:['Todos'],
          
            
      }),
        addTodo:builder.mutation<Todo, Omit<Todo, "id">>({
            query:(todo)=>({
                url:'/todos',
                method:"POST",
                body:todo
            }),
             invalidatesTags:["Todos"]
        }),
        updateTodo:builder.mutation<Todo,Todo>({
            query:(todo)=>({
                url:`/todos/${todo.id}`,
                method:"PATCH",
                body:todo
            }),
            invalidatesTags:['Todos']
        }),
        deleteTodo:builder.mutation<Todo,{id:string}>({
            query:({id})=>({
                url:`/todos/${id}`,
                method:"DELETE",
                body:{id}
            }),
            invalidatesTags:["Todos"]
        })
    })
})



export const {useGetTodosQuery,useAddTodoMutation,useUpdateTodoMutation,useDeleteTodoMutation}= apiSlice;



