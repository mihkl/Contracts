<template>
  <div :class="['p-4 border rounded-lg transition bg-white border-gray-300']">
    <div class="flex flex-row justify-between items-center">
      <span>{{ contract?.name }}</span>
      <div class="flex space-x-2 mb-2">
        <button
          class="px-6 py-2 bg-purple-500 text-white rounded-lg hover:bg-purple-600 transition"
          @click="openDetails(template.id)"
        >
          Details
        </button>
      </div>
    </div>
    <div class="flex flex-row justify-start items-center gap-3">
      <UTooltip
        :text="contractStatuses[index]"
        v-for="index in [0, 1, 2, 3]"
        :popper="{ placement: 'top' }"
      >
        <ContractProgressIcon class="!w-20 text-[#C9C9C9] !h-auto" />
      </UTooltip>
    </div>
  </div>
</template>

<script setup>
import ContractProgressIcon from "~/public/contract_progress.svg";
const emits = defineEmits(["openModal", "openDetailsModal"]);

const contractStatuses = [
  "Link Generated",
  "Contract Signed",
  "Contract Countersigned",
  "Final Contract Sent To Applicant",
];

const props = defineProps({
  contract: {
    name: String,
    id: Number,
    contractStatus: "LinkGenerated",
  },
});

const openDetails = async (id) => {
  openDetailsModal();
};

const openModal = () => {
  emits("openModal", true);
};

const openDetailsModal = () => {
  emits("openDetailsModal", true);
};
</script>
