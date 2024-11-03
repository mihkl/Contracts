<template>
  <div class="flex flex-col items-center p-4">
    <VuePDF :pdf="pdf" :page="page" />

    <div class="mt-2 text-gray-700 font-medium">
      Page {{ page }} / {{ pages }}
    </div>
    
    <div class="flex items-center space-x-2 mt-4">
      <button 
        @click="goToPage(1)" 
        class="px-2 py-1 border rounded hover:bg-gray-100"
        :disabled="page <= 1"
      >
        First
      </button>

      <button 
        @click="prevPage" 
        class="px-2 py-1 border rounded hover:bg-gray-100"
        :disabled="page <= 1"
      >
        &lt; Prev
      </button>

      <template v-for="p in pagesArray" :key="p">
        <button
          v-if="p === '...'"
          disabled
          class="px-2 py-1 border rounded bg-gray-100 text-gray-500"
        >
          ...
        </button>
        <button 
          v-else
          @click="goToPage(p)" 
          :class="{'bg-blue-500 text-white': page === p, 'border': page !== p}"
          class="px-2 py-1 rounded hover:bg-blue-100"
        >
          {{ p }}
        </button>
      </template>

      <button 
        @click="nextPage" 
        class="px-2 py-1 border rounded hover:bg-gray-100"
        :disabled="page >= pages"
      >
        Next &gt;
      </button>

      <button 
        @click="goToPage(pages)" 
        class="px-2 py-1 border rounded hover:bg-gray-100"
        :disabled="page >= pages"
      >
        Last
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue';
import { VuePDF, usePDF } from '@tato30/vue-pdf';

const props = defineProps<{ pdfUrl: string }>();
const page = ref(1);
const { pdf, pages } = usePDF(props.pdfUrl);

// Configuration for pagination
const SIBLING_COUNT = 1; // Number of siblings to show on each side of current page
const BOUNDARY_COUNT = 1; // Number of boundary pages to always show

watch(() => props.pdfUrl, (newUrl) => {
  page.value = 1;
  usePDF(newUrl);
});


const range = (start: number, end: number): number[] => {
  const length = end - start + 1;
  return Array.from({ length }, (_, i) => start + i);
};

const pagesArray = computed(() => {
  const totalPages = pages.value;
  const currentPage = page.value;


  if (totalPages <= 5) {
    return range(1, totalPages);
  }

  // Calculate the main range boundaries
  const startPages = range(1, BOUNDARY_COUNT);
  const endPages = range(totalPages - BOUNDARY_COUNT + 1, totalPages);

  // Calculate the sibling range boundaries
  const siblingStart = Math.max(
    Math.min(
      currentPage - SIBLING_COUNT,
      totalPages - SIBLING_COUNT * 2 - 1
    ),
    BOUNDARY_COUNT + 2
  );

  const siblingEnd = Math.min(
    Math.max(
      currentPage + SIBLING_COUNT,
      SIBLING_COUNT * 2 + 3
    ),
    endPages[0] - 2
  );

  const itemArray: (number | string)[] = [];

  itemArray.push(...startPages);

  if (siblingStart > BOUNDARY_COUNT + 2) {
    itemArray.push('...');
  } else if (BOUNDARY_COUNT + 1 < totalPages - BOUNDARY_COUNT) {
    itemArray.push(BOUNDARY_COUNT + 1);
  }

  itemArray.push(...range(siblingStart, siblingEnd));

  if (siblingEnd < totalPages - BOUNDARY_COUNT - 1) {
    itemArray.push('...');
  } else if (totalPages - BOUNDARY_COUNT > BOUNDARY_COUNT) {
    itemArray.push(totalPages - BOUNDARY_COUNT);
  }

  itemArray.push(...endPages);

  return itemArray;
});

const nextPage = () => {
  if (page.value < pages.value) page.value++;
};

const prevPage = () => {
  if (page.value > 1) page.value--;
};

const goToPage = (p: string | number) => {
  if (typeof p === 'number') {
    page.value = p;
  }
};
</script>