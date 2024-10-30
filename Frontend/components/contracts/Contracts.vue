<template>
  <div class="mt-10 px-4 w-full max-w-5xl">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">My contracts</h1>
      <div v-for="(template, index) in contractsStore.contracts" :key="index">
        {{ template.name }}
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import { useContractsStore } from "@/stores/ContractsStore";

const contractsStore = useContractsStore();

async function fetchContracts() {
  await contractsStore.fetchContracts();
}

onMounted(() => {
  fetchContracts();
});
</script>
