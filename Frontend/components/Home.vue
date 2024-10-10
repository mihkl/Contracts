<template>
    <div class="bg-light-900">
        <input type="file" @change="onFileChange" />
        <button @click="submitFile">Upload File</button>
    </div>
</template>

<script setup lang="ts">
import UploadConfirmationModal from './UploadConfirmationModal.vue';

const { serverResponse, setSelectedFile, uploadFile } = useTemplateUploadStore()

const onFileChange = (event: Event) => {
  const input = event.target as HTMLInputElement
  if (input.files && input.files.length > 0) {
    setSelectedFile(input.files[0])
  }
}

const submitFile = async () => {
  await uploadFile();
  console.log(serverResponse)
    openModal()
}

const modal = useModal()

function openModal(){
  modal.open(UploadConfirmationModal)
}
</script>