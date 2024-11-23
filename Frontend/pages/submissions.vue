<template>
  <Submissions />
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import { useContractsStore } from "@/stores/ContractsStore";

const contractsStore = useContractsStore();
const modal = useModal();

async function fetchContracts() {
  await contractsStore.fetchContracts(SigningStatus.SignedByFirstParty);
}

function openDetailsModal(contractId: number) {
  /*modal.open(DetailsModal, {
    contractId: contractId,
  });*/
}

onMounted(() => {
  fetchContracts();
});
</script>
