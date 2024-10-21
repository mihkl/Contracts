import type { NitroFetchOptions, NitroFetchRequest } from "nitropack";

export const useApi = () => {
  const runtimeConfig = useRuntimeConfig();
  const toast = useToast();

  const customFetch = async <T>(
    url: string,
    options?: NitroFetchOptions<NitroFetchRequest>
  ) => {
    return await $fetch<T>(url, {
      baseURL: runtimeConfig.public.apiBaseUrl,
      ...options,
      ignoreResponseError: true,
    }).catch((err) => err);
  };

  const fetchWithErrorHandling = async <T>(
    url: string,
    options?: NitroFetchOptions<NitroFetchRequest>
  ) => {
    const response = await customFetch<T>(url, options);

    if (!response) {
      toast.add({
        title: "ERROR",
        description: "No response from server",
      });
    }
    if (typeof response === "string") {
      toast.add({ title: "ERROR", description: response });
    } else {
      return response;
    }
  };

  return { customFetch, fetchWithErrorHandling };
};
