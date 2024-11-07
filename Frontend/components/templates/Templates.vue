<template>
  <div class="mt-10 px-4 w-full max-w-5xl">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">My Templates</h1>

      <div class="flex justify-between mb-4">
        <input
          type="text"
          v-model="filterQuery"
          placeholder="Filter by name"
          class="border px-3 py-2 rounded"
        />
      </div>

      <TemplateItem
        v-for="(template, index) in filteredTemplates"
        :key="index"
        :template="template"
        @openModal="openModal(template.id)"
        @openDetailsModal="openDetailsModal(template.id)"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import TemplateItem from "./TemplateItem.vue";
import GenerateLinkModal from "./GenerateLinkModal.vue";
import { useTemplateStore } from "~/stores/TemplateStore";
import DetailsModal from "./DetailsModal.vue";

const templateStore = useTemplateStore();
const modal = useModal();
const filterQuery = ref("");


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

function openDetailsModal(templateId: number) {
  modal.open(DetailsModal, {
    templateId: templateId,
  });
}

const filteredTemplates = computed(() => {
  // Filter templates by name
  let filtered = templateStore.templates.filter((template) =>
    template.name.toLowerCase().includes(filterQuery.value.toLowerCase())
  );

  // Sort templates alphabetically by name
  filtered = filtered.sort((a, b) => a.name.localeCompare(b.name));

  return filtered;
});
</script>

<style scoped></style>
