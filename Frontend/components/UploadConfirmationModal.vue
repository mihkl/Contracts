<template>
  <UModal>
    <UCard
      :ui="{
        ring: '',
        divide: 'divide-y divide-gray-100 dark:divide-gray-800',
      }"
    >
      <template #header>
        <p class="h-8">{{ template.name }}</p>
      </template>

      <TemplateFieldList :fields="template.fields" class="h-auto" />

      <template #footer>
        <UForm :schema="schema" :state="state" @submit="onSubmit">
          <UFormGroup label="Enter a name for your template">
            <UInput class="mb-3" v-model="state.name" type="text" />
            <UButton class="mr-3" @click="modal.close">Back</UButton>
            <UButton type="submit" @click="modal.close"
              >Save to my templates</UButton
            >
          </UFormGroup>
        </UForm>
        <p v-if="infoMessage" class="mt-3">{{ infoMessage }}</p>
      </template>
    </UCard>
  </UModal>
</template>

<script setup lang="ts">
import { object, string, type InferType } from "yup";
import TemplateFieldList from "./templates/TemplateFieldList.vue";
import type { FormSubmitEvent } from "#ui/types";

const auth = useAuth();
const modal = useModal();
const { serverResponse } = useTemplateStore();
const template = serverResponse!.template;
const infoMessage = serverResponse!.infoMessage;

const schema = object({
  name: string().required("Required"),
});

const state = reactive({
  name: undefined,
});

type Schema = InferType<typeof schema>;

async function onSubmit(event: FormSubmitEvent<Schema>) {
  await auth.fetchWithToken("/save", {
    method: "POST",
    body: JSON.stringify({ guid: serverResponse!.guid, name: state.name }),
  });

  navigateTo("/templates");
}
</script>
