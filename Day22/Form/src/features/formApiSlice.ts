import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import type { FormEntry, FormValues } from "../types/formTypes";

export const formApi = createApi({
  reducerPath: "formApi",
  baseQuery: fetchBaseQuery({
    baseUrl: "import.meta.env.VITE_API_BASE_URL",
  }),
  tagTypes: ["FormEntry"],
  endpoints: (builder) => ({
    getFormEntries: builder.query<FormEntry[], void>({
      query: () => "/formentries",
      providesTags: (result) =>
        result
          ? [
              ...result.map(({ id }) => ({ type: "FormEntry" as const, id })),
              { type: "FormEntry", id: "LIST" },
            ]
          : [{ type: "FormEntry", id: "LIST" }],
    }),
    // POST /api/formentries — multipart, because of the CV file
    createFormEntry: builder.mutation<FormEntry, FormValues>({
      query: (data) => {
        const multipart = new FormData();
        multipart.append("Name", data.name);
        multipart.append("Email", data.email);
        multipart.append("Age", String(data.age));
        multipart.append("Address", data.address);
        multipart.append("Description", data.description);
        multipart.append(
          "DateOfBirth",
          data.dateOfBirth ? data.dateOfBirth.toISOString() : "",
        );
        multipart.append("EducationJson", JSON.stringify(data.education));
        if (data.cvFile) {
          multipart.append("CvFile", data.cvFile);
        }
        return {
          url: "/formentries",
          method: "POST",
          body: "multipart",
          // IMPORTANT: do NOT set Content-Type manually here.
          // The browser must set it itself (including the multipart boundary
          // string), which only happens if you let fetch infer it from the
          // FormData object. Explicitly setting "multipart/form-data" yourself
          // omits the boundary and the request silently fails to parse server-side.
        
        };
      }, 
          // After a successful create, invalidate the LIST tag — this tells
      // RTK Query "the list is now stale," which automatically triggers
      // Display's useGetFormEntriesQuery to refetch. No manual dispatch needed.

      invalidatesTags:[{type:"FormEntry",id:"LIST"}]
    }),

    deleteFormEntry:builder.mutation<void, number>({
        query:(id)=>({
            url:`/formentries/${id}`,
            method:"DELETE"
        }),
        invalidatesTags:[{type:"FormEntry",id:"LIST"}]
    }),

    deleteAllEntries:builder.mutation<{message:string},void>({
      query: () => ({
        url: "/formentries/all",
        method: "DELETE",
      }),
      invalidatesTags: [{ type: "FormEntry", id: "LIST" }],
    
    })
  }),
});

export const {
    useCreateFormEntryMutation,
    useGetFormEntriesQuery,
    useDeleteFormEntryMutation,
    useDeleteAllEntriesMutation
}=formApi;