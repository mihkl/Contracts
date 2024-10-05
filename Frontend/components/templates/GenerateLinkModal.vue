<template>
  <UModal prevent-close>
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
            @click="modal.close()"
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
</template>

<script setup lang="ts">
import type { FormError } from "#ui/types";

const modal = useModal();

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
