<template>
  <div :class="['p-4 border rounded-lg transition bg-white border-gray-300']">
    <div class="flex flex-row justify-between items-center">
      <span class="mb-4">{{ submission?.name }}</span>
    </div>
    <div class="flex flex-row justify-start items-center gap-3">
      <UTooltip
        :text="contractStatuses[index]"
        v-for="index in [0, 1, 2, 3]"
        :popper="{ placement: 'top' }"
      >
        <SvgoContractProgress
          :class="[
            '!w-20',
            'text-[#C9C9C9]',
            '!h-auto',
            'cursor-pointer',
            index <= signingStatuses.indexOf(submission.signingStatus)
              ? 'highlighted'
              : '',
            'contract-progress-svg',
          ]"
        />
      </UTooltip>
      <button
        class="px-6 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition ml-auto"
        @click="openDetails(submission.id)"
      >
        Details
      </button>
      <button
        class="px-6 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition"
        @click="uploadFinalContract(submission.id)"
      >
        Upload final contract
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { SigningStatus, type Contract } from "../../Types/Contract.js";
const emits = defineEmits([
  "openModal",
  "openDetailsModal",
  "openUploadFinalContractModal",
]);

const contractStatuses = [
  "Link Generated",
  "Contract Signed",
  "Contract Countersigned",
  "Final Contract Sent To Applicant",
];

const signingStatuses: SigningStatus[] = [
  SigningStatus.SignedByNone,
  SigningStatus.SignedByFirstParty,
  SigningStatus.SignedBySecondParty,
  SigningStatus.SignedByAll,
];

defineProps<{
  submission: Contract;
}>();

const openDetails = async (id: number) => {
  openDetailsModal();
};

const uploadFinalContract = async (id: number) => {
  openUploadFinalContractModal();
};

const openDetailsModal = () => {
  emits("openDetailsModal", true);
};

const openUploadFinalContractModal = () => {
  emits("openUploadFinalContractModal", true);
};
</script>

<style>
.highlighted {
  color: rgb(79 70 229 / var(--tw-bg-opacity));
}
</style>
