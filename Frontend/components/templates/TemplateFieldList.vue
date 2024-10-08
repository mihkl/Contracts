<template>
<div v-if="fields && fields.length > 0" class="field-list">
    <h2>Contract Fields</h2>
    <div class="mt-3">
    <div
        v-for="(field, index) in fields"
        :key="index"
    >
        <h3 class="mt-3">{{ field.name }}</h3>
        <p><strong>Type:</strong> {{ field.type }}</p>
    </div>
    </div>
</div>
<div v-else>
    <p>No fields available to display.</p>
</div>
</template>

<script setup lang="ts">


defineProps({
    fields: {
        type: Array as () => TemplateField[],
        default: () => []
    }
})
</script>