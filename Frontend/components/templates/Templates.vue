<template>
  <div class="mt-10 w-full">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">My Templates</h1>

      <div
        v-if="!(filteredTemplates.length === 0 && filterQuery === '')"
        class="flex justify-between mb-4"
      >
        <input
          type="text"
          v-model="filterQuery"
          placeholder="Search by name/keywords"
          class="border px-3 py-2 rounded"
        />

        <button
          @click="openSortOptionsModal"
          class="border px-3 py-2 rounded flex items-center"
        >
          Sort Options
        </button>
      </div>

      <div
        v-if="filteredTemplates.length === 0"
        class="text-center text-gray-500"
      >
        No Templates Available
      </div>
      <TemplateItem
        v-else
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
import SortOptionsModal from "./SortOptionsModal.vue";
import { ref, watch } from "vue";
import debounce from "lodash/debounce";

const templateStore = useTemplateStore();
const modal = useModal();
const filterQuery = ref("");
const showSortOptionsModal = ref(false);
const sortType = ref("alphabetical");
const sortOrder = ref("asc");

async function fetchTemplates(query: string) {
  await templateStore.fetchTemplates(query);
}

onMounted(() => {
  fetchTemplates(filterQuery.value);
});

const debouncedFetchTemplates = debounce(fetchTemplates, 300);

watch(filterQuery, (newQuery) => {
  debouncedFetchTemplates(newQuery);
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

function openSortOptionsModal() {
  modal.open(SortOptionsModal, {
    "onApply-sort": applySortOptions,
    sortType: sortType.value,
    sortOrder: sortOrder.value,
  });
}

function applySortOptions({ type, order }: { type: string; order: string }) {
  sortType.value = type;
  sortOrder.value = order;
}

const filteredTemplates = computed(() => {
  let filtered = templateStore.templates;

  filtered = filtered.sort((a, b) => {
    if (sortType.value === "alphabetical") {
      return sortOrder.value === "asc"
        ? a.name.localeCompare(b.name)
        : b.name.localeCompare(a.name);
    } else if (sortType.value === "creationTime") {
      return sortOrder.value === "asc"
        ? new Date(a.creationTime).getTime() -
            new Date(b.creationTime).getTime()
        : new Date(b.creationTime).getTime() -
            new Date(a.creationTime).getTime();
    }
    return 0;
  });

  return filtered;
});
</script>

<style scoped></style>
