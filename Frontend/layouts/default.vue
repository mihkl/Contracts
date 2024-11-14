<template>
  <TheMenu v-if="auth.isAuthenticated.value === true" />
  <slot />
</template>

<script setup lang="ts">
const auth = useAuth();
</script>
