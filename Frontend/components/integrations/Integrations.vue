<template>
  <div class="mt-10 px-4 w-full max-w-5xl">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">Integrations</h1>
      <EmailIntegrationItem />
    </div>
  </div>
</template>

<script setup lang="ts">
import EmailIntegrationItem from "./EmailIntegrationItem.vue";
</script>
