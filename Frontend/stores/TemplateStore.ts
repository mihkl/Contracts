
import UploadConfirmationModal from "@/components/UploadConfirmationModal.vue";
import type { Template } from "@/Types/Template";

export const useTemplateStore = defineStore("file", () => {
  const modal = useModal();
  const toast = useToast();
  const api = useApi();
  const templates = ref<Template[]>([]);
  const serverResponse = ref<UploadFileResponse | null>(null);
  const selectedFile = ref<File | null>(null);

  const setSelectedFile = (file: File | null) => {
    selectedFile.value = file;
  };

  const uploadFile = async () => {
    if (!selectedFile.value) {
      toast.add({ title: "No file selected", description: "Please select a file to upload" });
      return;
    }

    const formData = new FormData();
    formData.append("file", selectedFile.value);

    const response = await api.fetchWithErrorHandling<UploadFileResponse>(
      "/upload",
      {
        method: "POST",
        body: formData,
      }
    );
    if (response) {
      serverResponse.value = response;
      modal.open(UploadConfirmationModal);
    }
  };

  const fetchTemplates = async () => {

      const response = await api.fetchWithErrorHandling<Template[]>("/templates", {
        method: "GET",
      });
      if (response){
        templates.value = response;
        return templates.value;
      }
  };

  return {
    templates,
    fetchTemplates,
    selectedFile,
    serverResponse,
    setSelectedFile,
    uploadFile,
  };
});
