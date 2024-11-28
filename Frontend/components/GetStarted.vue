<template>
    <div class="min-h-screen bg-gray-100 flex items-center justify-center">
      <div class="max-w-4xl w-full bg-white p-8 rounded shadow">
        <h1 class="text-4xl font-bold mb-6 text-center text-indigo-600">Get Started</h1>
        <p class="text-lg text-gray-700 mb-6 text-center">
          Follow these simple steps to get started with your contract management system.
        </p>
  
        <div class="space-y-10">
          <div class="flex items-start space-x-4">
            <div class="flex-shrink-0">
              <div class="bg-indigo-100 text-indigo-600 rounded-full w-12 h-12 flex items-center justify-center text-2xl font-bold">
                1
              </div>
            </div>
            <div>
              <h2 class="text-xl font-semibold mb-2 text-indigo-600">Prepare Your Template</h2>
              <p class="text-gray-700">
                Start by preparing a <strong>.docx</strong> file of the contract you'd like to use. Instead of leaving blank spaces for customers information, use placeholders like:
              </p>
              <div class="mt-3 bg-gray-50 p-4 rounded border border-gray-200">
                <strong>Example of all currently supported placeholders:</strong>
                <ul class="list-disc list-inside text-gray-600">
                  <li><code>_firstName_</code> : Placeholder for the customer's first name.</li>
                  <li><code>_lastName_</code> : Placeholder for the customer's last name.</li>
                  <li><code>_email_</code> : Placeholder for the customer's email.</li>
                  <li><code>_socialSecurityNumber_</code> : Placeholder for the customer's social security number.</li>
                  <li><code>_dateOfBirth_</code> : Placeholder for the customer's date of birth.</li>
                  <li><code>_address_</code> : Placeholder for the customer's address.</li>
                  <li><code>_bankAccountNumber_</code> : Placeholder for the customer's bank account number.</li>
                </ul>
              </div>
            </div>
          </div>
  
          <!-- Step 2 -->
          <div class="flex items-start space-x-4">
            <div class="flex-shrink-0">
              <div class="bg-indigo-100 text-indigo-600 rounded-full w-12 h-12 flex items-center justify-center text-2xl font-bold">
                2
              </div>
            </div>
            <div>
              <h2 class="text-xl font-semibold mb-2 text-indigo-600">Upload Your Template</h2>
              <p class="text-gray-700">
                Once your file is ready, upload it to the system from the Home page.
              </p>
              <img
                src="public/upload.png"
                alt="Upload Example"
                class="mt-3 rounded shadow border border-gray-200"
              />
            </div>
          </div>
  
          <div class="flex items-start space-x-4">
            <div class="flex-shrink-0">
              <div class="bg-indigo-100 text-indigo-600 rounded-full w-12 h-12 flex items-center justify-center text-2xl font-bold">
                3
              </div>
            </div>
            <div>
              <h2 class="text-xl font-semibold mb-2 text-indigo-600">Send to customer</h2>
              <p class="text-gray-700">
                After uploading your template, you can generate a link and send it to the customer to fill in the data. Once you generate the link, it will be copied to your clipboard.
              </p>  
              <img
                src="public/generate.png"
                alt="Customize Example"
                class="mt-3 rounded shadow border border-gray-200"
              />
            </div>
          </div>
  
          <div class="flex items-start space-x-4">
            <div class="flex-shrink-0">
              <div class="bg-indigo-100 text-indigo-600 rounded-full w-12 h-12 flex items-center justify-center text-2xl font-bold">
                4
              </div>
            </div>
            <div>
              <h2 class="text-xl font-semibold mb-2 text-indigo-600">Countersign the document</h2>
              <p class="text-gray-700">
                If the customer has signed the document, it will appear on the Submissions page.
              </p>
            </div>
          </div>
        </div>
  
        <div class="mt-8 text-center">
          <nuxt-link to="/templates" class="bg-indigo-500 text-white px-6 py-3 rounded hover:bg-indigo-600">
            Start Now
          </nuxt-link>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  definePageMeta({
    layout: "default",
  });
  </script>
  