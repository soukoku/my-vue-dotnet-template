<script setup lang="ts">
const baseUrl = import.meta.env.BASE_URL

const appAreas = [{
  text: 'Home',
  href: `${baseUrl}home`,
}, {
  text: 'Admin',
  href: `${baseUrl}admin`,
}, {
  text: 'Docs',
  href: `${baseUrl}docs`,
}]

function isCurrent(href: string) {
  return window.location.pathname.toLowerCase().startsWith(href)
}


</script>
<template>
  <header class="relative flex-none shadow select-none bg-gray-50">
    <nav class="flex h-12 mx-4 space-x-4">
      <span class="inline-flex items-center text-xl italic font-semibold text-blue-600">
        Sample App
      </span>
      <a v-for="route in appAreas" :key="route.href" :href="route.href"
        :class="[isCurrent(route.href) ? 'border-blue-500' : 'border-transparent hover:border-gray-300 hover:text-gray-700',
          'inline-flex items-center border-b-2 px-1 pt-1 text-sm font-medium text-gray-500 transition-all min-w-16 justify-center']">
        {{ route.text }}</a>
    </nav>
  </header>
</template>