import { useAuth } from "~/composables/useAuth";

export default defineNuxtRouteMiddleware((to) => {
  const auth = useAuth();

  const excludedToPaths = ["/register", "/login", "/contracts"];
  if (!auth.isAuthenticated.value && !excludedToPaths.includes(to.path)) {
    return navigateTo("/login");
  }
});
