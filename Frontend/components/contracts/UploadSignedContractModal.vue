<template>
  <UModal prevent-close>
    <UCard
      :ui="{
        ring: '',
        divide: 'divide-y divide-gray-100 dark:divide-gray-800',
      }"
    >
      <template #header>
        <div class="flex-col items-start justify-center">
          <div class="flex flex-row justify-between align-center mb-2">
            <h3
              class="text-base font-semibold leading-6 text-gray-900 dark:text-white"
            >
              Upload signed contract
            </h3>
            <UButton
              color="gray"
              variant="ghost"
              icon="i-heroicons-x-mark-20-solid"
              class="-my-1"
              @click="modal.close()"
            />
          </div>
          <p>
            Please sign the downloaded PDF contract and upload a .asice file.
          </p>
        </div>
      </template>
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
    </UCard>
  </UModal>
</template>

<script setup lang="ts">
const selectedFile = ref();
const modal = useModal();
const toast = useToast();
const api = useApi();
const route = useRoute();

const props = defineProps<{
  onSuccessfulContractUpload: () => void;
}>();

const id = route.params?.id;

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
    `contracts/${id}/sign`,
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
  } else props.onSuccessfulContractUpload();
};
</script>
