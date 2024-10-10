<script setup>
import { ref } from 'vue'
import { VuePDF, usePDF } from '@tato30/vue-pdf'

const scale = ref(1)
const { pdf } = usePDF('https://raw.githubusercontent.com/mozilla/pdf.js/ba2edeae/web/compressed.tracemonkey-pldi-09.pdf')
</script>

<template>
  <div class="flex justify-end p-20">
    <VuePDF :pdf="pdf" :scale="0.5"  />
  </div>
</template>
