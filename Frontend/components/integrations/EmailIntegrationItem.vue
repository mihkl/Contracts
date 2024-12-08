<template>
  <!-- Email Integration Section -->
  <div :class="['p-4 border rounded-lg transition border-gray-300']">
    <h2 class="font-semibold text-xl mb-1">Email integration</h2>
    <p class="text-lg !font-light mb-4">Configure automatic email sending</p>
    <UToggle v-model="selected" class="mb-4" />
    <UButton @click="submit" class="mr-10 bg-indigo-500 hover:bg-indigo-600 block">
      Save settings
    </UButton>
  </div>

  <!-- Email Configuration Options -->
  <div v-if="selected" :class="['p-4 border rounded-lg transition border-gray-300']">
    <!-- Automatically Send Final Contract -->
    <div class="flex flex-row gap-4 items-center mb-4">
      <UToggle v-model="notifyOnContractUploadSelected" @change="handleContractUploadToggle" />
      <p>Automatically send final contract to applicant.</p>
    </div>
    <div v-if="notifyOnContractUploadSelected" class="mb-4">
      <label for="notifyOnUploadSubject" class="block font-medium mb-2">Email Subject</label>
      <textarea
        id="notifyOnUploadSubject"
        v-model="state.notifyOnUploadSubject"
        class="w-full border rounded-lg p-2"
        :class="{ 'border-red-500': errors.notifyOnUploadSubject }"
        placeholder="Enter email subject for final contract"
        rows="2"
      ></textarea>
      <p v-if="errors.notifyOnUploadSubject" class="text-red-500 text-sm">
        {{ errors.notifyOnUploadSubject }}
      </p>
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
          v-model="state.documentIsAttached"
          class="mr-2"
        />
        <label for="includeUploadAttachment" class="text-sm">Include contract as email attachment</label>
      </div>
    </div>

    <!-- Notify on New Contract Signature -->
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
        placeholder="Enter email to receive the notification"
        class="w-full border rounded-lg p-2"
      />
      <p v-if="errors.signatureNotificationEmail" class="text-red-500 text-sm">
        {{ errors.signatureNotificationEmail }}
      </p>
      <label for="notifyOnSignatureSubject" class="block font-medium mb-2">Email Subject</label>
      <textarea
        id="notifyOnSignatureSubject"
        v-model="state.notifyOnSignatureSubject"
        class="w-full border rounded-lg p-2"
        :class="{ 'border-red-500': errors.notifyOnSignatureSubject }"
        placeholder="Enter email subject for signature notification"
        rows="2"
      ></textarea>
      <p v-if="errors.notifyOnSignatureSubject" class="text-red-500 text-sm">
        {{ errors.notifyOnSignatureSubject }}
      </p>
      <label for="notifyOnSignatureContent" class="block font-medium mb-2 mt-4">
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

  <!-- SMTP Settings -->
  <div v-if="selected" :class="['p-4 border rounded-lg transition border-gray-300']">
    <h1 class="font-bold text-xl mb-1">SMTP settings</h1>
    <p class="text-lg !font-light mb-4">Configure automatic email sending</p>
    <UForm :state="state" @submit="submit">
      <UFormGroup label="SMTP host" name="host" class="mb-4" :required="true">
        <UInput type="text" v-model="state.host" />
      </UFormGroup>
      <UFormGroup label="SMTP port" name="port" class="mb-4" :required="true">
        <UInput type="number" v-model="state.port" />
      </UFormGroup>
      <UFormGroup label="Username" name="username" class="mb-4" :required="true">
        <UInput type="text" v-model="state.username" />
      </UFormGroup>
      <UFormGroup label="Password" name="password" class="mb-4" :required="true">
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

const defaultUploadSubject = "Subject: Your Document Has Been Countersigned"
const defaultUploadContent = `We are pleased to inform you that your document has been successfully countersigned, and the signing process is now complete. 

If you have any further questions or need assistance, please do not hesitate to contact us.

Thank you for trusting us with your business.

Best regards,  
Y`;

const defaultNotificationSubject = "Subject: Client Signed the Document - Your Action Required"
const defaultNotificationContent = `This email is to notify you that the client has signed the document. The next step is for you to review and countersign the document to finalize the process.

If you have any questions or need further details, please reach out to our team.

Best regards,  
Y`;


