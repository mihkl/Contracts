<template>
  <div class="mt-10 px-4 w-full">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">Submissions</h1>

      <div class="flex justify-between mb-4">
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

      <SubmissionItem
        v-for="(submission, index) in filteredContracts"
        :key="index"
        :submission="submission"
        @openDetailsModal="openDetailsModal(submission)"
        @openUploadFinalContractModal="openUploadFinalContractModal(submission)"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import SubmissionItem from "./SubmissionItem.vue";
import SubmissionDetailsModal from "./SubmissionDetailsModal.vue";
import UploadFinalContractModal from "./UploadFinalContractModal.vue";
import SortOptionsModal from "../templates/SortOptionsModal.vue";

const auth = useAuth();
const submissions = ref<Contract[]>([]);
const modal = useModal();
const sortType = ref("alphabetical");
const sortOrder = ref("asc");
const filterQuery = ref("");
const contractsStore = useContractsStore();

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
  let filtered = contractsStore.contracts.filter((contract) =>
    contract.name.toLowerCase().includes(filterQuery.value.toLowerCase())
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


async function fetchSubmissions() {
  const response = await auth.fetchWithToken<Contract[]>("/contracts", {
    method: "GET",
    query: {
      status: SigningStatus.SignedByFirstParty,
    },
  });

  if (!response.error) {
    submissions.value = [...response];
    return submissions.value;
  }
}

function openDetailsModal(submission: Contract) {
  modal.open(SubmissionDetailsModal, {
    submission,
  });
}

function openUploadFinalContractModal(submission: Contract) {
  modal.open(UploadFinalContractModal, {
    contractId: submission.id,
  });
}

onMounted(() => {
  fetchSubmissions();
});
</script>
