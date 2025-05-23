<template>
  <div class="min-h-screen flex items-center justify-center">
    <div class="max-w-4xl w-full p-8">
      <h1 class="text-4xl font-bold mb-6 text-center text-indigo-600">Get Started</h1>
      <p class="text-lg mb-6 text-center">
        Follow these simple steps to get started with your contract management system.
      </p>

      <div class="space-y-6">
        <!-- Step 1 -->
        <div>
          <button
            class="flex items-center justify-between w-full px-4 py-3 bg-indigo-100 rounded-lg text-indigo-600 font-semibold text-left"
            @click="toggleStep(1)"
          >
            <span>Step 1: Prepare Your Template</span>
            <span>{{ openSteps.includes(1) ? "-" : "+" }}</span>
          </button>
          <div
            v-show="openSteps.includes(1)"
            class="mt-3 p-4 rounded border border-gray-200 transition-all duration-300"
          >
            <p class="">
              Start by preparing a <strong>.docx</strong> file of the contract you'd like to use. Instead of leaving blank spaces for customers' information, use placeholders like:
            </p>
            <div class="mt-3">
              <strong>Example of all currently supported placeholders:</strong>
              <ul class="list-disc list-inside">
                <li><code>_firstName_</code>: Placeholder for the customer's first name.</li>
                <li><code>_lastName_</code>: Placeholder for the customer's last name.</li>
                <li><code>_email_</code>: Placeholder for the customer's email.</li>
                <li><code>_socialSecurityNumber_</code>: Placeholder for the customer's social security number.</li>
                <li><code>_dateOfBirth_</code>: Placeholder for the customer's date of birth.</li>
                <li><code>_address_</code>: Placeholder for the customer's address.</li>
                <li><code>_bankAccountNumber_</code>: Placeholder for the customer's bank account number.</li>
              </ul>
            </div>
          </div>
        </div>

        <!-- Step 2 -->
        <div>
          <button
            class="flex items-center justify-between w-full px-4 py-3 bg-indigo-100 rounded-lg text-indigo-600 font-semibold text-left"
            @click="toggleStep(2)"
          >
            <span>Step 2: Upload Your Template</span>
            <span>{{ openSteps.includes(2) ? "-" : "+" }}</span>
          </button>
          <div
            v-show="openSteps.includes(2)"
            class="mt-3 p-4 rounded border border-gray-200 transition-all duration-300"
          >
            <p>
              Once your file is ready, upload it to the system from the <strong>Home</strong> page.
            </p>
            <img
              src="public/upload.png"
              alt="Upload Example"
              class="mt-3 rounded shadow border border-gray-200"
            />
          </div>
        </div>

        <!-- Step 3 -->
        <div>
          <button
            class="flex items-center justify-between w-full px-4 py-3 bg-indigo-100 rounded-lg text-indigo-600 font-semibold text-left"
            @click="toggleStep(3)"
          >
            <span>Step 3: Send to Customer to Sign</span>
            <span>{{ openSteps.includes(3) ? "-" : "+" }}</span>
          </button>
          <div
            v-show="openSteps.includes(3)"
            class="mt-3 p-4 rounded border border-gray-200 transition-all duration-300"
          >
            <p>
              After uploading your template, it will appear on <strong>My Templates</strong> page. You can now generate a link and send it to the customer to fill in the data and sign it. 
              Once you generate the link, it will be copied to your clipboard which can be sent to customer.
            </p>
            <img
              src="public/generate.png"
              alt="Customize Example"
              class="mt-3 rounded shadow border border-gray-200"
            />
          </div>
        </div>

        <!-- Step 4 -->
        <div>
          <button
            class="flex items-center justify-between w-full px-4 py-3 bg-indigo-100 rounded-lg text-indigo-600 font-semibold text-left"
            @click="toggleStep(4)"
          >
            <span>Step 4: Countersign the Document</span>
            <span>{{ openSteps.includes(4) ? "-" : "+" }}</span>
          </button>
          <div
            v-show="openSteps.includes(4)"
            class="mt-3 p-4 rounded border border-gray-200 transition-all duration-300"
          >
            <p>
              If the customer has signed the document, it will appear on the <strong>Submissions</strong> page. Next step would be to countersign the document.
            </p>
            <img
              src="public/submissions.png"
              alt="Customize Example"
              class="mt-3 rounded shadow border border-gray-200"
            />
          </div>
        </div>

        <!-- Step 5 -->
        <div>
          <button
            class="flex items-center justify-between w-full px-4 py-3 bg-indigo-100 rounded-lg text-indigo-600 font-semibold text-left"
            @click="toggleStep(5)"
          >
            <span>Bonus Step: Automatic Email notifications</span>
            <span>{{ openSteps.includes(5) ? "-" : "+" }}</span>
          </button>
          <div
            v-show="openSteps.includes(5)"
            class="mt-3 p-4 rounded border border-gray-200 transition-all duration-300"
          >
            <p>
              On the <strong>Integrations</strong> page you can set up email notification system. You can enable that a notification email will be sent to the customer after both parties have signed the contract and/or a company representitive will receive an email after the customer has first signed the contract.  
            </p>

          </div>
        </div>
      </div>

      <div class="mt-8 text-center">
        <nuxt-link to="/" class="bg-indigo-500 text-white px-6 py-3 rounded hover:bg-indigo-600">
          Start Now
        </nuxt-link>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";

const openSteps = ref([]);

const toggleStep = (step) => {
  if (openSteps.value.includes(step)) {
    openSteps.value = openSteps.value.filter((s) => s !== step);
  } else {
    openSteps.value.push(step);
  }
};
definePageMeta({
  layout: "default",
});
</script>
