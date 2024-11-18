<template>
  <section class="p-10">
    <UForm
      class="space-y-4"
      v-if="
        Object.keys(formState)?.length > 0 &&
        !error &&
        !pdfUrl &&
        !contractSuccessfullyUploaded
      "
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
        <UInput :type="field.type" v-model="formState[field.name]" />
      </UFormGroup>
      <UButton type="submit">Submit</UButton>
    </UForm>

    <div v-if="pdfUrl && !contractSuccessfullyUploaded">
      <header>
        <h1 class="text-center text-2xl font-bold">
          Please download the contract to proceed or go back to modify.
        </h1>
      </header>

      <ClientOnly>
        <PdfViewer :pdfUrl="pdfUrl" class="border-1" />
      </ClientOnly>

      <div class="mt-4 flex justify-center">
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
    </div>

    <main v-if="error">
      <h1 class="text-center text-4xl mt-10 font-medium">
        Whoops! Seems like there is a problem.
      </h1>
      <p class="text-center mt-6 text-2xl">{{ error }}</p>
    </main>
    <div v-if="contractSuccessfullyUploaded">
      <header>
        <h1 class="text-center text-2xl font-bold mb-4">
          Thank you! The contract has been successfully signed.
        </h1>
        <p class="text-center text-lg">
          You may now close the page. The company representative will notify you
          of any further steps.
        </p>
      </header>
    </div>
  </section>
</template>

<script setup lang="ts">
import UploadSignedContractModal from "~/components/contracts/UploadSignedContractModal.vue";

const route = useRoute();
const api = useApi();
const toast = useToast();
const modal = useModal();
const runtimeConfig = useRuntimeConfig();

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
const contractSuccessfullyUploaded = ref<boolean>(false);

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
    const response = await api.fetchWithErrorHandling(`/contracts/${id}/generate-pdf`, {
      method: "POST",
      body: JSON.stringify({
        replacements: hasFields.value
          ? Object.keys(formState).map((key) => ({
              name: key,
              value: String(formState[key]),
            }))
          : [{ name: "string", value: "string" }],
      }),
    });
    if (response?.error) {
      return;
    }

    pdfUrl.value = `${runtimeConfig.public.apiBaseUrl}/contracts/${id}/pdf`;
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

function onSuccessfulContractUpload() {
  contractSuccessfullyUploaded.value = true;
}

function openSignedContractUploadModal() {
  modal.open(UploadSignedContractModal, {
    onSuccessfulContractUpload,
  });
}
</script>

<style>
canvas {
  border: 1px solid black;
}
</style>
