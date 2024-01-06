<script setup lang="ts">
import { getPageData } from '@/stores/pageData'

const pageData = getPageData<{ message: string }>()
</script>

<template>
  <main>
    <h1>This is an home page</h1>
    <p>Message: {{ pageData.message }}</p>
  </main>
</template>
