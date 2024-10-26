<template>
  <div
    :class="[
      'p-4 border rounded-lg cursor-pointer transition flex items-center justify-between bg-white border-gray-300',
    ]"
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
const emits = defineEmits(["openModal", "openDetailsModal"]);
const api = useApi();
const store = useTemplateStore();

const props = defineProps({
  template: {
    name: String,
    id: Number,
  },
});

const generateLink = async (id) => {
  openModal();
};

const openDetails = async (id) => {
  openDetailsModal();
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
