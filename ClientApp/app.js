import Vue from 'vue'
import axios from 'axios'
import router from './router/index'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import VueSweetalert2 from 'vue-sweetalert2'
import VueLodash from 'vue-lodash'
import Vuetify from 'vuetify'
import Lightbox from 'vue-my-photos'
import Vuelidate from 'vuelidate'


import 'material-design-icons-iconfont/dist/material-design-icons.css'
import 'vuetify/dist/vuetify.min.css'


Vue.component('lightbox', Lightbox)
Vue.use(VueLodash)
Vue.config.devtools = true
Vue.config.performance = true
Vue.use(VueSweetalert2)
Vue.use(Lightbox)
Vue.use(Vuelidate)


const opts =  {
  icons: {
    iconfont: 'md',
  },
  theme:
  {
    dark: true,
    themes: {
      dark: {
        primary: '#fc0'
      }
    }
  }
}

Vue.use(Vuetify)

Vue.prototype.$http = axios;

sync(store, router);

const app = new Vue({  
  vuetify: new Vuetify(opts),
  store,
  router,
  ...App
});

export {
  app,
  router,
  store
}
