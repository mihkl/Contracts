<template>
  <div class="flex items-center border-b border-gray-200 dark:border-gray-800">
    <div class="flex-shrink-0 mr-6">
      <nuxt-link to="/">
        <img src="public/logo.png" alt="Epic logo" class="h-12 w-auto ml-4" />
      </nuxt-link>
    </div>
    <UHorizontalNavigation :links="links" class="flex-1 content-center"/>
    <ColorMode />
    <UButton
      v-if="auth.isAuthenticated.value"
      @click="auth.logOut"
      icon="i-heroicons-arrow-right-start-on-rectangle"
      class="mr-6 my-0 mx-2 bg-indigo-500 hover:bg-indigo-600 mr-4"
    />
  </div>
</template>

<script setup lang="ts">

const auth = useAuth();

const links = [
  {
    label: "Home",
    icon: "i-heroicons-home",
    to: "/",
  },
  {
    label: "Templates",
    icon: "i-heroicons-queue-list",
    to: "/templates", 
  },
  {
    label: "Contracts",
    icon: "i-heroicons-document-text",
    to: "/contracts",
  },
  {
    label: "Submissions",
    icon: "i-heroicons-paper-airplane",
    to: "/submissions",
  },
  {
    label: "Integrations",
    icon: "i-heroicons-link",
    to: "/integrations",
  },
  {
    label: "Get Started",
    icon: "i-heroicons-hand-raised",
    to: "/getStarted",
  },
];

</script>
