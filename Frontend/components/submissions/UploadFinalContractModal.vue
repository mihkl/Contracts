<template>
  <UModal prevent-close>
    <UCard
      :ui="{
        ring: '',
        divide: 'divide-y divide-gray-100 dark:divide-gray-800',
      }"
    >
      <template #header>
        <div class="flex items-center justify-between">
          <h3
            class="text-base font-semibold leading-6 text-gray-900 dark:text-white"
          >
            Upload final contract
          </h3>
        </div>
        <section>
          <input
            type="file"
            @change="onFileChange"
            class="mt-5 block w-full text-sm text-gray-600 dark:text-gray-300 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-indigo-100 file:text-indigo-700 hover:file:bg-indigo-200"
            accept=".asice"
          />
          <button
            type="button"
            @click="submitFile"
            class="px-4 py-2 bg-indigo-600 text-white font-semibold rounded-md hover:bg-indigo-700 mt-4"
          >
            Upload File
          </button>
        </section>
      </template>
    </UCard>
  </UModal>
</template>

<script setup lang="ts">
const selectedFile = ref();
const toast = useToast();
const modal = useModal();
const api = useApi();

const props = defineProps<{
  contractId: number;
}>();

const onFileChange = (event: Event) => {
  const input = event.target as HTMLInputElement;
  if (input.files && input.files.length > 0) {
    selectedFile.value = input.files[0];
  }
};

const submitFile = async () => {
  if (!selectedFile.value) {
    toast.add({
      title: "No file selected",
      description: "Please select a file to upload",
    });
    return;
  }

  const formData = new FormData();
  formData.append("file", selectedFile.value);

  const response = await api.fetchWithErrorHandling<UploadFileResponse>(
    `contracts/${props.contractId}/sign`,
    {
      method: "POST",
      body: formData,
    }
  );

  if (response?.error) {
    toast.add({
      title: "There has been an error.",
      description: response?.error,
    });
  } else {
    modal.close();
  }
};
</script>
