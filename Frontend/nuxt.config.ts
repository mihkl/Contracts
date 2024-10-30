export default defineNuxtConfig({
  compatibilityDate: "2024-04-03",
  devtools: { enabled: true },
  modules: [
    "@nuxt/ui",
    "@nuxtjs/tailwindcss",
    "@pinia/nuxt",
    "@nuxtjs/color-mode",
  ],
  imports: {
    dirs: [
      "Types/*.ts",
      "components/templates/*.vue",
      "components/contracts/*.vue",
    ],
  },
  runtimeConfig: {
    public: {
      apiBaseUrl: "http://localhost:5143/api/",
    },
  },
  colorMode: {
    preference: "system",
    fallback: "light",
    classSuffix: "",
  },
});
