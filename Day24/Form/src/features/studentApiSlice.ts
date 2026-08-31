import type {
  PaginatedStudents,
  Student,
  StudentEntry,
} from "../types/formTypes";
import { baseApi } from "../app/base-api";

export const studentApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getStudents: builder.query<
      PaginatedStudents,
      { page: number; search?: string }
    >({
      query: ({ page, search }) => {
        const params = new URLSearchParams({
          page: String(page),
          pageSize: "10",
        });
        if (search?.trim()) params.set("search", search.trim());
        return { url: `/students?${params.toString()}` };
      },
      providesTags: (result) =>
        result
          ? [
              ...result.items.map((s) => ({
                type: "StudentEntry" as const,
                id: s.id,
              })),
              { type: "StudentEntry", id: "LIST" },
            ]
          : [{ type: "StudentEntry", id: "LIST" }],
    }),

    getStudentById: builder.query<StudentEntry, number>({
      query: (id) => ({ url: `/students/${id}` }),
      providesTags: (result, error, id) => [{ type: "StudentEntry", id }],
    }),

    createStudent: builder.mutation<StudentEntry, Student>({
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
          url: "/students",
          method: "POST",
          data: multipart,
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
        return { url: `/students/${id}`, method: "PATCH", data: multipart };
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
  overrideExisting: false,
});

export const {
  useCreateStudentMutation,
  useGetStudentsQuery,
  useGetStudentByIdQuery,
  useUpdateStudentMutation,
  useDeleteStudentEntryMutation,
  useDeleteAllEntriesMutation,
} = studentApi;