<script setup>
import { useEditor, EditorContent } from '@tiptap/vue-3';
import StarterKit from '@tiptap/starter-kit';


const props = defineProps({
  modelValue: {
    type: String,
    required: false,
    default: '',
  },
});

const emits = defineEmits(['update:modelValue']);

const editor = useEditor({
  content: props.modelValue,
  extensions: [StarterKit],
  onUpdate: ({ editor }) => {
    emits('update:modelValue', editor.getHTML());
  },
});

onBeforeUnmount(() => {
  if (editor) {
    editor.destroy();
  }
});
</script>

<template>
  <div>
    <div v-if="editor" class="buttons text-gray-700 flex items-center flex-wrap gap-x-2 border-t border-l border-r border-gray-400 p-2">
      <button
        @click="editor.chain().focus().toggleBold().run()"
        :disabled="!editor.can().chain().focus().toggleBold().run()"
        :class="[
          'px-2 py-1 rounded text-black font-semibold',
          editor.isActive('bold') ? 'bg-indigo-500' : 'hover:bg-indigo-300',
          !editor.can().chain().focus().toggleBold().run() ? 'cursor-not-allowed opacity-50' : 'cursor-pointer'
        ]"
      >
        Bold
      </button>
      <button
        @click="editor.chain().focus().toggleItalic().run()"
        :disabled="!editor.can().chain().focus().toggleItalic().run()"
        :class="[
          'px-2 py-1 rounded text-black font-semibold',
          editor.isActive('italic') ? 'bg-indigo-500' : 'hover:bg-indigo-300',
          !editor.can().chain().focus().toggleItalic().run() ? 'cursor-not-allowed opacity-50' : 'cursor-pointer'
        ]"
      >
        Italic
      </button>
    </div>
    <TiptapEditorContent :editor="editor" />
  </div>
</template>
