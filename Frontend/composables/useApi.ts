import type { NitroFetchOptions, NitroFetchRequest } from "nitropack";

export const useApi = () => {
  const runtimeConfig = useRuntimeConfig();

  const customFetch = async <T>(
    url: string,
    options?: NitroFetchOptions<NitroFetchRequest>
  ) => {
    return await $fetch<T>(url, {
      baseURL: runtimeConfig.public.apiBaseUrl,
      ...options,
    });
  };
  return { customFetch };
};
