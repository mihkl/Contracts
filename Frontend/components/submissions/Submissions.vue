<template>
  <div class="mt-10 px-4 w-full">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">My Submissions</h1>
      <SubmissionItem
        v-for="(submission, index) in submissions"
        :key="index"
        :submission="submission"
        @openDetailsModal="openDetailsModal(submission)"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import SubmissionItem from "./SubmissionItem.vue";
import SubmissionDetailsModal from "./SubmissionDetailsModal.vue";

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

onMounted(() => {
  fetchSubmissions();
});
</script>
