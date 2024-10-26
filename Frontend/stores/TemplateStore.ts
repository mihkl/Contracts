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
      toast.add({
        title: "No file selected",
        description: "Please select a file to upload",
      });
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
    const response = await api.fetchWithErrorHandling<Template[]>(
      "/templates",
      {
        method: "GET",
      }
    );
    if (response) {
      templates.value = response;
      return templates.value;
    }
  };

  const downloadFile = async (id: number, fileName: string) => {
    try {
      const response = await api.fetchWithErrorHandling(
        `/templates/${id}/file`,
        {
          method: "GET",
          responseType: "blob",
        }
      );

      const blob = response as Blob;

      const url = window.URL.createObjectURL(blob);

      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", fileName);

      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);

      console.log(`File '${fileName}' downloaded successfully.`);
    } catch (error) {
      console.error(`Error downloading file with ID ${id}:`, error);
      throw error;
    }
  };

  const deleteTemplate = async (id: number) => {
    try {
      await api.fetchWithErrorHandling(`/templates/${id}`, {
        method: "DELETE",
      });

      await fetchTemplates();

      console.log(
        `Template with ID ${id} deleted successfully and templates updated`
      );
    } catch (error) {
      console.error(`Error deleting template with ID ${id}:`, error);
      throw error;
    }
  };

  return {
    templates,
    fetchTemplates,
    selectedFile,
    serverResponse,
    setSelectedFile,
    uploadFile,
    downloadFile,
    deleteTemplate,
  };
});
