<template>
  <div>
    <p>Here you can view all your contracts.</p>
    <div v-for="(template, index) in contractsStore.contracts" :key="index">
      {{ template.name }}
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