// State object
const state = reactive({
  host: "",
  port: 0,
  username: "",
  password: "",
  fromEmail: "",
  notifyOnUploadSubject: "",
  notifyOnUploadContent: "",
  notifyOnSignatureSubject: "",
  signatureNotificationEmail: "",
  notifyOnSignatureContent: "",
  documentIsAttached: false,
  notificationDocumentIsAttached: false,
});

// Error tracking
const errors = reactive({
  notifyOnUploadSubject: "",
  notifyOnUploadContent: "",
  notifyOnSignatureSubject: "",
  signatureNotificationEmail: "",
  notifyOnSignatureContent: "",
});

// Toggles
const handleContractUploadToggle = () => {
  if (notifyOnContractUploadSelected.value) {
    if (!state.notifyOnUploadContent) {
      state.notifyOnUploadSubject = defaultUploadSubject;
      state.notifyOnUploadContent = defaultUploadContent;
    }
  } else {
    state.notifyOnUploadSubject = "";
    state.notifyOnUploadContent = "";
    state.documentIsAttached = false;
    errors.notifyOnUploadSubject = "";
    errors.notifyOnUploadContent = "";
  }
};

const handleSignatureToggle = () => {
  if (sendFinalContractSelected.value) {
    if (!state.notifyOnSignatureContent) {
      state.notifyOnSignatureSubject = defaultNotificationSubject;
      state.notifyOnSignatureContent = defaultNotificationContent;
    }
  } else {
    state.notifyOnSignatureSubject = "";
    state.signatureNotificationEmail = "";
    state.notifyOnSignatureContent = "";
    state.notificationDocumentIsAttached = false;
    errors.notifyOnSignatureSubject = "";
    errors.signatureNotificationEmail = "";
    errors.notifyOnSignatureContent = "";
  }
};

// Validation
const validate = () => {
  let isValid = true;
  if (notifyOnContractUploadSelected.value) {
    if (!state.notifyOnUploadSubject) {
      errors.notifyOnUploadSubject = "Email subject is required.";
      isValid = false;
    }
    if (!state.notifyOnUploadContent) {
      errors.notifyOnUploadContent = "Email content is required.";
      isValid = false;
    }
  }
  if (sendFinalContractSelected.value) {
    if (!state.notifyOnSignatureSubject) {
      errors.notifyOnSignatureSubject = "Email subject is required.";
      isValid = false;
    }
    if (!state.signatureNotificationEmail) {
      errors.signatureNotificationEmail = "Notification email is required.";
      isValid = false;
    }
    if (!state.notifyOnSignatureContent) {
      errors.notifyOnSignatureContent = "Notification content is required.";
      isValid = false;
    }
  }
  return isValid;
};

// Fetch initial settings
onMounted(async () => {
  const response = await auth.fetchWithToken("/settings/smtp");
  if (!response?.error) {
    Object.assign(state, response);

    // Initialize toggles based on fetched data
    notifyOnContractUploadSelected.value = !!response.notifyOnUploadContent;
    sendFinalContractSelected.value = !!response.notifyOnSignatureContent;

    selected.value = true;
  }
});

// Submit settings
const submit = async () => {
  if (!validate()) return;
  toast.add({ title: "Saving!", description: "Saving new settings." });

  const payload = {
    ...state,
    notifyOnUploadContent: notifyOnContractUploadSelected.value ? state.notifyOnUploadContent : "",
    notifyOnUploadSubject: notifyOnContractUploadSelected.value ? state.notifyOnUploadSubject : "",
    signatureNotificationEmail: sendFinalContractSelected.value ? state.signatureNotificationEmail : "",
    notifyOnSignatureContent: sendFinalContractSelected.value ? state.notifyOnSignatureContent : "",
    notifyOnSignatureSubject: sendFinalContractSelected.value ? state.notifyOnSignatureSubject : "",
    documentIsAttached: notifyOnContractUploadSelected.value ? state.documentIsAttached : false,
    notificationDocumentIsAttached: sendFinalContractSelected.value ? state.notificationDocumentIsAttached : false,
  };

  const method = selected.value ? "POST" : "DELETE";
  await auth.fetchWithToken("/settings/smtp", { method, body: JSON.stringify(payload) });
};
</script>
