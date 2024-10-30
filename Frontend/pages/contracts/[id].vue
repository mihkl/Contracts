<template>
  <section class="p-10">
    <UForm
      class="space-y-4"
      v-if="Object.keys(formState)?.length > 0 && !error && !showPdf"
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
    
    <main v-if="error">
      <h1 class="text-center text-4xl mt-10 font-medium">
        Whoops! Seems like the link is not correct.
      </h1>
      <p class="text-center mt-6 text-2xl">{{ error }}</p>
    </main>

    <!-- Display PdfViewer component after submission -->
    <client-only v-if="showPdf">
      <PdfViewer />
    </client-only>
  </section>
</template>

<script setup lang="ts">
import { useRoute } from "nuxt/app";
import { ref, reactive, onMounted } from 'vue';

const route = useRoute();
const api = useApi();
const toast = useToast();

definePageMeta({
  layout: false,
});

const contractFields = ref<{
  fields: { name: string; type: string }[];
}>();
const error = ref<string>();
const formState = reactive<Record<string, any>>({});
const showPdf = ref(false); // New flag for displaying PdfViewer

const id = route.params?.id;
const signature = route.query?.signature;
const validFrom = route.query?.validFrom;
const validUntil = route.query?.validUntil;

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
  }

  contractFields.value = response;

  response.fields.forEach((field: { name: string }) => {
    formState[field.name] = null;
  });
});

async function onSubmit() {
  const toastId = "loading";
  toast.add({
    id: toastId,
    title: "Loading...",
    timeout: 0,
  });

  await api.fetchWithErrorHandling(`/contracts/${id}/generate-pdf`, {
    method: "POST",
    body: JSON.stringify({
      replacements: [
        ...Object.keys(formState).map((key) => ({
          name: key,
          value: formState[key],
        })),
      ],
    }),
  });

  toast.remove(toastId);
  showPdf.value = true; // Show PdfViewer component after submission
}
</script>
