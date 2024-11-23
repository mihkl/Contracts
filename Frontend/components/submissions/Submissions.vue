<template>
  <div class="mt-10 px-4 w-full max-w-5xl">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">My contracts</h1>
      <SubmissionItem
        v-for="(contract, index) in contractsStore.contracts"
        :key="index"
        :submission="contract"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import { useContractsStore } from "@/stores/ContractsStore";
import SubmissionItem from "./SubmissionItem.vue";

const contractsStore = useContractsStore();
const modal = useModal();
const auth = useAuth();
const submissions = ref<Contract[]>([]);

async function fetchSubmissions() {
  const response = await auth.fetchWithToken<Contract[]>("/contracts", {
    method: "GET",
    query: {
      SigningStatus: SigningStatus.SignedByFirstParty,
    },
  });

  if (!response.error) {
    submissions.value = [...response];
    return submissions.value;
  }
}

onMounted(() => {
  fetchSubmissions();
});
</script>
