<template>
  <div class="w-full max-w-5xl">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">My Templates</h1>
      <TemplateItem
        v-for="(template, index) in templates"
        :key="index"
        :template="template"
        @open-modal="openModal"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import TemplateItem from "./TemplateItem.vue";
import GenerateLinkModal from "./GenerateLinkModal.vue";

const templates = ref([
  { name: "Template 1", id: 1 },
  { name: "Template 2", id: 2 },
  { name: "Template 3", id: 3 },
  { name: "Template 4", id: 4 },
]);

const modal = useModal();

function openModal() {
  modal.open(GenerateLinkModal);
}
</script>

<style scoped>
/* You can add any custom styles if needed */
</style>
