<template>
  <v-container>
    <v-row>
      <v-col lg="12">
        <v-menu :nudge-width="300" offset-x>
          <template v-slot:activator="{ on }">
            <v-btn color="green" outlined dark v-on="on">Создать новый элемент</v-btn>
          </template>
          <v-card>
            <v-row>
              <v-col lg="6" class="pl-8">
                <v-text-field
                  outlined
                  :error-messages="newElement.name ? [] : 'Введите название элемента'"
                  label="Название элемента"
                ></v-text-field>
              </v-col>
              <v-col lg="6" class="px-8">
                <v-select
                  outlined
                  :items="avElementTypes"
                  v-model="newElement.selectedType"
                  no-data-text="Нет данных"
                  item-text="name"
                  item-value="id"
                  label="Тип элемента"
                ></v-select>
              </v-col>
            </v-row>
            <v-row>
              <v-col lg="12" class="px-8">
                <v-text-field outlined v-model="newElement.comment" label="Описание элемента"></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col lg="6" offset-lg="6" class="pr-8">
                <v-btn block outlined color="success" @click="createElement()">Создать</v-btn>
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
  data() {
    return {
      newElement: { name: "", comment: "", selectedType: 0 },
      avElementTypes: []
    };
  },

  methods: {

    async initElementTypes() {
         await this.$http
            .get(`/api/elementtype/all`)
            .then(response => { this.avElementTypes = response.data
                                this.newElement.selectedType = this.avElementTypes[0].id})
            .catch(err => console.log(err))
    },

    createElement() {
        this.$store.commit("elements/addtoElements", newElement)
    }
  },

  async mounted() {
    this.initElementTypes()
  }

}
</script>

