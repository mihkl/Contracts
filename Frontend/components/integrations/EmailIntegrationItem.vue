<template>
  <div :class="['p-4 border rounded-lg transition border-gray-300']">
    <h2 class="font-semibold text-xl mb-1">Email integration</h2>
    <p class="text-lg !font-light mb-4">Configure automatic email sending</p>
    <UToggle v-model="selected" class="mb-4" />
    <UButton
      @click="submit"
      class="mr-10 bg-indigo-500 hover:bg-indigo-600 block"
      >Save settings</UButton
    >
  </div>
  <div
    v-if="selected === true"
    :class="['p-4 border rounded-lg transition border-gray-300']"
  >
    <div class="flex flex-row gap-4 items-center mb-4">
      <UToggle v-model="notifyOnContractUploadSelected" @change="handleContractUploadToggle" />
      <p>Automatically send final contract to applicant.</p>
    </div>
    <div v-if="notifyOnContractUploadSelected" class="mb-4">
      <label for="notifyOnUploadContent" class="block font-medium mb-2">Email Content</label>
      <textarea
        id="notifyOnUploadContent"
        v-model="state.notifyOnUploadContent"
        class="w-full border rounded-lg p-2"
        placeholder="Write your email content here..."
        rows="4"
      ></textarea>
    </div>
    <div class="flex flex-row gap-4 items-center mb-4">
      <UToggle v-model="sendFinalContractSelected" @change="handleSignatureToggle" />
      <p>Notify on new contract signature.</p>
    </div>
    <div v-if="sendFinalContractSelected" class="mb-4">
      <label for="signatureEmail" class="block font-medium mb-2">Notification Receiver</label>
      <UInput
        id="signatureEmail"
        type="email"
        v-model="state.signatureNotificationEmail"
        placeholder="Enter email for signature notifications"
        class="w-full border rounded-lg p-2"
      />
    </div>
    <div v-if="sendFinalContractSelected" class="mb-4">
      <label for="notifyOnSignatureContent" class="block font-medium mb-2">
        Email Content
      </label>
      <textarea
        id="notifyOnSignatureContent"
        v-model="state.notifyOnSignatureContent"
        class="w-full border rounded-lg p-2"
        placeholder="Write the email content for the signature notification..."
        rows="4"
      ></textarea>
    </div>
  </div>
  <div
    v-if="selected === true"
    :class="['p-4 border rounded-lg transition border-gray-300']"
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
        class="mb-4" :required="true"
      >
        <UInput type="password" v-model="state.password" />
      </UFormGroup>
      <UFormGroup label="From email" name="email" class="mb-4" :required="true">
        <UInput type="email" v-model="state.fromEmail" />
      </UFormGroup>
    </UForm>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from "vue";

const selected = ref(false);
const notifyOnContractUploadSelected = ref(false);
const sendFinalContractSelected = ref(false);

const auth = useAuth();
const smtpSettings = ref();

const defaultUploadContent = "Example upload text";
const defaultNotificationEmail = "";
const defaultNotificationContent = "Example notification text";

const state = reactive({
  host: "",
  port: 0,
  username: "",
  password: "",
  fromEmail: "",
  notifyOnUploadContent: "",
  signatureNotificationEmail: "",
  notifyOnSignatureContent: "",
});

const handleContractUploadToggle = () => {
  if (!notifyOnContractUploadSelected.value) {
    state.notifyOnUploadContent = "";
  } else {
    state.notifyOnUploadContent = defaultUploadContent;
  }
};

const handleSignatureToggle = () => {
  if (!sendFinalContractSelected.value) {
    state.signatureNotificationEmail = "";
    state.notifyOnSignatureContent = "";
  } else {
    state.signatureNotificationEmail = defaultNotificationEmail;
    state.notifyOnSignatureContent = defaultNotificationContent;
  }
};

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
      state.notifyOnUploadContent = response?.notifyOnUploadContent || "";
      state.signatureNotificationEmail = response?.signatureNotificationEmail || "";
      state.notifyOnSignatureContent = response?.notifyOnSignatureContent || "";
    }
  }
});

const submit = async () => {
  // Create a Partial payload to allow optional properties
  const payload: Partial<typeof state> = { ...state };

  if (!notifyOnContractUploadSelected.value) {
    payload.notifyOnUploadContent = undefined; // Set as undefined instead of using delete
  }

  if (!sendFinalContractSelected.value) {
    payload.signatureNotificationEmail = undefined; // Set as undefined instead of using delete
    payload.notifyOnSignatureContent = undefined;
  }

  if (selected.value === false) {
    await auth.fetchWithToken("/settings/smtp", {
      method: "DELETE",
    });
  } else {
    await auth.fetchWithToken("/settings/smtp", {
      method: "POST",
      body: JSON.stringify(payload),
    });
  }
};

</script>
