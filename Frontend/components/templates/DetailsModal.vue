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
          <h3
            class="text-base font-semibold leading-6 text-gray-900 dark:text-white"
          >
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
      <h4 class="text-lg font-bold text-gray-800 dark:text-white">
        {{ selectedTemplate?.name }}
      </h4>
      <TemplateFieldList
        :fields="selectedTemplate?.fields"
        class="h-auto mt-5"
      />

      <template #footer>
        <UButton class="mr-3" @click="downloadTemplate">Download</UButton>
      </template>
    </UCard>
  </UModal>
</template>

<script setup lang="ts">
import { useTemplateStore } from "@/stores/TemplateStore";
import { computed, defineProps } from "vue";
import TemplateFieldList from "./TemplateFieldList.vue";

const templateStore = useTemplateStore();
const modal = useModal();

const props = defineProps<{ templateId: number }>();

const selectedTemplate = computed(() =>
  templateStore.templates.find((template) => template.id === props.templateId)
);

const downloadTemplate = () => {
  if (selectedTemplate.value) {
    templateStore.downloadFile(
      selectedTemplate.value.id,
      `${selectedTemplate.value.name}.docx`
    );
  } else {
    console.error("No template selected");
  }
};
</script>
