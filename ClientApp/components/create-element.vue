<template>
  <v-container>
    <v-row>
      <v-col lg="12">
        <v-menu v-model="menu" :close-on-content-click="false" :nudge-width="300">
          <template v-slot:activator="{ on }">
            <v-btn color="indigo" dark v-on="on">Создать новый элемент</v-btn>
          </template>
          <v-card>
            <v-row>
              <v-col lg="5" class="pl-8">
                <v-text-field
                  
                  v-model="newElement.name"
                  :error-messages="validationErrors"
                  label="Название элемента"
                ></v-text-field>
              </v-col>
              <v-col lg="7" class="px-8">
                <v-select
                  
                  :items="avElementTypes"
                  v-model="newElement.typeId"
                  no-data-text="Нет данных"
                  item-text="name"
                  item-value="id"
                  label="Тип элемента"
                ></v-select>
              </v-col>
            </v-row>
            <v-row>
            <v-col lg="12" class="px-8">
                <v-text-field v-model="newElement.comment" label="Описание элемента"></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col lg="6" offset-lg="6" class="pr-8">
                <v-btn v-if="validationErrors.length === 0" block color="success" @click="createElement()">Создать</v-btn>
              </v-col>
            </v-row>
          </v-card>
        </v-menu>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  props: ['mode'],
  data() {
    return {
      newElement: {name: "", comment: "", typeId: 0, isAvaliableToDelete: true},
      avElementTypes: [],
      menu: false
    }
  },

  methods: {    
    async initElementTypes() {
         await this.$http
            .get(`/api/elementtype/all`)
            .then(response => { this.avElementTypes = response.data
                                this.newElement.typeId = this.avElementTypes[0].id})
            .catch(err => console.log(err))
    },

    createElement() {
        if(this.mode === "create")
        {
          this.$store.commit("elements/addtoElements", this.newElement)
        }
        if(this.mode === "update")
        {
           this.$emit('create-element', this.newElement)
        }       
        this.menu = false
    }
  },

  computed: {
     validationErrors() {
        if(!this.newElement.name) {
           return 'Введите название элемента'
        }
        if(this.$store.state.elements.elements.filter(x => x.name === this.newElement.name).length > 0)
        {
           return 'Элемент с таким названием уже существует'
        }
        return []
     }
  },

  async mounted() {
    this.initElementTypes()
  }

}
</script>

