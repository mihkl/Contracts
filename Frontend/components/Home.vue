<template>
  <div class="mt-10 px-4">
    <h2 class="text-3xl font-semibold mb-6">Upload a .docx template</h2>
    <form class="space-y-6">
      <div>
        <input
          type="file"
          @change="onFileChange"
          class="mt-5 block w-full text-sm text-gray-600 dark:text-gray-300 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-indigo-100 file:text-indigo-700 hover:file:bg-indigo-200"
        />
      </div>

      <button
        type="button"
        @click="submitFile"
        class="px-4 py-2 bg-indigo-600 text-white font-semibold rounded-md hover:bg-indigo-700 mt-4"
      >
        Upload File
      </button>
    </form>
  </div>
</template>

<script setup lang="ts">
import UploadConfirmationModal from './UploadConfirmationModal.vue';

const { serverResponse, setSelectedFile, uploadFile } = useTemplateUploadStore();

const onFileChange = (event: Event) => {
  const input = event.target as HTMLInputElement;
  if (input.files && input.files.length > 0) {
    setSelectedFile(input.files[0]);
  }
};

const submitFile = async () => {
  await uploadFile();
  console.log(serverResponse);
  openModal();
};

const modal = useModal();

function openModal() {
  modal.open(UploadConfirmationModal);
}
</script>

<style scoped>
</style>
