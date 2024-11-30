<template>
  <div :class="['p-4 border rounded-lg transition border-gray-300']">
    <h2 class="font-semibold text-xl mb-1">Email integration</h2>
    <p class="text-lg !font-light mb-4">Configure automatic email sending</p>
    <UToggle v-model="selected" class="mb-4" />
  </div>
  <div
    v-if="selected === true"
    :class="['p-4 border rounded-lg transition border-gray-300']"
  >
    <h1 class="font-bold text-xl mb-1">SMTP settings</h1>
    <p class="text-lg !font-light mb-4">Configure automatic email sending</p>
    <UForm :state="state">
      <UFormGroup label="SMTP host" name="host" class="mb-4" :required="true">
        <UInput type="text" />
      </UFormGroup>
      <UFormGroup label="SMTP port" name="port" class="mb-4" :required="true">
        <UInput type="number" />
      </UFormGroup>
      <UFormGroup
        label="Username"
        name="username"
        class="mb-4"
        :required="true"
      >
        <UInput type="text" />
      </UFormGroup>
      <UFormGroup
        label="Password"
        name="password"
        class="mb-4"
        :required="true"
      >
        <UInput type="password" />
      </UFormGroup>
      <UFormGroup label="From email" name="email" class="mb-4" :required="true">
        <UInput type="email" />
      </UFormGroup>
      <UButton type="submit" class="mr-10 bg-indigo-500 hover:bg-indigo-600"
        >Submit</UButton
      >
    </UForm>
  </div>
</template>

<script setup lang="ts">
const selected = ref(false);

const state = reactive({
  host: "",
  port: 0,
  username: "",
  password: "",
  email: "",
});
</script>
