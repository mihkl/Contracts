<template>
  <div class="justify-center items-center flex h-screen">
    <div class="space-y-6 w-1/2">
      <UForm :state="user" class="space-y-6" @submit="submit">
        <UFormGroup label="Email" name="email">
          <UInput v-model="user.email" />
        </UFormGroup>

        <UFormGroup label="Password" name="password">
          <UInput v-model="user.password" type="password" />
        </UFormGroup>

        <div class="flex justify-between items-center">
          <UButton type="submit" class="mr-10 bg-indigo-500 hover:bg-indigo-600"
            >Submit</UButton
          >
          <a href="/register"
            >Don't have an account?
            <span class="text-indigo-600 font-medium hover:text-indigo-500"
              >Register here!</span
            ></a
          >
        </div>
      </UForm>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useAuth } from "~/composables/useAuth";
import type { User } from "~/Types/User";

const auth = useAuth();
const user: User = { email: "", password: "" };

let showError = ref(false);

const submit = async () => {
  showError.value = !(await auth.logIn(user));

  await navigateTo("/");
};
</script>
