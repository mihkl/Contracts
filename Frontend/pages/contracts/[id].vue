<template>
  <section class="p-10">
    <UForm
      class="space-y-4"
      v-if="Object.keys(formState)?.length > 0 && !error && !pdfUrl"
      :state="formState"
      @submit="onSubmit"
    >
      <UFormGroup
        v-for="(field, index) in contractFields?.fields"
        :label="field.name"
        name="contractName"
        required
        :key="index"
      >
        <UInput type="string" v-model="formState[field.name]" />
      </UFormGroup>
      <UButton type="submit">Submit</UButton>
    </UForm>

    <header v-if="pdfUrl">
      <h1 class="text-center text-2xl font-bold">
        Please download the contract to proceed or go back to modify.
      </h1>
    </header>

    <ClientOnly>
      <PdfViewer v-if="pdfUrl" :pdfUrl="pdfUrl" class="border-1" />
    </ClientOnly>
    <header></header>

    <div v-if="pdfUrl" class="mt-4 flex justify-center">
      <UButton v-if="hasFields" @click="resetForm" class="mr-2">
        Back to Form
      </UButton>
      <UButton @click="downloadPdf" class="mr-2">Download PDF </UButton>
      <UButton
        @click="openSignedContractUploadModal"
        v-if="pdfDownloaded === true"
        >Upload signed contract
      </UButton>
    </div>

    <main v-if="error">
      <h1 class="text-center text-4xl mt-10 font-medium">
        Whoops! Seems like the link is not correct.
      </h1>
      <p class="text-center mt-6 text-2xl">{{ error }}</p>
    </main>
  </section>
</template>

<script setup lang="ts">
import UploadSignedContractModal from "~/components/contracts/UploadSignedContractModal.vue";

const route = useRoute();
const api = useApi();
const toast = useToast();
const modal = useModal();

definePageMeta({
  layout: false,
});

const contractFields = ref<{
  fields: { name: string; type: string }[];
}>();
const error = ref<string>();
const formState = reactive<Record<string, any>>({});
const pdfUrl = ref<string>();
const pdfDownloaded = ref<boolean>(false);

const id = route.params?.id;
const signature = route.query?.signature;
const validFrom = route.query?.validFrom;
const validUntil = route.query?.validUntil;

const hasFields = computed(
  () => (contractFields.value?.fields ?? []).length > 0
);

async function generatePdf() {
  const toastId = "loading";
  toast.add({
    id: toastId,
    title: "Loading...",
    timeout: 0,
  });

  try {
    await api.fetchWithErrorHandling(`/contracts/${id}/generate-pdf`, {
      method: "POST",
      body: JSON.stringify({
        replacements: hasFields.value
          ? Object.keys(formState).map((key) => ({
              name: key,
              value: formState[key],
            }))
          : [{ name: "string", value: "string" }],
      }),
    });

    pdfUrl.value = `http://localhost:5143/api/contracts/${id}/pdf`;
  } catch (err) {
    toast.add({
      title: "Error",
      description: "Failed to generate PDF",
      color: "red",
    });
  } finally {
    toast.remove(toastId);
  }
}

onMounted(async () => {
  const response = await api.fetchWithErrorHandling<{
    fields: { name: string; type: string }[];
  }>(
    `/contracts/${id}?signature=${encodeURIComponent(
      typeof signature === "string" && signature
    )}&validFrom=${validFrom}&validUntil=${validUntil}`,
    { method: "GET" }
  );

  if (response?.error) {
    error.value = response.error;
    return;
  }

  contractFields.value = response;

  if (response.fields?.length > 0) {
    response.fields.forEach((field: { name: string }) => {
      formState[field.name] = null;
    });
  } else {
    // If there are no fields, generate the PDF immediately
    await generatePdf();
  }
});

async function onSubmit() {
  await generatePdf();
  pdfDownloaded.value = false;
}

function resetForm() {
  pdfUrl.value = undefined;
}

function downloadPdf() {
  if (pdfUrl.value) {
    window.open(pdfUrl.value, "_blank");
  }
  pdfDownloaded.value = true;
}

function openSignedContractUploadModal() {
  modal.open(UploadSignedContractModal);
}
</script>

<style>
canvas {
  border: 1px solid black;
}
</style>
