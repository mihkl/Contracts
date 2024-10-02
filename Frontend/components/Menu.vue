<template>
    <UHorizontalNavigation
      :links="links"
      class="border-b border-gray-200 dark:border-gray-800 content-center"
    />
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
  