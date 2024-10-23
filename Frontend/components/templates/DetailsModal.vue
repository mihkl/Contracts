<template>
  <UModal prevent-close>
    <UCard :ui="{ ring: '', divide: 'divide-y divide-gray-100 dark:divide-gray-800' }">
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

      <TemplateFieldList :fields="selectedTemplate?.fields" class="h-auto"/>
      <p>{{ templateStore.templates }}</p>



      <template #footer>
        <UButton class="mr-3" @click="">Download</UButton>
      </template>
    </UCard>
  </UModal>
</template>

<script setup lang="ts">
import { useTemplateUploadStore } from '@/stores/TemplateUploadStore';
import { computed, defineProps } from 'vue';
import TemplateFieldList from './TemplateFieldList.vue';


const templateStore = useTemplateUploadStore();
const modal = useModal();


// Accept the selected template ID as a prop
const props = defineProps<{ templateId: number }>();

// Computed property to get the selected template by ID
const selectedTemplate = computed(() => 
  templateStore.templates.find(template => template.id === props.templateId)
);
</script>
