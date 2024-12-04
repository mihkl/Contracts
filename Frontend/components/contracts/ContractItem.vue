<template>
  <div :class="['p-4 border rounded-lg transition border-gray-300']">
    <div class="flex flex-row justify-between items-center">
      <span class="mb-4">{{ contract?.name }}</span>
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
            index <= signingStatuses.indexOf(contract.signingStatus)
              ? 'text-indigo-700'
              : 'text-gray-400', 
            'contract-progress-svg',
          ]"
      />
      </UTooltip>
      <button
        class="px-6 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition ml-auto"
        @click="openDetails(contract.id)"
      >
        Details
      </button>

      <button
        class="px-6 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition"
        @click="deleteContract(contract.id)"
      >
        Delete
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { SigningStatus, type Contract } from "../../Types/Contract.js";
const emits = defineEmits(["openModal", "openDetailsModal"]);
const store = useContractsStore();

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
  contract: Contract;
}>();

const openDetails = async (id: number) => {
  openDetailsModal();
};

const openDetailsModal = () => {
  emits("openDetailsModal", true);
};

const deleteContract = async (id: number) => {
  try {
    await store.deleteContract(id);
    console.log(`Contract with ID ${id} deleted successfully`);
  } catch (error) {
    console.error("Error deleting contract:", error);
  }
};

</script>

<style>
.highlighted {
  color: rgb(79 70 229 / var(--tw-bg-opacity));
}
</style>
