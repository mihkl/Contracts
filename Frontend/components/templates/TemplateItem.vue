<template>
  <div @mouseover="isHighlighted = true" @mouseleave="isHighlighted = false" :class="[
    'p-4 border rounded-lg cursor-pointer transition flex items-center justify-between text-black',
    isHighlighted
      ? 'bg-gray-200 border-purple-500'
      : 'bg-white border-gray-300',
  ]">
    <span>{{ template.name }}</span>
    <button class="px-6 py-2 bg-purple-500 text-white rounded-lg hover:bg-purple-600 transition"
      @click="generateLink(template.id)">
      Generate link
    </button>
  </div>
</template>

<script setup>
const emits = defineEmits(["openModal"]);

const props = defineProps({
  template: {
    name: String,
    id: Number,
  },
});

const generateLink = async (id) => {
  openModal();
};

const openModal = () => {
  emits("openModal", true);
};
</script>
