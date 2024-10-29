<template>
  <div class="p-2">
    <NuxtLayout />
    <NuxtPage />
    <UModals />
    <UNotifications />
  </div>
</template>

<script setup>
definePageMeta({
  layout: "default",
});
</script>
