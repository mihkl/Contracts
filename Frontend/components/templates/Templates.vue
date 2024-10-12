<template>
  <div class="mt-10 px-4 w-full max-w-5xl">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">My Templates</h1>
      <TemplateItem
        v-for="(template, index) in templateStore.templates"
        :key="index"
        :template="template"
        @openModal="openModal"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import TemplateItem from "./TemplateItem.vue";
import GenerateLinkModal from "./GenerateLinkModal.vue";
import { useTemplateUploadStore } from "@/stores/TemplateUploadStore";

const templateStore = useTemplateUploadStore();
const modal = useModal();

async function fetchTemplates() {
  try {
    await templateStore.fetchTemplates(); 
  } catch (error) {
    console.error("Error fetching templates:", error);
  }
}

onMounted(() => {
  fetchTemplates();
});

function openModal() {
  modal.open(GenerateLinkModal);
}
</script>

<style scoped>
/* You can add any custom styles if needed */
</style>
