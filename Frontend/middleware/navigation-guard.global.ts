import { useAuth } from "~/composables/useAuth";

export default defineNuxtRouteMiddleware((to) => {
  const auth = useAuth();

  const excludedPathsRegexes = [
    /^\/register$/,
    /^\/login$/,
    /^\/contracts\/\d+$/,
  ];

  if (
    !auth.isAuthenticated.value &&
    !excludedPathsRegexes.some((path) => path.test(to.path))
  ) {
    return navigateTo("/login");
  }
});
