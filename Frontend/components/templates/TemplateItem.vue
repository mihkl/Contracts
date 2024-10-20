<template>
  <div
    class="p-4 border rounded-lg cursor-pointer transition flex items-center justify-between bg-white border-gray-300"
  >
    <span>{{ template.name }}</span>
    <div class="flex space-x-2">
      <button
        class="px-6 py-2 bg-purple-500 text-white rounded-lg hover:bg-purple-600 transition"
        @click="generateLink(template.id)"
      >
        Generate link
      </button>

      <button
        class="px-6 py-2 bg-purple-500 text-white rounded-lg hover:bg-purple-600 transition"
        @click="openDetails(template.id)"
      >
        Details
      </button>

      <button
        class="px-6 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition"
        @click="deleteTemplate(template.id)"
      >
        Delete
      </button>
    </div>
  </div>
</template>

<script setup>
import { useTemplateUploadStore } from "@/stores/TemplateUploadStore";

const emits = defineEmits(["openModal"]);
const api = useApi();
const store = useTemplateUploadStore();

const props = defineProps({
  template: {
    name: String,
    id: Number,
  },
});

const generateLink = async (id) => {
  try {
    // Open the modal before the request
    openModal();

    // Make an API request to fetch the link
    const response = await api.customFetch(`/contracts/${id}/url`, {
      method: "GET", // Specify the method if needed (default is GET)
    });

    // Handle the response (assuming the response has a `link` field)
    if (response && response.link) {
      console.log("Generated link:", response.link);
      // You can now use the response data (e.g., display the link in the modal)
    }
  } catch (error) {
    console.error("Error fetching the link:", error);
    // Handle errors appropriately
  }
};

const openDetails = async (id) => {
  try {
    // Open the modal before the request
    openDetailsModal();


  } catch (error) {
    console.error("Error fetching the link:", error);
    // Handle errors appropriately
  }
};

const openModal = () => {
  emits("openModal", true);
};

const openDetailsModal = () => {
  emits("openDetailsModal", true);
};

const deleteTemplate = async (id) => {
  try {
    await store.deleteTemplate(id); 
    console.log(`Template with ID ${id} deleted successfully`);
  } catch (error) {
    console.error("Error deleting template:", error);
  }
};
</script>
