import type { Contract } from "~/Types/Contract";

export const useContractsStore = defineStore("contract", () => {
  const api = useApi();

  const contracts = ref<Contract[]>([]);

  const fetchContracts = async () => {
      const response = await api.fetchWithErrorHandling<Contract[]>("/contracts", {
        method: "GET",
      });
      if (!response.error){
        contracts.value = response;
        return contracts.value;
      }
  };

  const fetchPDF = async (contractId: number) => {
    try {
      const response = await api.fetchWithErrorHandling<Blob>(`/contracts/${contractId}/pdf`, {
        method: "GET",
      });

      if (!response.error) {
        const pdfBlobUrl = URL.createObjectURL(response);
        window.open(pdfBlobUrl, "_blank"); 
      }
    } catch (error) {
      console.error("Error fetching PDF:", error);
    }
  };
  return {
    contracts,
    fetchContracts,
    fetchPDF,
  };
});
