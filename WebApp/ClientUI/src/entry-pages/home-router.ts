import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import NotFoundView from '../views/NotFoundView.vue'

const baseUrl = import.meta.env.BASE_URL

const router = createRouter({
  // history: createWebHistory(import.meta.env.BASE_URL),

  history: createWebHistory(baseUrl + 'home'),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue')
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'NotFound',
      component: NotFoundView,
      meta: { text: 'Not Found' }
    }
  ]
})
router.afterEach((to) => {
  let pgTitle = to.meta.text as string
  if (pgTitle) pgTitle += ' - Sample App'
  else pgTitle = 'Sample App'
  document.title = pgTitle
})

export default router
