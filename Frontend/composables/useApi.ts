import type { NitroFetchOptions, NitroFetchRequest } from "nitropack";

export const useApi = () => {
  const errorCodes: number[] = [
    400, 401, 403, 404, 409, 500, 502, 503, 504, 599,
  ];
  const runtimeConfig = useRuntimeConfig();
  const toast = useToast();

  const customFetch = async <T>(
    url: string,
    options?: NitroFetchOptions<NitroFetchRequest>
  ) => {
    return await $fetch
      .raw<T>(url, {
        baseURL: runtimeConfig.public.apiBaseUrl,
        ...options,
        ignoreResponseError: true,
      })
      .catch((err) => err.data);
  };

  const fetchWithErrorHandling = async <T>(
    url: string,
    options?: NitroFetchOptions<NitroFetchRequest>
  ) => {
    const response = await customFetch<T>(url, options);

    if (errorCodes.includes(response.status)) {
      toast.add({ title: response.statusText, description: response._data });
    } else {
      return response._data;
    }
  };

  return { customFetch, fetchWithErrorHandling };
};
