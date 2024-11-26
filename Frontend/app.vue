<template>
  <NuxtLayout>
    <main class="flex-grow p-4 mx-auto max-w-4xl">
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
