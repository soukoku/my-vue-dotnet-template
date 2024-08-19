<script setup lang="ts">
import GlobalNav from '@/components/GlobalNav.vue'
import 'swagger-ui/dist/swagger-ui.css'
import SwaggerUI from 'swagger-ui'
import { onMounted, ref } from 'vue'

const baseUrl = import.meta.env.BASE_URL

const swag = ref<HTMLElement>()

onMounted(() => {
  // swagger = 
  SwaggerUI({
    url: `${baseUrl}swagger/v1/swagger.json`,
    domNode: swag.value,
    defaultModelsExpandDepth: 0,
    defaultModelExpandDepth: 3,
  })
})
</script>

<template>
  <GlobalNav />
  <div>
    This is the documentation area of the app. The sample swagger UI could be just one of
    the many documentation pages.
  </div>
  <div ref="swag">
  </div>
</template>
