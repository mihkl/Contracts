<template></template>
<script setup lang="ts">
import { useRoute } from "nuxt/app";

const route = useRoute();

const signature = route.query?.signature;
const validFrom = route.query?.validFrom;
const validUntil = route.query?.validUntil;

console.log(signature, validFrom, validUntil);
</script>
