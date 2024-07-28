<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import GlobalNav from '@/components/GlobalNav.vue'
</script>

<template>
  <GlobalNav />
  <div>
    This is the main area of the app.

    <nav>
      <RouterLink to="/">Home</RouterLink> |
      <RouterLink to="/about">About</RouterLink> |
    </nav>
  </div>

  <RouterView />
</template>
