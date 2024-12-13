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
        <UFormGroup label="Email Subject" name="notifyOnUploadSubject" class="mb-4" :required="true">
          <UTextarea
            id="notifyOnUploadSubject"
            v-model="state.notifyOnUploadSubject"
            class="w-full border rounded-lg p-2"
            :class="{ 'border-red-500': errors.notifyOnUploadSubject }"
            placeholder="Enter email subject for final contract"
            
          ></UTextarea>
          <p v-if="errors.notifyOnUploadSubject" class="text-red-500 text-sm">
            {{ errors.notifyOnUploadSubject }}
          </p>
        </UFormGroup>

        <UFormGroup label="Email Content" name="notifyOnUploadContent" class="mb-4" :required="true">
          <UTextarea
            id="notifyOnUploadContent"
            v-model="state.notifyOnUploadContent"
            class="w-full border rounded-lg p-2"
            :class="{ 'border-red-500': errors.notifyOnUploadContent }"
            placeholder="Write your email content here."
            
          ></UTextarea>
          <p v-if="errors.notifyOnUploadContent" class="text-red-500 text-sm">
            {{ errors.notifyOnUploadContent }}
          </p>
        </UFormGroup>

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
        <UFormGroup label="Notification Receiver" name="signatureNotificationEmail" class="mb-4" :required="true">
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
        </UFormGroup>
        <UFormGroup label="Email Subject" name="notifyOnSignatureSubject" class="mb-4" :required="true">
          <client-only>
            <TiptapEditor v-model="state.notifyOnSignatureSubject" />
          </client-only>
          <p v-if="errors.notifyOnSignatureSubject" class="text-red-500 text-sm">
            {{ errors.notifyOnSignatureSubject }}
          </p>
        </UFormGroup>
        <UFormGroup label="Email Content" name="notifyOnSignatureContent" class="mb-4" :required="true">
          <UTextarea
            id="notifyOnSignatureContent"
            v-model="state.notifyOnSignatureContent"
            :class="{ 'border-red-500': errors.notifyOnSignatureContent }"
            class="w-full border rounded-lg p-2"
            placeholder="Write the email content for the signature notification."
          ></UTextarea>
          <p v-if="errors.notifyOnSignatureContent" class="text-red-500 text-sm">
            {{ errors.notifyOnSignatureContent }}
          </p>
        </UFormGroup>
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
    <UFormGroup label="SMTP host" name="host" class="mb-4" :required="true">
    <UInput type="text" v-model="state.host" :class="{ 'border-red-500': errors.host }" />
    <p v-if="errors.host" class="text-red-500 text-sm">{{ errors.host }}</p>
  </UFormGroup>
  <UFormGroup label="SMTP port" name="port" class="mb-4" :required="true">
    <UInput type="number" v-model="state.port" :class="{ 'border-red-500': errors.port }" />
    <p v-if="errors.port" class="text-red-500 text-sm">{{ errors.port }}</p>
  </UFormGroup>
  <UFormGroup label="Username" name="username" class="mb-4">
    <UInput type="text" v-model="state.username" />
  </UFormGroup>
  <UFormGroup label="Password" name="password" class="mb-4">
    <UInput type="password" v-model="state.password" />
  </UFormGroup>
  <UFormGroup label="From email" name="email" class="mb-4" :required="true">
    <UInput type="email" v-model="state.fromEmail" :class="{ 'border-red-500': errors.fromEmail }" />
    <p v-if="errors.fromEmail" class="text-red-500 text-sm">{{ errors.fromEmail }}</p>
  </UFormGroup>

  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from "vue";
import TiptapEditor from '~/components/TiptapEditor.vue'



const selected = ref(false);
const notifyOnContractUploadSelected = ref(false);
const sendFinalContractSelected = ref(false);
const toast = useToast();
const auth = useAuth();

const defaultUploadSubject = "Subject: Your Document Has Been Countersigned";
const defaultUploadContent = `We are pleased to inform you that your document has been successfully countersigned, and the signing process is now complete. 

If you have any further questions or need assistance, please do not hesitate to contact us.

Thank you for trusting us with your business.

Best regards,  
Y`;

const defaultNotificationSubject = "Subject: Client Signed the Document - Your Action Required";
const defaultNotificationContent = `This email is to notify you that the client has signed the document. The next step is for you to review and countersign the document to finalize the process.

If you have any questions or need further details, please reach out to our team.

Best regards,  
Y`;

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

const errors = reactive<Record<string, string>>({});

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
  }
  clearErrors();
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
  }
  clearErrors();
};

const validate = (): boolean => {
  clearErrors();
  const newErrors: Record<string, string> = {};

  if (selected.value) {
    if (!state.host) {
      newErrors.host = "SMTP host is required.";
    }
    if (!state.port || state.port <= 0) {
      newErrors.port = "SMTP port is required and must be greater than 0.";
    }
    if (!state.fromEmail) {
      newErrors.fromEmail = "From email is required.";
    } else if (!state.fromEmail.includes("@")) {
      newErrors.fromEmail = "Email must contain '@' symbol."
    }
  }

  if (notifyOnContractUploadSelected.value) {
    if (!state.notifyOnUploadSubject) {
      newErrors.notifyOnUploadSubject = "Email subject is required.";
    }
    if (!state.notifyOnUploadContent) {
      newErrors.notifyOnUploadContent = "Email content is required.";
    }
  }

  if (sendFinalContractSelected.value) {
    if (!state.notifyOnSignatureSubject) {
      newErrors.notifyOnSignatureSubject = "Email subject is required.";
    }
    if (!state.signatureNotificationEmail) {
      newErrors.signatureNotificationEmail = "Notification email is required.";
    } else if (!state.signatureNotificationEmail.includes("@")) {
      newErrors.signatureNotificationEmail = "Email must contain '@' symbol.";
    }
    if (!state.notifyOnSignatureContent) {
      newErrors.notifyOnSignatureContent = "Notification content is required.";
    }
  }

  if (Object.keys(newErrors).length > 0) {
    Object.assign(errors, newErrors); 
    return false;
  }
  return true;
};

const clearErrors = () => {
  Object.keys(errors).forEach((key) => {
    errors[key] = "";
  });
};


onMounted(async () => {
  const response = await auth.fetchWithToken("/settings/smtp");
  if (!response?.error) {
    Object.assign(state, response);

    notifyOnContractUploadSelected.value = !!response.notifyOnUploadContent;
    sendFinalContractSelected.value = !!response.notifyOnSignatureContent;

    selected.value = true;
  }
});

const submit = async () => {
  if (!validate()) {
    toast.add({ title: "Unsuccessful", description: "Please fill all required fields."});
    return;
  }

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

  const response = await auth.fetchWithToken("/settings/smtp", { method, body: JSON.stringify(payload) });

  if (response?.error) {
    toast.add({ title: "Unsuccessful", description: "Failed to save settings." });
  } else {
    toast.add({ title: "Success", description: "Settings saved successfully." });
  }
};


</script>
