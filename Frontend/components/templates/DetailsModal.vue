<template>
  <!-- Small Pop-up Modal for Details -->
  <div v-if="isDetailsModalOpen" class="fixed inset-0 bg-gray-600 bg-opacity-50 flex items-center justify-center z-50">
    <div class="relative bg-white p-4 rounded-lg shadow-lg w-96">
      <!-- Close icon (X) at the top-right corner -->
      <button
        class="absolute top-2 right-2 text-gray-500 hover:text-gray-700 transition"
        @click="closeDetailsModal"
      >
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mt-2 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
          <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" />
        </svg>
      </button>

      <h2 class="text-xl font-semibold mb-4">Template Details</h2>
      <p>Template: {{ template.name }}</p>
      <p>Id: {{ template.id }}</p>

      <!-- Structured display for fields -->
      <div v-if="template.fields && template.fields.length > 0">
        <h3>Fields:</h3>
        <ul class="list-disc ml-5">
          <li v-for="(field, index) in template.fields" :key="index">
            {{ field.name }}: {{ field.type }}
          </li>
        </ul>
      </div>
      <div v-else>
        <p>No fields available.</p>
      </div>
    </div>
  </div>
</template>

<script setup>
const props = defineProps({
  template: {
    name: String,
    id: Number,
    fields: {
      type: Array,
      default: () => []
    }
  },
  isDetailsModalOpen: Boolean,
});

const emits = defineEmits(["close"]);

const closeDetailsModal = () => {
  emits("close");
};
</script>
