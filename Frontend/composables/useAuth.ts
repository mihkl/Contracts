import type { NitroFetchOptions, NitroFetchRequest } from "nitropack";
import type { User } from "~/Types/User";

export const useAuth = () => {
  const activeToken = useState<string | undefined>("token", () => undefined);

  const api = useApi();

  const isAuthenticated = computed(() => !!activeToken.value);

  const register = async (user: User) => {
    const response = await api.fetchWithErrorHandling<{ token: string }>(
      "register",
      {
        method: "POST",
        body: user,
      }
    );

    if (!response.error) {
      console.log("Registered");
      return true;
    }

    return false;
  };

  const logIn = async (user: User) => {
    const response = await api.fetchWithErrorHandling<{ token: string }>(
      "login",
      {
        method: "POST",
        body: user,
      }
    );
    console.log(response);
    if (!response.error) {
      activeToken.value = response.accessToken;
      console.log("Logged in with token: " + activeToken.value);
      return true;
    }

    return false;
  };

  const logOut = async () => {
    activeToken.value = undefined;
    location.replace("/login");
    await nextTick();
  };

  const fetchWithToken = async <T>(
    url: string,
    options?: NitroFetchOptions<NitroFetchRequest>
  ) => {
    return await api.fetchWithErrorHandling<T>(url, {
      ...options,
      headers: {
        Authorization: "Bearer " + activeToken.value,
        ...options?.headers,
      },
    });
  };

  return { register, logIn, logOut, isAuthenticated, fetchWithToken };
};
