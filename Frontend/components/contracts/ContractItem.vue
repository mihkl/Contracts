<template>
  <div
    :class="[
      'p-4 border rounded-lg cursor-pointer transition flex items-center justify-between bg-white border-gray-300',
    ]"
  >
    <span>{{ contract?.name }}</span>
    <div class="flex space-x-2">
      <button
        class="px-6 py-2 bg-purple-500 text-white rounded-lg hover:bg-purple-600 transition"
        @click="openDetails(template.id)"
      >
        Details
      </button>
    </div>
  </div>
</template>

<script setup>
const emits = defineEmits(["openModal", "openDetailsModal"]);
const api = useApi();
const store = useTemplateStore();

const props = defineProps({
  contract: {
    name: String,
    id: Number,
  },
});

const openDetails = async (id) => {
  openDetailsModal();
};

const openModal = () => {
  emits("openModal", true);
};

const openDetailsModal = () => {
  emits("openDetailsModal", true);
};
</script>
