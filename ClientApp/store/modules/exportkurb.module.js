export const exportkurb = {
  namespaced: true,
  state: {
    elementsReady: []
  },

  getters: {
    isReadyForExport: state => {
      return state.elementsReady.length > 0 && state.elementsReady.every(_ => _.ready);
    },
    isThisElementisReady: state => key => {
      const thisElement = state.elementsReady.find(_ => _.key === key)
      return thisElement && thisElement.ready
    }
  },

  mutations: {
    updateElementsReady(state, payload) {
      const element = state.elementsReady.find(_ => _.key === payload.key);
      if (element) {
        element.ready = payload.ready;
      } else {
        state.elementsReady.push(payload);
      }
    },
    deleteFromElementsReady(state, payload) {
      state.elementsReady = state.elementsReady.filter(function(value) {
        return value.key !== payload
      });
    }
  }
}
