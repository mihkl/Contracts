<template>
  <div class="mt-10 px-4 w-full max-w-5xl">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">Submissions</h1>
      <SubmissionItem
        v-for="(submission, index) in submissions"
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

const auth = useAuth();
const submissions = ref<Contract[]>([]);
const modal = useModal();

async function fetchSubmissions() {
  const response = await auth.fetchWithToken<Contract[]>("/contracts", {
    method: "GET",
    query: {
      minimumStatus: SigningStatus.SignedByFirstParty,
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
