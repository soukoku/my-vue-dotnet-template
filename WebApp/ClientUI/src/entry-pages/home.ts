import '../assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './home-root.vue'
import router from './home-router'

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.mount('#app')
