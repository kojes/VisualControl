import { userService } from '../../services'
import router from '../../router/index'
// eslint-disable-next-line no-undef
const user = JSON.parse(localStorage.getItem('user'))
const initialState = user
  ? { status: { loggedIn: true }, user }
  : { status: {}, user: null }

export const authentication = {
  namespaced: true,
  state: initialState,
  actions: {
    login ({ dispatch, commit }, { username, password }) {
      commit('loginRequest', { username })

      userService.login(username, password)
        .then(
          currentUser => {
            commit('loginSuccess', currentUser)
            router.push('/')
          },
          error => {
            commit('loginFailure', error)
            dispatch('alert/error', error, { root: true })
          }
        )
    },

    registry ({ dispatch, commit }, { user }) {
      commit('registryRequest', { user })

      userService.registry(user)
        .then(
          currentUser => {
            commit('registrySuccess', currentUser)
            router.push('/')
          },
          error => {
            commit('registryFailure', error)
            dispatch('alert/error', error, { root: true })
          }
        )
    },
    logout ({ commit }) {
      userService.logout()
      commit('logout')
    }
  },
  mutations: {
    loginRequest (state, currentUser) {
      state.status = { loggingIn: true }
      state.user = currentUser
    },
    loginSuccess (state, currentUser) {
      state.status = { loggedIn: true }
      state.user = currentUser
    },
    loginFailure (state) {
      state.status = {}
      state.user = null
    },
    registryRequest (state, currentUser) {
      state.status = { loggingIn: true }
      state.user = currentUser
    },
    registrySuccess (state, currentUser) {
      state.status = { loggedIn: true }
      state.user = currentUser
    },
    registryFailure (state) {
      state.status = {}
      state.user = null
    },
    logout (state) {
      state.status = {}
      state.user = null
    }
  }
}
