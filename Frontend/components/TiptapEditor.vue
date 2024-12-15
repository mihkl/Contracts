<script setup>
import { useEditor, EditorContent } from '@tiptap/vue-3';
import StarterKit from '@tiptap/starter-kit';
import HardBreak from '@tiptap/extension-hard-break'



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
  extensions: [StarterKit, HardBreak.extend({
    addKeyboardShortcuts() {
      return {
        Enter: () => this.editor.commands.setHardBreak(),
      };
    },
  }),
],
  onUpdate: ({ editor }) => {
    emits('update:modelValue', editor.getHTML());
  },
});

onBeforeUnmount(() => {
  unref(editor).destroy();
});
</script>

<template>
  <div>
    <div v-if="editor" class="buttons text-gray-700 flex items-center flex-wrap gap-2 border-r border-l border-t border-gray-400 p-2">
      <button
        @click="editor.chain().focus().toggleBold().run()"
        :disabled="!editor.can().chain().focus().toggleBold().run()"
        :class="[ 
          'px-2 py-1 rounded text-black font-semibold',
          editor.isActive('bold') ? 'bg-indigo-500 text-white' : 'hover:bg-indigo-300',
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
          editor.isActive('italic') ? 'bg-indigo-500 text-white' : 'hover:bg-indigo-300',
          !editor.can().chain().focus().toggleItalic().run() ? 'cursor-not-allowed opacity-50' : 'cursor-pointer'
        ]"
      >
        Italic
      </button>

      <button
        @click="editor.chain().focus().toggleCode().run()"
        :disabled="!editor.can().chain().focus().toggleCode().run()"
        :class="[ 
          'px-2 py-1 rounded text-black font-semibold',
          editor.isActive('code') ? 'bg-indigo-500 text-white' : 'hover:bg-indigo-300',
          !editor.can().chain().focus().toggleCode().run() ? 'cursor-not-allowed opacity-50' : 'cursor-pointer'
        ]"
      >
        Code
      </button>
      <button
        @click="editor.chain().focus().toggleHeading({ level: 1 }).run()"
        :class="[ 
          'px-2 py-1 rounded text-black font-semibold',
          editor.isActive('heading', { level: 1 }) ? 'bg-indigo-500 text-white' : 'hover:bg-indigo-300 cursor-pointer'
        ]"
      >
        H1
      </button>

      <button
        @click="editor.chain().focus().toggleHeading({ level: 2 }).run()"
        :class="[ 
          'px-2 py-1 rounded text-black font-semibold',
          editor.isActive('heading', { level: 2 }) ? 'bg-indigo-500 text-white' : 'hover:bg-indigo-300 cursor-pointer'
        ]"
      >
        H2
      </button>

      <button
        @click="editor.chain().focus().toggleBulletList().run()"
        :class="[ 
          'px-2 py-1 rounded text-black font-semibold',
          editor.isActive('bulletList') ? 'bg-indigo-500 text-white' : 'hover:bg-indigo-300 cursor-pointer'
        ]"
      >
        Bullet List
      </button>

      <button
        @click="editor.chain().focus().toggleOrderedList().run()"
        :class="[ 
          'px-2 py-1 rounded text-black font-semibold',
          editor.isActive('orderedList') ? 'bg-indigo-500 text-white' : 'hover:bg-indigo-300 cursor-pointer'
        ]"
      >
        Ordered List
      </button>
      <button
        @click="editor.chain().focus().undo().run()"
        :disabled="!editor.can().chain().focus().undo().run()"
        :class="[ 
          'px-2 py-1 rounded text-black font-semibold hover:bg-indigo-300',
          !editor.can().chain().focus().undo().run() ? 'cursor-not-allowed opacity-50' : 'cursor-pointer'
        ]"
      >
        Undo
      </button>

      <button
        @click="editor.chain().focus().redo().run()"
        :disabled="!editor.can().chain().focus().redo().run()"
        :class="[ 
          'px-2 py-1 rounded text-black font-semibold hover:bg-indigo-300',
          !editor.can().chain().focus().redo().run() ? 'cursor-not-allowed opacity-50' : 'cursor-pointer'
        ]"
      >
        Redo
      </button>
    </div v-if="editor">
      <TiptapEditorContent :editor="editor" class="border border-gray-400 p-2"/>
    </div>
  </template>

<style>
.tiptap h1 {
  font-size: 24px;
  font-weight: bold;
}
.tiptap h2 {
  font-size: 18px;
  font-weight: bold;
}
.tiptap ul {
  list-style-type: disc;
  margin-left: 1.5rem;  
  padding-left: 1rem;   
}


.tiptap ol {
  list-style-type: decimal; 
  margin-left: 1.5rem;
  padding-left: 1rem;
}


.tiptap li {
  margin-bottom: 0.5rem; 
  font-size: 16px;       
  line-height: 1.5;      
}

</style>