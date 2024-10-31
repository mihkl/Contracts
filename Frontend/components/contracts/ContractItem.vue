<template>
  <div
    :class="[
      'p-4 border rounded-lg cursor-pointer transition bg-white border-gray-300',
    ]"
  >
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
      <ContractProgressIcon
        class="!w-20 text-[#C9C9C9] !h-auto"
        v-for="index in [1, 2, 3, 4]"
      />
    </div>
  </div>
</template>

<script setup>
import ContractProgressIcon from "~/public/contract_progress.svg";
const emits = defineEmits(["openModal", "openDetailsModal"]);

const props = defineProps({
  contract: {
    name: String,
    id: Number,
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
