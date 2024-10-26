<template>
  <UForm
    class="space-y-4"
    v-if="Object.keys(formState)?.length > 0"
    :state="formState"
    @submit="onSubmit"
  >
    <UFormGroup
      v-for="(field, index) in contractFields?.fields"
      :label="field.name"
      name="contractName"
      required
      :key="index"
    >
      <UInput type="text" v-model="formState[field.name]" />
    </UFormGroup>
    <UButton type="submit">Submit</UButton>
  </UForm>
</template>

<script setup lang="ts">
import { useRoute } from "nuxt/app";

const route = useRoute();
const api = useApi();

const contractFields = ref<{
  fields: { name: string; type: string }[];
}>();

const formState = reactive<Record<string, any>>({});

const id = route.params?.id;
const signature = route.query?.signature;
const validFrom = route.query?.validFrom;
const validUntil = route.query?.validUntil;

onMounted(async () => {
  const fields = await api.fetchWithErrorHandling<{
    fields: { name: string; type: string }[];
  }>(
    `/contracts/${id}?signature=${encodeURIComponent(
      typeof signature === "string" && signature
    )}&validFrom=${validFrom}&validUntil=${validUntil}`,
    { method: "GET" }
  );

  contractFields.value = fields;

  fields.fields.forEach((field: { name: string }) => {
    formState[field.name] = null;
  });
});

async function onSubmit() {
  const response = await api.fetchWithErrorHandling(
    `/contracts/${id}/generate-pdf`,
    {
      method: "POST",
      body: JSON.stringify({
        replacements: [
          ...Object.keys(formState).map((key) => ({
            name: key,
            value: formState[key],
          })),
        ],
      }),
    }
  );
}
</script>
