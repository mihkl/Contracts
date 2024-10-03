<template>
    <div class="flex justify-center">
      <div class="w-full max-w-5xl flex">
        <div class="w-1/4 space-y-4">
          <h1 class="text-3xl font-semibold my-6">My Templates</h1>
          <div
            v-for="(template, index) in templates"
            :key="index"
            @mouseover="highlighted = index"
            @mouseleave="highlighted = null"
            :class="[
              'p-4 border rounded-lg cursor-pointer transition',
              highlighted === index ? 'bg-gray-200 border-purple-500' : 'bg-white border-gray-300'
            ]"
          >
            {{ template }}
          </div>
          <button
            class="mt-6 px-6 py-2 bg-purple-500 text-white rounded-lg hover:bg-purple-600 transition"
            @click="generateLink"
          >
            Generate link
          </button>
        </div>
        <div class="w-3/4"></div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
import { errorMessages } from 'vue/compiler-sfc';
  
  const templates = ref(['Template 1', 'Template 2', 'Template 3', 'Template 4']);
  const highlighted = ref(null);
  
  const generateLink = () => {
    return alert("Get pranked!")
  };
  </script>
  
  <style scoped>
  /* You can add any custom styles if needed */
  </style>
  