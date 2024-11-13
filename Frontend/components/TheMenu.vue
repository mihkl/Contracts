<template>
  <div class="flex items-center border-b border-gray-200 dark:border-gray-800">
    <div class="flex-shrink-0 mr-6">
      <nuxt-link to="/">
        <img src="public/logo.png" alt="Epic logo" class="h-12 w-auto ml-2" />
      </nuxt-link>
    </div>
    <UHorizontalNavigation :links="links" class="flex-1 content-center" />
    <ColorMode />
  </div>
</template>

<script setup lang="ts">
const route = useRoute();

const links = [
  {
    label: "Home",
    icon: "i-heroicons-home",
    to: "/",
  },
  {
    label: "My Templates",
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
];
</script>
