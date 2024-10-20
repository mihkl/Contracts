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
            Details
          </h3>
          <UButton
            color="gray"
            variant="ghost"
            icon="i-heroicons-x-mark-20-solid"
            class="-my-1"
            @click="modal.close()"
          />
        </div>
      </template>


      <div class="space-y-4 p-4">
        <p><strong>Template Name:</strong> {{ template.name }}</p>
        <p><strong>Template ID:</strong> {{ template.id }}</p>

        <div v-if="template.fields && template.fields.length > 0">
          <h3 class="font-semibold">Fields:</h3>
          <ul class="list-disc ml-5">
            <li v-for="(field, index) in template.fields" :key="index">
              {{ field.name }}: {{ field.type }}
            </li>
          </ul>
        </div>
        <div v-else>
          <p>No fields available.</p>
        </div>


        <UButton type="button" @click="downloadTemplate">Download file</UButton>
      </div>
    </UCard>
  </UModal>
</template>

<script setup lang="ts">
import { useTemplateUploadStore } from '@/stores/TemplateUploadStore';
import { ref } from 'vue';

// Using store to retrieve template data (like your working example)
const { serverResponse } = useTemplateUploadStore();
const template = ref(serverResponse?.template || { name: 'hi', id: 0, fields: [] });
const modal = useModal()



// Download button logic
const downloadTemplate = () => {
  console.log(`Downloading template: ${template.value.name}`);
  // Implement your actual download logic here
};
</script>

<style scoped>
/* Add any additional styling here */
</style>
