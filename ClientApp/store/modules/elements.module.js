export const elements = {
    namespaced: true,
    state: {
      elements: []
    },
  
    getters: {
        getElements: state => [...state.elements]    
    },

    mutations: {
      fillElements(state, payload) {
        state.elements = [...payload]
      },
      addtoElements(state, payload) {
        state.elements = [...state.elements, {elementId: payload.elementId, name: payload.name, comment: payload.comment, docName: payload.docName,
                                              typeId: payload.typeId, isAvaliableToDelete: true}]
      },
      updateElement(state, payload) {
          let updatedElement = state.elements.find(x => x.elementId === payload.elementId)
          updatedElement.name = payload.name
          updatedElement.comment = payload.comment
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
  