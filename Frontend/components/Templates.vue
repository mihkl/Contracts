<template>
  <div class="w-full max-w-5xl">
    <div class="space-y-4">
      <h1 class="text-3xl font-semibold my-6">My Templates</h1>
      <TemplateItem
        v-for="(template, index) in templates"
        :key="index"
        :template="template"
        @open-modal="isOpen = true"
      />

      <UModal v-model="isOpen" prevent-close>
        <UCard
          :ui="{
            ring: '',
            divide: 'divide-y divide-gray-100 dark:divide-gray-800',
          }"
        >
          <template #header>
            <div class="flex items-center justify-between">
              <h3
                class="text-base font-semibold leading-6 text-gray-900 dark:text-white"
              >
                Generate link
              </h3>

              <UButton
                color="gray"
                variant="ghost"
                icon="i-heroicons-x-mark-20-solid"
                class="-my-1"
                @click="isOpen = false"
              />
            </div>
          </template>
          <UForm
            :validate="validate"
            :state="state"
            class="space-y-4"
            @submit="onSubmit"
          >
            <UFormGroup label="Valid from (optional)" name="validFrom">
              <UInput type="date" v-model="state.validFrom" />
            </UFormGroup>
            <UFormGroup label="Valid until (optional)" name="validUntil">
              <UInput type="date" v-model="state.validUntil" />
            </UFormGroup>
            <UButton type="submit"> Submit </UButton>
          </UForm>
        </UCard>
      </UModal>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import TemplateItem from "./TemplateItem/TemplateItem.vue";
import type { FormError } from "#ui/types";

const templates = ref([
  { name: "Template 1", id: 1 },
  { name: "Template 2", id: 2 },
  { name: "Template 3", id: 3 },
  { name: "Template 4", id: 4 },
]);
const isOpen = ref(false);

const state = reactive({
  validFrom: undefined,
  validUntil: undefined,
});

const validate = (state: {
  validFrom: string;
  validUntil: string;
}): FormError[] => {
  const errors: FormError[] = [];
  if (
    convertStringToDate(state.validUntil) <
    convertStringToDate(new Date().toString())
  )
    errors.push({
      path: "validUntil",
      message: "Valid until cannot be in the past.",
    });

  console.log(errors);
  return errors;
};

const convertStringToDate = (dateString: string): Date => {
  const date = new Date(dateString);
  date.setHours(0, 0, 0);
  return date;
};

const onSubmit = () => {
  console.log("SUbmit");
};
</script>

<style scoped>
/* You can add any custom styles if needed */
</style>
