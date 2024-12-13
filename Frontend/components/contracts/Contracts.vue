<template>
  <div class="mt-10 px-4 w-full">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">My Contracts</h1>

      <div v-if="filteredContracts.length > 0" class="flex justify-between mb-4">
        <input
          type="text"
          v-model="filterQuery"
          placeholder="Search by name/keywords"
          class="border px-3 py-2 rounded"
        />
        <button @click="openSortOptionsModal" class="border px-3 py-2 rounded flex items-center">
          Sort Options
        </button>
      </div>

      <div v-if="filteredContracts.length === 0" class="text-center text-gray-500">
        No Contracts Available
      </div>
      <ContractItem
        v-for="(contract, index) in filteredContracts"
        :key="index"
        :contract="contract"
        @openDetailsModal="openDetailsModal(contract.id)"
      />
    </div>

  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from "vue";
import { useContractsStore } from "@/stores/ContractsStore";
import ContractItem from "./ContractItem.vue";
import DetailsModal from "./DetailsModal.vue";
import SortOptionsModal from "../templates/SortOptionsModal.vue";
import debounce from "lodash/debounce";

const contractsStore = useContractsStore();
const modal = useModal();
const filterQuery = ref("");
const showSortOptionsModal = ref(false);
const sortType = ref("alphabetical");
const sortOrder = ref("asc");

async function fetchContracts(query: string) {
  await contractsStore.fetchContracts(query);
}

onMounted(() => {
  fetchContracts(filterQuery.value);
});

const debouncedFetchContracts = debounce(fetchContracts, 300);

watch(filterQuery, (newQuery) => {
  debouncedFetchContracts(newQuery);
});

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
  let filtered = contractsStore.contracts;

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

function openDetailsModal(contractId: number) {
  modal.open(DetailsModal, {
    contractId: contractId,
  });
}
</script>
