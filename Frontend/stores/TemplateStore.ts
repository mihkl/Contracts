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

  const downloadFile = async (id: number) => {
    try {
      // Request the file from the API
      const response = await api.customFetch(`/templates/${id}/file`, {
        method: "GET",
        responseType: "blob", // Expecting the response to be a Blob
      });
  
      // Type assertion to let TypeScript know that this is a Blob
      const blob = response as Blob;
  
      // Create a download link from the Blob
      const url = window.URL.createObjectURL(blob); // Use the Blob directly
  
      // Create a link element for downloading the file
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", `template_${id}.docx`); // Set a filename for the downloaded file
  
      // Append the link to the document body, trigger the download, then clean up
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
  
      console.log("DOCX file downloaded successfully.");
    } catch (error) {
      console.error(`Error downloading DOCX file with ID ${id}:`, error);
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
