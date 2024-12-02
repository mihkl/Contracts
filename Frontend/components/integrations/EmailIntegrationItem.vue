<template>
  <div :class="['p-4 border rounded-lg transition bg-white border-gray-300']">
    <h2 class="font-semibold text-xl mb-1">Email integration</h2>
    <p class="text-lg !font-light mb-4">Configure automatic email sending</p>
    <UToggle v-model="selected" class="mb-4" />
  </div>
  <div
    v-if="selected === true"
    :class="['p-4 border rounded-lg transition bg-white border-gray-300']"
  >
    <h1 class="font-bold text-xl mb-1">SMTP settings</h1>
    <p class="text-lg !font-light mb-4">Configure automatic email sending</p>
    <UForm :state="state" @submit="submit">
      <UFormGroup label="SMTP host" name="host" class="mb-4" :required="true">
        <UInput type="text" v-model="state.host" />
      </UFormGroup>
      <UFormGroup label="SMTP port" name="port" class="mb-4" :required="true">
        <UInput type="number" v-model="state.port" />
      </UFormGroup>
      <UFormGroup
        label="Username"
        name="username"
        class="mb-4"
        :required="true"
      >
        <UInput type="text" v-model="state.username" />
      </UFormGroup>
      <UFormGroup
        label="Password"
        name="password"
        class="mb-4"
        :required="true"
      >
        <UInput type="password" v-model="state.password" />
      </UFormGroup>
      <UFormGroup label="From email" name="email" class="mb-4" :required="true">
        <UInput type="email" v-model="state.fromEmail" />
      </UFormGroup>
    </UForm>
  </div>
  <UButton @click="submit" class="mr-10 bg-indigo-500 hover:bg-indigo-600"
    >Save</UButton
  >
</template>

<script setup lang="ts">
const selected = ref(false);
const auth = useAuth();
const smtpSettings = ref();

onMounted(async () => {
  const response = await auth.fetchWithToken("/settings/smtp");
  if (!response?.error || !response) {
    smtpSettings.value = response;

    if (response) {
      selected.value = true;
      state.host = response?.host;
      state.fromEmail = response?.fromEmail;
      state.port = response?.port;
      state.username = response?.username;
      state.password = response?.password;
    }
  }
});

const state = reactive({
  host: "",
  port: 0,
  username: "",
  password: "",
  fromEmail: "",
});

const submit = async () => {
  if (selected.value === false)
    await auth.fetchWithToken("/settings/smtp", {
      method: "DELETE",
    });
  else
    await auth.fetchWithToken("/settings/smtp", {
      method: "POST",
      body: JSON.stringify({
        ...state,
      }),
    });
};
</script>
