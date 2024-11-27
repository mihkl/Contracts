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
            Upload final contract
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
      <h4 class="text-lg font-bold text-gray-800 dark:text-white mb-3">
        {{ submission.name }}
      </h4>
      <div>
        <p class="truncate mb-3">
          <b>Valid from:</b>
          +
        </p>
        <p class="truncate mb-3">
          <b>Valid until:</b>
        </p>
      </div>
    </UCard>
  </UModal>
</template>
<script setup lang="ts">
const modal = useModal();
const props = defineProps<{ submission: Contract }>();
</script>
