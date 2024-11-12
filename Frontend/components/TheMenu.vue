<template>
  <div class="flex items-center border-b border-gray-200 dark:border-gray-800">
    <div class="flex-shrink-0 mr-6">
      <nuxt-link to="/">
        <img src="public/logo.png" alt="Epic logo" class="h-12 w-auto ml-2" />
      </nuxt-link>
    </div>
    <UHorizontalNavigation :links="links" class="flex-1 content-center" />
    <ColorMode />
    <UButton v-if="auth.isAuthenticated.value" @click="auth.logOut" icon="i-heroicons-arrow-right-start-on-rectangle"
      class="mr-6 mb-2.5" />
  </div>
</template>

<script setup lang="ts">
const route = useRoute();
const auth = useAuth();
const forceUpdate = ref(0);

const handleLogOut = async () => {
  await auth.logOut();
  forceUpdate.value++;
};

const links = [
  {
    label: "Home",
    icon: 'i-heroicons-home',
    to: "/",
  },
  {
    label: "My Templates",
    icon: 'i-heroicons-queue-list',
    to: "/templates",
  },
  {
    label: "Contracts",
    icon: 'i-heroicons-document-text',
    to: "/contracts",
  },
  {
    label: "Login",
    icon: 'i-heroicons-arrow-right-end-on-rectangle',
    to: "/login",
  },
  {
    label: "Register",
    icon: 'i-heroicons-user',
    to: "/register",
  },
];
</script>
