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
        :class="{ 'border-red-500': errors.notifyOnUploadContent }"
        placeholder="Write your email content here."
        rows="4"
      ></textarea>
      <p v-if="errors.notifyOnUploadContent" class="text-red-500 text-sm">
        {{ errors.notifyOnUploadContent }}
      </p>
      <div class="flex items-center mt-2">
        <input
          id="includeUploadAttachment"
          type="checkbox"
          v-model="state.applicantDocumentIsAttached"
          class="mr-2"
        />
        <label for="includeUploadAttachment" class="text-sm">Include contract as email attachment</label>
      </div>
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
        :class="{ 'border-red-500': errors.signatureNotificationEmail }"
        placeholder="Enter email that will receive the notification"
        class="w-full border rounded-lg p-2"
      />
      <p v-if="errors.signatureNotificationEmail" class="text-red-500 text-sm">
        {{ errors.signatureNotificationEmail }}
      </p>
    </div>
    <div v-if="sendFinalContractSelected" class="mb-4">
      <label for="notifyOnSignatureContent" class="block font-medium mb-2">
        Email Content
      </label>
      <textarea
        id="notifyOnSignatureContent"
        v-model="state.notifyOnSignatureContent"
        :class="{ 'border-red-500': errors.notifyOnSignatureContent }"
        class="w-full border rounded-lg p-2"
        placeholder="Write the email content for the signature notification."
        rows="4"
      ></textarea>
      <p v-if="errors.notifyOnSignatureContent" class="text-red-500 text-sm">
        {{ errors.notifyOnSignatureContent }}
      </p>
      <div class="flex items-center mt-2">
        <input
          id="includeSignatureAttachment"
          type="checkbox"
          v-model="state.notificationDocumentIsAttached"
          class="mr-2"
        />
        <label for="includeSignatureAttachment" class="text-sm">Include contract as email attachment</label>
      </div>
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
const toast = useToast();

const auth = useAuth();
const smtpSettings = ref();

const defaultUploadContent = "We are pleased to inform you that your document has been countersigned and the signing process is now complete. Thank you for your trust and cooperation. If you have any questions, please don’t hesitate to contact us.";
const defaultNotificationEmail = "";
const defaultNotificationContent = "We would like to notify you that a client has signed the document. It is now ready for your countersignature to finalize the process. Please review and countersign the document at your earliest convenience";

const state = reactive({
  host: "",
  port: 0,
  username: "",
  password: "",
  fromEmail: "",
  notifyOnUploadContent: "",
  signatureNotificationEmail: "",
  notifyOnSignatureContent: "",
  applicantDocumentIsAttached: false,
  notificationDocumentIsAttached: false,
});

const errors = reactive({
  notifyOnUploadContent: "",
  signatureNotificationEmail: "",
  notifyOnSignatureContent: "",
});

const handleContractUploadToggle = () => {
  if (!notifyOnContractUploadSelected.value) {
    state.notifyOnUploadContent = "";
    state.applicantDocumentIsAttached = false;
    errors.notifyOnUploadContent = ""; 
  } else {
    state.notifyOnUploadContent = defaultUploadContent;
  }
};

const handleSignatureToggle = () => {
  if (!sendFinalContractSelected.value) {
    state.signatureNotificationEmail = "";
    state.notifyOnSignatureContent = "";
    state.notificationDocumentIsAttached = false;
    errors.signatureNotificationEmail = "";
    errors.notifyOnSignatureContent = ""; 
  } else {
    state.signatureNotificationEmail = defaultNotificationEmail;
    state.notifyOnSignatureContent = defaultNotificationContent;
  }
};

const validate = () => {
  let isValid = true;

  // Validate "Automatically send final contract to applicant"
  if (notifyOnContractUploadSelected.value && !state.notifyOnUploadContent) {
    errors.notifyOnUploadContent = "Email content is required.";
    isValid = false;
  } else {
    errors.notifyOnUploadContent = "";
  }

  // Validate "Notify on new contract signature"
  if (sendFinalContractSelected.value) {
    if (!state.signatureNotificationEmail) {
      errors.signatureNotificationEmail = "Notification email is required.";
      isValid = false;
    } else {
      errors.signatureNotificationEmail = "";
    }

    if (!state.notifyOnSignatureContent) {
      errors.notifyOnSignatureContent = "Notification email content is required.";
      isValid = false;
    } else {
      errors.notifyOnSignatureContent = "";
    }
  }

  return isValid;
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
      state.applicantDocumentIsAttached = response?.applicantDocumentIsAttached || false;
      state.notificationDocumentIsAttached = response?.notificationDocumentIsAttached || false;
    }
  }
});

const submit = async () => {
  if (!validate()) {
    return;
  }
  toast.add({
    title: "Saving!",
    description: "Saving new settings."
  })

  const payload: Partial<typeof state> = { ...state };

    payload.notifyOnUploadContent = notifyOnContractUploadSelected.value
    ? state.notifyOnUploadContent
    : "";

  payload.signatureNotificationEmail = sendFinalContractSelected.value
    ? state.signatureNotificationEmail
    : "";
  payload.notifyOnSignatureContent = sendFinalContractSelected.value
    ? state.notifyOnSignatureContent
    : "";

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
