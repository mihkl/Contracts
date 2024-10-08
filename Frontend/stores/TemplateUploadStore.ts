export const useTemplateUploadStore = defineStore('file', () => {
    const api = useApi()
    const selectedFile = ref<File | null>(null)
    const serverResponse = ref<UploadFileResponse | null>(null)

    
    const setSelectedFile = (file: File | null) => {
      selectedFile.value = file
    }
  
    const uploadFile = async () => {
      if (!selectedFile.value) {
        alert('Please select a file.')
        return
      }
  
      const formData = new FormData()
      formData.append('file', selectedFile.value)
  
      try {
        const response = await api.customFetch<UploadFileResponse>("/upload", {
          method: "POST",
          body: formData,
        })
        console.log('File uploaded successfully:', response)
        serverResponse.value = response

      } catch (error) {
        console.error('Error uploading file:', error)
        throw error
      }
    }
    return { selectedFile, serverResponse, setSelectedFile, uploadFile }
  })