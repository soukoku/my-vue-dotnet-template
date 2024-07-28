<script setup lang="ts">
import GlobalNav from '@/components/GlobalNav.vue'
import { getPageData } from '@/stores/pageData'
interface ErrorPageModel {
  requestId?: string
}
const pageData = getPageData<ErrorPageModel>();

</script>

<template>
  <GlobalNav />

  <div class="prose">
    This is sample catch-all error page from aspnet. The request ID is: {{ pageData.requestId }}
  </div>
</template>
