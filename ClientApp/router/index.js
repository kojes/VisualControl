import Vue from 'vue'
import VueRouter from 'vue-router'
import { routes } from './routes'

Vue.use(VueRouter);

let router = new VueRouter({
  mode: 'history',
  routes
});

router.beforeEach((to, from, next) => {

  const publicPages = ["/login", "/registration"];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem("user");

  if (authRequired && !loggedIn) {
    return next("/login");
  }

  next();
});

export default router
