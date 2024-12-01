<template>
  <div :class="['p-4 border rounded-lg transition border-gray-300']">
    <div class="flex flex-row justify-between items-center">
      <span>{{ contract?.name }}</span>
      <UButton 
        class="px-6 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition ml-auto dark:bg-indigo-600 dark:hover:bg-indigo-700"
        @click="downloadSignedContract"
        color="indigo"
        >
          Download
      </UButton>

    </div>
  </div>
</template>

<script setup lang="ts">
import { type Contract } from "../../Types/Contract.js";

const contractStore = useContractsStore();
const downloadSignedContract = () => {
  contractStore.fetchSignedContract(props.contract.id, "CompanyRepresentative");
};

const props = defineProps<{
  contract: Contract;
}>();
</script>
