import type { Contract } from "~/Types/Contract";

export const useContractsStore = defineStore("contract", () => {
  const api = useApi();

  const contracts = ref<Contract[]>([]);

  const fetchContracts = async () => {
    try {
      const response = await api.customFetch<Contract[]>("/contracts", {
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
  };
});
