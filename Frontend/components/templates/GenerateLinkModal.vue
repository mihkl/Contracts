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
        <UFormGroup label="Name" name="contractName" required>
          <UInput type="text" v-model="state.contractName" />
        </UFormGroup>
        <UFormGroup label="Valid from (optional)" name="validFrom">
          <UInput type="date" v-model="state.validFrom" />
        </UFormGroup>
        <UFormGroup label="Valid until (optional)" name="validUntil">
          <UInput type="date" v-model="state.validUntil" />
        </UFormGroup>
        <UButton type="submit" color="indigo"> Submit </UButton>
      </UForm>
    </UCard>
  </UModal>
</template>

<script setup lang="ts">
import type { FormError } from "#ui/types";

const props = defineProps({
  templateId: Number,
});

const modal = useModal();
const toast = useToast();

const state = reactive({
  validFrom: undefined,
  validUntil: undefined,
  contractName: "",
});

const validate = (state: {
  validFrom: string;
  validUntil: string;
  contractName: string;
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

  if (state.contractName.trim() === "")
    errors.push({ path: "contractName", message: "Name is required." });

  return errors;
};

const convertStringToDate = (dateString: string): Date => {
  const date = new Date(dateString);
  date.setHours(0, 0, 0);
  return date;
};

const onSubmit = async () => {
  const api = useApi();
  const auth = useAuth();

  const response: { url?: string } = await auth.fetchWithToken(
    `/contracts/${props.templateId}/url`,
    {
      method: "POST",
      body: JSON.stringify({
        Name: state.contractName,
        validFrom: state.validFrom,
        validUntil: state.validUntil,
      }),
    }
  );

  if (response?.url) {
    modal.close();
    navigator.clipboard.writeText(window.location.origin + "/" + response.url);
    navigateTo("/contracts");
    toast.add({
      title: "Success!",
      description: "The link has been copied to your clipboard.",
    });
  }
};
</script>
