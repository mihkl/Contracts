import type { Contract } from "~/Types/Contract";

export const useContractsStore = defineStore("contract", () => {
  const api = useApi();

  const contracts = ref<Contract[]>([]);

  const fetchContracts = async () => {
      const response = await api.fetchWithErrorHandling<Contract[]>("/contracts", {
        method: "GET",
      });
      if (response){
        contracts.value = response;
        return contracts.value;
      }
  };

  return {
    contracts,
    fetchContracts,
  };
});
