<script setup lang="ts">
import { getPageData } from '@/stores/pageData'
import { ref } from 'vue'

const pageData = getPageData<{ message: string, baseUrl: string }>()

const apiResult = ref()

async function testApi() {
  const resp = await fetch(`${pageData.baseUrl}api/myapi`, { method: 'GET' })
  const data = await resp.json()
  apiResult.value = data
}
</script>

<template>
  <main>
    <h1>This is an home page</h1>
    <p>Message: {{ pageData.message }}</p>

    <div>
      <button @click="testApi"
        class="rounded-md bg-blue-600 px-2.5 py-1.5 text-sm font-semibold text-white shadow-sm hover:bg-blue-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-blue-600">
        Test calling API
      </button>
    </div>
    <div>
      Result:
      <code>{{ apiResult }}</code>
    </div>
  </main>
</template>
