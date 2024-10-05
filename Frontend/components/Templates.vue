<template>
  <div class="w-full max-w-5xl">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">My Templates</h1>
      <TemplateItem
        v-for="(template, index) in templates"
        :key="index"
        :template="template"
      />
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import TemplateItem from "./TemplateItem/TemplateItem.vue";

const templates = ref([
  { name: "Template 1", id: 1 },
  { name: "Template 2", id: 2 },
  { name: "Template 3", id: 3 },
  { name: "Template 4", id: 4 },
]);
</script>

<style scoped>
/* You can add any custom styles if needed */
</style>
