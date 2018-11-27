import Vue from 'vue'
import axios from 'axios'
import router from './router/index'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import { FontAwesomeIcon } from './icons'
import VueSweetalert2 from 'vue-sweetalert2';
import VueLodash from 'vue-lodash'



// Registration of global components
Vue.component('icon', FontAwesomeIcon);
Vue.use(VueLodash);
Vue.use(VueSweetalert2);



Vue.prototype.$http = axios;

sync(store, router);

const app = new Vue({
  store,
  router,
  ...App
});

export {
  app,
  router,
  store
}
