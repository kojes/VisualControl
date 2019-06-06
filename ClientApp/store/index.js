import Vue from 'vue'
import Vuex from 'vuex'
import defects from './modules/defects'
import { alert } from './modules/alert.module'
import { authentication } from './modules/authentication.module'
import { users } from './modules/users.module'
import { wafermeas } from './modules/wafermeas.module'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    defects, alert, authentication, users, wafermeas
  }

})
