import type { Template } from "@/Types/Template";

export const useContractsStore = defineStore("contract", () => {
  const api = useApi();

  const contracts = ref<Template[]>([]);
  const serverResponse = ref<UploadFileResponse | null>(null);

  const selectedFile = ref<File | null>(null);

  const setSelectedFile = (file: File | null) => {
    selectedFile.value = file;
  };

  const fetchContracts = async () => {
    try {
      const response = await api.customFetch<Template[]>("/contracts", {
        method: "GET",
      });
      contracts.value = response;
      return contracts.value;
    } catch (error) {
      console.error("Error fetching contracts:", error);
      throw error;
    }
  };

  return {
    contracts,
    fetchContracts,
    selectedFile,
    serverResponse,
    setSelectedFile,
  };
});
