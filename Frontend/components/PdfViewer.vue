<template>
  <div class="flex flex-col items-center p-4">
    <div class="flex items-center space-x-4 mb-4">
      <button 
        @click="prevPage" 
        class="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-300 transition"
        :disabled="page <= 1"
      >
        Prev
      </button>
      
      <span class="text-gray-700 font-medium">{{ page }} / {{ pages }}</span>
      
      <button 
        @click="nextPage" 
        class="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-300 transition"
        :disabled="page >= pages"
      >
        Next
      </button>
    </div>
    
    <VuePDF :pdf="pdf" :page="page" />
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
import { VuePDF, usePDF } from '@tato30/vue-pdf';

const props = defineProps<{ pdfUrl: string }>();

const page = ref(1); // Current page number

// Load PDF using pdfUrl prop
const { pdf, pages } = usePDF(props.pdfUrl);

// Watch for changes in pdfUrl to reset the page and reload the PDF
watch(() => props.pdfUrl, (newUrl) => {
  page.value = 1; // Reset to the first page
  usePDF(newUrl); // Reload the PDF with the new URL
});

// Navigation methods for previous and next pages
function nextPage() {
  if (page.value < pages.value) page.value++;
}

function prevPage() {
  if (page.value > 1) page.value--;
}
</script>
