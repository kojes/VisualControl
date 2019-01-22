import Vue from 'vue'
import Vuex from 'vuex'
import Defects from './modules/defects'

Vue.use(Vuex);



export default new Vuex.Store({
  modules: {
     Defects
  }
  
})
