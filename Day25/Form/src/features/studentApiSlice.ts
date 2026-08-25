import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import type {
  PaginatedStudents,
  Student,
  StudentEntry,
} from "../types/formTypes";

export const studentApi = createApi({
  reducerPath: "studentApi",
  baseQuery: fetchBaseQuery({
    baseUrl: import.meta.env.VITE_API_BASE_URL,
  }),
  tagTypes: ["StudentEntry"],
  endpoints: (builder) => ({
    getStudents: builder.infiniteQuery<PaginatedStudents, void, number>({
      infiniteQueryOptions: {
        initialPageParam: 1,
        getNextPageParam: (lastPage) =>
          lastPage.hasNextPage ? lastPage.page + 1 : undefined,
        getPreviousPageParam: (firstPage) =>
          firstPage.page > 1 ? firstPage.page - 1 : undefined,
      },
      query: ({ pageParam }) => `/students?page=${pageParam}&pageSize=10`,
      providesTags: (result) =>
        result
          ? [
              ...result.pages.flatMap((p) =>
                p.items.map((s) => ({
                  type: "StudentEntry" as const,
                  id: s.id,
                })),
              ),
              { type: "StudentEntry", id: "LIST" },
            ]
          : [{ type: "StudentEntry", id: "LIST" }],
    }),

   //genrally infinite query normally means page1->page2(getsadded), so the client has multiple pages added
   //so the traditionally pagination is usually [previous]page1[next] on click on next [previous]page2[next] so to display one page at a time we use this
//     getStudents:builder.query<PaginatedStudents,number>({
//       query:(page) => `/students?page=${page}&pageSize=10`,
//         providesTags: (result) =>
//     result
//       ? [
//           ...result.items.map((s) => ({ type: "StudentEntry" as const, id: s.id })),
//           { type: "StudentEntry", id: "LIST" },
//         ]
//       : [{ type: "StudentEntry", id: "LIST" }],
// }),

    getStudentById: builder.query<StudentEntry, number>({
      query: (id) => `/students/${id}`,
      providesTags: (result, error, id) => [{ type: "StudentEntry", id }],
    }),
    // POST /api/formentries — multipart, because of the CV file

    createStudent: builder.mutation<StudentEntry, Student>({
      query: (data) => {
        console.log(data);
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
        console.log(multipart);
        return {
          url: "/students",
          method: "POST",
          body: multipart,
        };
      },
      invalidatesTags: [{ type: "StudentEntry", id: "LIST" }],
    }),


    updateStudent: builder.mutation<
      StudentEntry,
      { id: number; data: Student }
    >({
      query: ({ id, data }) => {
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

        return { url: `/students/${id}`, method: "PATCH", body: multipart };
      },
      invalidatesTags: (result, error, { id }) => [
        { type: "StudentEntry", id },
        { type: "StudentEntry", id: "LIST" },
      ],
    }),

    
    deleteStudentEntry: builder.mutation<void, number>({
      query: (id) => ({
        url: `/students/${id}`,
        method: "DELETE",
      }),
      invalidatesTags: (result, error, id) => [
        { type: "StudentEntry", id },
        { type: "StudentEntry", id: "LIST" },
      ],
    }),


    deleteAllEntries: builder.mutation<{ message: string }, void>({
      query: () => ({
        url: "/formentries/all",
        method: "DELETE",
      }),
      invalidatesTags: [{ type: "StudentEntry", id: "LIST" }],
    }),
  }),
});

export const {
  useCreateStudentMutation,
  useGetStudentsInfiniteQuery,
//  useGetStudentsQuery,
  useGetStudentByIdQuery,
  useUpdateStudentMutation,
  useDeleteStudentEntryMutation,
  useDeleteAllEntriesMutation,
} = studentApi;
