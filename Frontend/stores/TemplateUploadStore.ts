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

  return {
    templates,
    fetchTemplates,
    selectedFile,
    serverResponse,
    setSelectedFile,
    uploadFile,
  };
});
