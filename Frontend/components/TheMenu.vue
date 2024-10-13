<template>
  <div class="flex items-center border-b border-gray-200 dark:border-gray-800">
    <div class="flex-shrink-0 mr-6">
      <nuxt-link to="/">
        <img
          src="public/logo.png"
          alt="Epic logo"
          class="h-12 w-auto ml-2"   
        />
      </nuxt-link>
    </div>
    <UHorizontalNavigation
      :links="links"
      class="flex-1 content-center"
    />
  </div>
</template>

<script setup lang="ts">
const route = useRoute();

const links = [
  {
    label: "Home",
    to: "/",
  },
  {
    label: "My Templates",
    to: "/templates",
  },
  {
    label: "Contracts",
    to: "/contracts",
  },
];
</script>
