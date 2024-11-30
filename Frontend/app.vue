<template>
  <NuxtLayout>
    <main class="flex-grow p-4 mx-auto max-w-6xl">
      <NuxtPage />
    </main>
    <UModals />
    <UNotifications />
  </NuxtLayout>
</template>

<script setup>
definePageMeta({
  layout: "default",
});
</script>
