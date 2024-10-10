<template>
  <div
    @mouseover="isHighlighted = true"
    @mouseleave="isHighlighted = false"
    :class="[
      'p-4 border rounded-lg cursor-pointer transition flex items-center justify-between',
      isHighlighted
        ? 'bg-gray-200 border-purple-500'
        : 'bg-white border-gray-300',
    ]"
  >
    <span>{{ template.name }}</span>
    <button
      class="px-6 py-2 bg-purple-500 text-white rounded-lg hover:bg-purple-600 transition"
      @click="generateLink(template.id)"
    >
      Generate link
    </button>
  </div>
</template>

<script setup>

const emits = defineEmits(["openModal"]);
const api = useApi();
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


const openModal = () => {
  emits("openModal", true);
};
</script>
