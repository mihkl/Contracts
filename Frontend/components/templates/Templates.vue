<template>
  <div class="mt-10 px-4 w-full max-w-5xl">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">My Templates</h1>
      <TemplateItem
        v-for="(template, index) in templateStore.templates"
        :key="index"
        :template="template"
        @openModal="openModal(template.id)"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import TemplateItem from "./TemplateItem.vue";
import GenerateLinkModal from "./GenerateLinkModal.vue";
import { useTemplateStore } from "~/stores/TemplateStore";

const templateStore = useTemplateStore();
const modal = useModal();

async function fetchTemplates() {
    await templateStore.fetchTemplates();
}

onMounted(() => {
  fetchTemplates();
});

function openModal(templateId: number) {
  modal.open(GenerateLinkModal, {
    templateId: templateId,
  });
}
</script>

<style scoped></style>
