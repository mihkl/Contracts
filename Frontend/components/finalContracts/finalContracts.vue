<template>
  <div class="mt-10 px-4 w-full max-w-5xl">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">Final contracts</h1>
      <finalContractItem
        v-for="(contract, index) in contracts"
        :key="index"
        :contract="contract"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import finalContractItem from "./finalContractItem.vue";

const auth = useAuth();
const contracts = ref<Contract[]>([]);

async function fetchContracts() {
  const response = await auth.fetchWithToken<Contract[]>("/contracts", {
    method: "GET",
    query: {
      minimumStatus: SigningStatus.SignedByAll,
    },
  });

  if (!response.error) {
    contracts.value = [...response];
    return contracts.value;
  }
}
onMounted(() => {
  fetchContracts();
});
</script>
