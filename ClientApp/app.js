import Vue from 'vue'
import axios from 'axios'
import router from './router/index'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import { FontAwesomeIcon } from './icons'
import VueSweetalert2 from 'vue-sweetalert2';
import VueLodash from 'vue-lodash'
import Vuetify from 'vuetify'
import Lightbox from 'vue-my-photos'





// Registration of global components

import '@fortawesome/fontawesome-free/css/all.css'
import 'bootstrap/dist/css/bootstrap.css'
Vue.component('icon', FontAwesomeIcon);
Vue.component('lightbox', Lightbox);
Vue.use(VueLodash);
Vue.use(VueSweetalert2);
Vue.use(Lightbox);


Vue.use(Vuetify,
  {
    iconfont: 'fa'
  });
import 'vuetify/dist/vuetify.min.css'



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
