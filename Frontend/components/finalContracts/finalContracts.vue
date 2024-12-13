<template>
  <div class="mt-10 px-4 w-full max-w-5xl">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">Final contracts</h1>

      <div v-if="filteredContracts.length > 0" class="flex justify-between mb-4">
        <input
          type="text"
          v-model="filterQuery"
          placeholder="Search by name"
          class="border px-3 py-2 rounded"
        />
        <button @click="openSortOptionsModal" class="border px-3 py-2 rounded flex items-center">
          Sort Options
        </button>
      </div>

      <div v-if="filteredContracts.length === 0" class="text-center text-gray-500">
        No Final Contracts Available
      </div>
      <finalContractItem
        v-for="(contract, index) in filteredContracts"
        :key="index"
        :contract="contract"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import finalContractItem from "./finalContractItem.vue";
import SortOptionsModal from "../templates/SortOptionsModal.vue";

const auth = useAuth();
const contracts = ref<Contract[]>([]);
const filterQuery = ref("");
const sortType = ref("alphabetical");
const sortOrder = ref("asc");
const contractsStore = useContractsStore();
const modal = useModal();

function openSortOptionsModal() {
  modal.open(SortOptionsModal, {
    'onApply-sort': applySortOptions,
    sortType: sortType.value,
    sortOrder: sortOrder.value,
  });
}

function applySortOptions({ type, order }: { type: string; order: string }) {
  sortType.value = type;
  sortOrder.value = order;
}

const filteredContracts = computed(() => {
  let filtered = contractsStore.contracts.filter(
    (contract) =>
      contract.name.toLowerCase().includes(filterQuery.value.toLowerCase()) &&
      contract.signingStatus === SigningStatus.SignedByAll
  );

  filtered = filtered.sort((a, b) => {
    if (sortType.value === "alphabetical") {
      return sortOrder.value === "asc"
        ? a.name.localeCompare(b.name)
        : b.name.localeCompare(a.name);
    } else if (sortType.value === "creationTime") {
      return sortOrder.value === "asc"
        ? new Date(a.creationTime).getTime() - new Date(b.creationTime).getTime()
        : new Date(b.creationTime).getTime() - new Date(a.creationTime).getTime();
    }
    return 0;
  });

  return filtered;
});

async function fetchContracts() {
  const response = await auth.fetchWithToken<Contract[]>("/contracts", {
    method: "GET",
    query: {
      status: SigningStatus.SignedByAll,
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
