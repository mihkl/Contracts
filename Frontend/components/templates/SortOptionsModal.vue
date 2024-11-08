<template>
    <UModal prevent-close>
      <UCard
        :ui="{
          ring: '',
          divide: 'divide-y divide-gray-100 dark:divide-gray-800',
        }"
      >
        <template #header>
          <div class="flex items-center justify-between">
            <h3 class="text-base font-semibold leading-6 text-gray-900 dark:text-white">
              Sort Options
            </h3>
            <UButton
              color="gray"
              variant="ghost"
              icon="i-heroicons-x-mark-20-solid"
              class="-my-1"
              @click="closeModal"
            />
          </div>
        </template>
  
        <div class="p-4">
          <h4 class="text-lg font-bold text-gray-800 dark:text-white mb-4">
            Select Sort Type
          </h4>
          <div class="mb-4">
            <label class="flex items-center mb-2">
              <input
                type="radio"
                value="alphabetical"
                v-model="selectedSortType"
                class="mr-2"
              />
              Sort Alphabetically
            </label>
            <label class="flex items-center">
              <input
                type="radio"
                value="creationTime"
                v-model="selectedSortType"
                class="mr-2"
              />
              Sort by Date
            </label>
          </div>
  
          <h4 class="text-lg font-bold text-gray-800 dark:text-white mb-4">
            Select Sort Order
          </h4>
          <div>
            <label class="flex items-center mb-2">
              <input
                type="radio"
                value="asc"
                v-model="selectedSortOrder"
                class="mr-2"
              />
              Ascending
            </label>
            <label class="flex items-center">
              <input
                type="radio"
                value="desc"
                v-model="selectedSortOrder"
                class="mr-2"
              />
              Descending
            </label>
          </div>
        </div>
  
        <template #footer>
          <UButton class="mr-3" @click="applySort">Apply</UButton>
        </template>
      </UCard>
    </UModal>
  </template>
  
  <script setup lang="ts">
  import { ref, defineEmits } from 'vue';
  
  const modal = useModal();
  const emit = defineEmits(["apply-sort"]);
  
  const selectedSortType = ref("alphabetical");
  const selectedSortOrder = ref("asc");
  
  function applySort() {
    emit("apply-sort", { type: selectedSortType.value, order: selectedSortOrder.value });
    closeModal();
  }
  
  function closeModal() {
    modal.close();
  }
  </script>
  