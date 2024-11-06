<template>
  <div class="mt-10 px-4 w-full max-w-5xl">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">My contracts</h1>
      <ContractItem
        v-for="(contract, index) in contractsStore.contracts"
        :key="index"
        :contract="contract"
        @openDetailsModal="openDetailsModal(contract.id)"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import { useContractsStore } from "@/stores/ContractsStore";
import ContractItem from "./ContractItem.vue";
import DetailsModal from "./DetailsModal.vue";

const contractsStore = useContractsStore();
const modal = useModal();

async function fetchContracts() {
  await contractsStore.fetchContracts();
}

function openDetailsModal(contractId: number) {
  modal.open(DetailsModal, {
    contractId: contractId,
  });
}

onMounted(() => {
  fetchContracts();
});
</script>
