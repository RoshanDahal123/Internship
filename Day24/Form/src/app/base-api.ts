
// src/app/base-api.ts

import {
  BaseQueryFn,
  createApi,
} from "@reduxjs/toolkit/query/react";
import {
  AxiosError,
  AxiosRequestConfig,
} from "axios";
import { axiosInstance } from "../lib/axios";



type AxiosBaseQueryArgs = {
  url: string;
  method?: AxiosRequestConfig["method"];
  data?: AxiosRequestConfig["data"];
  params?: AxiosRequestConfig["params"];
  headers?: AxiosRequestConfig["headers"];
};

type AxiosBaseQueryFn = BaseQueryFn<
  AxiosBaseQueryArgs,
  unknown,
  {
    status?: number;
    data?: unknown;
  }
>;

const axiosBaseQuery =
  ({
    baseUrl = "",
  }: {
    baseUrl?: string;
  } = {}): AxiosBaseQueryFn =>
  async ({
    url,
    method,
    data,
    params,
    headers,
  }) => {
    try {
      const isFormData= data instanceof FormData;
      const result = await axiosInstance({
        url: `${baseUrl}${url}`,
        method,
        data,
        params,
        headers: {
          ...headers,
          // Let the browser set multipart/form-data with the correct
          // boundary itself — don't let the JSON default leak through.
          ...(isFormData ? { "Content-Type": undefined } : {}),
        },

        // Cookies are automatically included.
        withCredentials: true,
      });

      return {
        data: result.data,
      };
    } catch (axiosError) {
      const error = axiosError as AxiosError;

      return {
        error: {
          status: error.response?.status,
          data: error.response?.data ?? error.message,
        },
      };
    }
  };

export const baseApi = createApi({
  reducerPath: "api",

  baseQuery: axiosBaseQuery(),

  tagTypes: [
    "Auth",
    "Students",
    "StudentEntry"
  ],

  endpoints: () => ({}),
});

