<template>
  <UForm class="space-y-4">
    <UFormGroup
      v-for="(field, index) in contractFields?.fields"
      :label="field.name"
      name="contractName"
      required
      :key="index"
    >
      <UInput type="text" />
    </UFormGroup>
  </UForm>
</template>

<script setup lang="ts">
import { useRoute } from "nuxt/app";

const route = useRoute();
const api = useApi();

const contractFields = ref<{
  fields: { name: string; type: string }[];
}>();

const id = route.params?.id;
const signature = route.query?.signature;
const validFrom = route.query?.validFrom;
const validUntil = route.query?.validUntil;

const fields = await api.customFetch<{
  fields: { name: string; type: string }[];
}>(
  `/contracts/${id}?signature=${signature}&validFrom=${validFrom}&validUntil=${validUntil}`,
  {
    method: "GET",
  }
);

contractFields.value = fields;
</script>
