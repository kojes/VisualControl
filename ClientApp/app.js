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
import Vuelidate from 'vuelidate'



// Registration of global components

import 'material-design-icons-iconfont/dist/material-design-icons.css'
import colors from 'vuetify/es5/util/colors'



Vue.component('lightbox', Lightbox);
Vue.use(VueLodash);
Vue.use(VueSweetalert2);
Vue.use(Lightbox);
Vue.use(Vuelidate);
Vue.use(Vuetify);

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
