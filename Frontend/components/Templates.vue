<template>
  <div class="w-full max-w-5xl">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">My Templates</h1>
      <TemplateItem
        v-for="(template, index) in templates"
        :key="index"
        :template="template"
        @open-modal="isOpen = true"
      />

      <UModal v-model="isOpen" prevent-close>
        <UCard
          :ui="{
            ring: '',
            divide: 'divide-y divide-gray-100 dark:divide-gray-800',
          }"
        >
          <template #header>
            <div class="flex items-center justify-between">
              <h3
                class="text-base font-semibold leading-6 text-gray-900 dark:text-white"
              >
                Modal
              </h3>
              <UButton
                color="gray"
                variant="ghost"
                icon="i-heroicons-x-mark-20-solid"
                class="-my-1"
                @click="isOpen = false"
              />
            </div>
          </template>

          <Placeholder class="h-32" />
        </UCard>
      </UModal>
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
const isOpen = ref(false);
</script>

<style scoped>
/* You can add any custom styles if needed */
</style>
