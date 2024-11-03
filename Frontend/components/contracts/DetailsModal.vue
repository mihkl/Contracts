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
      <h4 class="text-lg font-bold text-gray-800 dark:text-white mb-3">
        {{ selectedContract?.name }}
      </h4>
      <div>
        <p class="truncate mb-3">
          <b>Valid from:</b> {{ new Date(selectedContract!.linkValidFrom) }}
        </p>
        <p class="truncate mb-3">
          <b>Valid until:</b> {{ new Date(selectedContract!.linkValidFrom) }}
        </p>
        <UButton class="mr-3" @click="copyLink">Copy link</UButton>
      </div>
    </UCard>
  </UModal>
</template>

<script setup lang="ts">
import { useContractsStore } from "@/stores/ContractsStore";
import { computed, defineProps } from "vue";

const contractStore = useContractsStore();
const modal = useModal();

const props = defineProps<{ contractId: number }>();

const selectedContract = computed(() =>
  contractStore.contracts.find(
    (contract: Contract) => contract.id === props.contractId
  )
);

const copyLink = () => {
  navigator.clipboard.writeText(
    window.location.origin + "/" + selectedContract.value?.url
  );
};
</script>
