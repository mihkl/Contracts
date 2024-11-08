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

        <div class="flex space-x-2">
          <!-- Button for Alphabetical Sort -->
          <button @click="toggleAlphabeticalSort" class="border px-3 py-2 rounded flex items-center">
            Sort Alphabetically
            <span v-if="sortAlphabetically" class="ml-2">
              <span v-if="sortDirectionAlphabetical === 'asc'">⬆️</span>
              <span v-else>⬇️</span>
            </span>
          </button>

          <!-- Button for Creation Time Sort -->
          <button @click="toggleCreationTimeSort" class="border px-3 py-2 rounded flex items-center">
            Sort by Creation Time
            <span v-if="sortByCreationTime" class="ml-2">
              <span v-if="sortDirectionCreation === 'asc'">⬆️</span>
              <span v-else>⬇️</span>
            </span>
          </button>
        </div>
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
const sortDirectionAlphabetical = ref("asc");
const sortByCreationTime = ref(false);
const sortDirectionCreation = ref("asc");

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

function toggleAlphabeticalSort() {
  sortAlphabetically.value = !sortAlphabetically.value;
  sortByCreationTime.value = false; // Disable creation time sort when alphabetical sort is active
  sortDirectionAlphabetical.value = sortDirectionAlphabetical.value === "asc" ? "desc" : "asc";
}

function toggleCreationTimeSort() {
  sortByCreationTime.value = !sortByCreationTime.value;
  sortAlphabetically.value = false; // Disable alphabetical sort when creation time sort is active
  sortDirectionCreation.value = sortDirectionCreation.value === "asc" ? "desc" : "asc";
}

const filteredTemplates = computed(() => {
  let filtered = templateStore.templates.filter((template) =>
    template.name.toLowerCase().includes(filterQuery.value.toLowerCase())
  );

  if (sortAlphabetically.value) {
    filtered = filtered.sort((a, b) => {
      return sortDirectionAlphabetical.value === "asc"
        ? a.name.localeCompare(b.name)
        : b.name.localeCompare(a.name);
    });
  } else if (sortByCreationTime.value) {
    filtered = filtered.sort((a, b) => {
      return sortDirectionCreation.value === "asc"
        ? new Date(a.creationTime).getTime() - new Date(b.creationTime).getTime()
        : new Date(b.creationTime).getTime() - new Date(a.creationTime).getTime();
    });
  }

  return filtered;
});
</script>

<style scoped></style>
