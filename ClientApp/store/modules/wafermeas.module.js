export const wafermeas = {
  namespaced: true,
  state: {
    selectedDies: []
  },
  actions: {
    updateSelectedDies ({ commit }, { selectedDies }) {
      commit('updateSelectedDies', { selectedDies })
    }

  },
  mutations: {
    updateSelectedDies (state, selectedDies) {
      state.selectedDies = selectedDies
    }
  }
}
