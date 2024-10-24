import type { Template } from "@/Types/Template";

export const useTemplateUploadStore = defineStore("file", () => {
  const api = useApi();
  const templates = ref<Template[]>([]);
  const serverResponse = ref<UploadFileResponse | null>(null);
  const selectedFile = ref<File | null>(null);

  const setSelectedFile = (file: File | null) => {
    selectedFile.value = file;
  };

  const uploadFile = async () => {
    if (!selectedFile.value) {
      alert("Please select a file.");
      return;
    }

    const formData = new FormData();
    formData.append("file", selectedFile.value);

    try {
      const response = await api.customFetch<UploadFileResponse>("/upload", {
        method: "POST",
        body: formData, 
      });
      console.log("File uploaded successfully:", response);
      serverResponse.value = response;
    } catch (error) {
      console.error("Error uploading file:", error);
      throw error;
    }
  };

  const fetchTemplates = async () => {
    try {
      const response = await api.customFetch<Template[]>("/templates", {
        method: "GET",
      });
      templates.value = response;
      return templates.value;
    } catch (error) {
      console.error("Error fetching templates:", error);
      throw error;
    }
  };

  const deleteTemplate = async (id: number) => {
    try {
      await api.customFetch(`/templates/${id}`, {
        method: "DELETE",
      });
  
      await fetchTemplates(); 
  
      console.log(`Template with ID ${id} deleted successfully and templates updated`);
    } catch (error) {
      console.error(`Error deleting template with ID ${id}:`, error);
      throw error;
    }
  };

  const downloadFile = async (id: number, fileName: string) => {
    try {
      // Request the file from the API
      const response = await api.customFetch(`/templates/${id}/file`, {
        method: "GET",
        responseType: "blob",
      });
  
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
  

  return {
    templates,
    fetchTemplates,
    selectedFile,
    serverResponse,
    setSelectedFile,
    uploadFile,
    deleteTemplate,
    downloadFile
  };
});
