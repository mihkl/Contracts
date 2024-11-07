<template>
  <div class="mt-10 px-4 w-full max-w-5xl">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">My Templates</h1>

      <div class="flex justify-between mb-4">
        <input
          type="text"
          v-model="filterQuery"
          placeholder="Search by name"
          class="border px-3 py-2 rounded"
        />

        <button @click="toggleSort" class="border px-3 py-2 rounded flex items-center">
          Sort Alphabetically
          <span v-if="sortAlphabetically" class="ml-2">
            <span v-if="sortDirection === 'asc'">⬆️</span>
            <span v-else>⬇️</span>
          </span>
        </button>
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
const sortAlphabetically = ref(false);
const sortDirection = ref("asc");


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

function toggleSort() {
  if (!sortAlphabetically.value) {
    sortAlphabetically.value = true;
    sortDirection.value = "asc";
  } else {
    sortDirection.value = sortDirection.value === "asc" ? "desc" : "asc";
  }
}

const filteredTemplates = computed(() => {
  let filtered = templateStore.templates.filter((template) =>
    template.name.toLowerCase().includes(filterQuery.value.toLowerCase())
  );

  if (sortAlphabetically.value) {
    filtered = filtered.sort((a, b) => {
      if (sortDirection.value === "asc") {
        return a.name.localeCompare(b.name);
      } else {
        return b.name.localeCompare(a.name);
      }
    });
  }

  return filtered;
});
</script>

<style scoped></style>
