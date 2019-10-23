export const exportkurb = {
  namespaced: true,
  state: {
    elementsReady: [],
    elementsAutoIdmr: []
  },

  getters: {
    isReadyForExport: state => {
      return state.elementsReady.length > 0 && state.elementsReady.every(_ => _.ready);
    },
    isThisElementisReady: state => key => {
      const thisElement = state.elementsReady.find(_ => _.key === key)
      return thisElement && thisElement.ready
    },
    elementsAutoIdmrStatus: state => {
       return state.elementsAutoIdmr
    }
  },

  mutations: {
    initAutoIdmr(state, payload) {
        state.elementsAutoIdmr = payload.elements
    },

    clearAutoIdmr(state) {
       state.elementsAutoIdmr = []
    },

    updateElementAutoIdmr(state, payload) {
      const element = state.elementsAutoIdmr.find(_ => _.key === payload.key);
      if (element) {
        element.done = payload.done;
      }
    
    },
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
};
