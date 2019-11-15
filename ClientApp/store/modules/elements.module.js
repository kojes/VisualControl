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
        state.elements = [...state.elements, {name: payload.name, comment: payload.comment, typeId: payload.typeId}]
      },
      deleteFromElements(state, payload) {
        state.elements = state.elements.filter(function(value) {
          return value.name !== payload
        })
      },
      clearElements(state) {
        state.elements = []
      }
    }
  }
  