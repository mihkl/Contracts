<script setup>
function iconName(theme) {
  if (theme === "system") return "i-ph-laptop";
  if (theme === "light") return "i-ph-sun";
  if (theme === "dark") return "i-ph-moon";
  return "i-ph-coffee";
}
</script>

<template>
  <div>
    <ul>
      <li
        v-for="theme of ['light', 'dark', 'system', 'sepia']"
        :key="theme"
        :class="{
          preferred: !$colorMode.unknown && theme === $colorMode.preference,
          selected: !$colorMode.unknown && theme === $colorMode.value,
        }"
      >
        <Icon
          :name="iconName(theme)"
          class="size-6"
          @click="$colorMode.preference = theme"
        />
      </li>
    </ul>
  </div>
</template>

<style scoped>
ul {
  list-style: none;
  padding: 0;
  margin: 0;
}
ul li {
  display: inline-block;
  padding: 5px;
  margin-right: 5px;
  line-height: 0;
}
p {
  margin: 0;
  padding-top: 10px;
  padding-bottom: 20px;
}
li {
  position: relative;
  cursor: pointer;
  padding: 7px;
  background-color: var(--bg-secondary);
  border: 2px solid var(--border-color);
  margin: 0;
  border-radius: 5px;
  transition: all 0.1s ease;
}
li:hover {
  top: -3px;
}

li.preferred {
  border-color: var(--color-primary);
}
li.selected {
  color: var(--color-primary);
}
</style>
