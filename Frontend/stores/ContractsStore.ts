import { useAuth } from "~/composables/useAuth";
import type { Contract, SigningStatus } from "~/Types/Contract";

export const useContractsStore = defineStore("contract", () => {
  const auth = useAuth();

  const contracts = ref<Contract[]>([]);

  const fetchContracts = async () => {
    const response = await auth.fetchWithToken<Contract[]>("/contracts", {
      method: "GET",
    });
    if (!response.error) {
      contracts.value = response;
      return contracts.value;
    }
  };

  const fetchPDF = async (contractId: number) => {
    try {
      const response = await auth.fetchWithToken<Blob>(
        `/contracts/${contractId}/pdf`,
        {
          method: "GET",
        }
      );

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
