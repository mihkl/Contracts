import { useAuth } from "~/composables/useAuth";

export default defineNuxtRouteMiddleware((to, from) => {
  const auth = useAuth();

  if ((to.path == "/contracts" || to.path == "/templates") && !auth.isAuthenticated.value) {
    return navigateTo("/login");
  }
});
