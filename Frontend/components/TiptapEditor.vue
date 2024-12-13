<script setup>
import { useEditor, EditorContent } from '@tiptap/vue-3';
import StarterKit from '@tiptap/starter-kit';

// Define props and emits for v-model
const props = defineProps({
  modelValue: {
    type: String,
    required: false,
    default: '',
  },
});

const emits = defineEmits(['update:modelValue']);

// Initialize editor
const editor = useEditor({
  content: props.modelValue, // Set initial content
  extensions: [StarterKit],
  onUpdate: ({ editor }) => {
    emits('update:modelValue', editor.getHTML()); // Emit updated content
  },
});

// Destroy editor on unmount
onBeforeUnmount(() => {
  if (editor) {
    editor.destroy();
  }
});
</script>

<template>
  <div v-if="editor">
    <EditorContent :editor="editor" />
  </div>
</template>
