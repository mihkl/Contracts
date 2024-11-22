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
          <b>Valid from:</b>
          {{ formatDateToString(new Date(selectedContract!.linkValidFrom)) }}
        </p>
        <p class="truncate mb-3">
          <b>Valid until:</b>
          {{ formatDateToString(new Date(selectedContract!.linkValidUntil)) }}
        </p>
        <UButton class="mr-3" @click="copyLink">Copy link</UButton>
        <UButton v-if="canOpenDetails" @click="openContract">Open PDF</UButton>
      </div>
    </UCard>
  </UModal>
</template>

<script setup lang="ts">
import { useContractsStore } from "@/stores/ContractsStore";
import { computed, defineProps } from "vue";
import { SigningStatus } from "~/Types/Contract";

const contractStore = useContractsStore();
const modal = useModal();
const toast = useToast();

const props = defineProps<{ contractId: number }>();

const selectedContract = computed(() =>
  contractStore.contracts.find(
    (contract) => contract.id === props.contractId
  )
);  

const canOpenDetails = computed(() => {
  return selectedContract.value?.signingStatus !== SigningStatus.SignedByNone;
});

const copyLink = () => {
  navigator.clipboard.writeText(
    window.location.origin + "/" + selectedContract.value?.url
  );
  toast.add({
    title: "Success!",
    description: "The link has been copied to your clipboard.",
  });
};

const openContract = () => {
  if (selectedContract.value) {
    contractStore.fetchPDF(selectedContract.value.id);
  } else {
    console.error("No contract selected");
  }
};

function formatDateToString(date: Date) {
  const mm = String(date.getMonth() + 1).padStart(2, "0");
  const dd = String(date.getDate()).padStart(2, "0");
  const yyyy = date.getFullYear();

  if (yyyy === 1) return "Not set";
  return `${mm}.${dd}.${yyyy}`;
}
</script>
