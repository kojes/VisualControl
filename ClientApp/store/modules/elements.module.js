export const elements = {
    namespaced: true,
    state: {
      elements: []
    },
  
    getters: {
        getElements: state => state.elements      
    },

    mutations: {
      addtoElements(state, payload) {
        state.elements.push(payload)
      },
      deleteFromElements(state, payload) {
        state.elementsReady = state.elements.filter(function(value) {
          return value.name !== payload
        })
      }
    }
  };
  