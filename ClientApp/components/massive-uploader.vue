<template>
  <v-container grid-list-lg>
    <v-layout row wrap>
      <v-flex lg4>
        <v-card dark color="info">
          <v-card-text>
            <v-text-field v-model="folderPath" label="Путь к папке с дефектами"></v-text-field>
          </v-card-text>
        </v-card>
      </v-flex>

      <v-flex lg4>
        <v-card dark color="info">
          <v-card-text>
            <v-btn outlined color="primary" @click="getFolderDefects()">Проверить папку</v-btn>
          </v-card-text>
        </v-card>
      </v-flex>

   </v-layout>

    <v-layout row wrap>
      <v-flex lg4>
        <v-card dark color="info">
          <v-card-text>
            <v-text-field v-model="selectedWafer" label="Номер пластины"></v-text-field>
          </v-card-text>
        </v-card>
      </v-flex>

      <v-flex lg4>
        <v-card dark color="info">
          <v-card-text>
            <v-autocomplete v-model="selectedStage"
                            :items="stages"
                            no-data-text="Нет данных"
                            filled
                            outlined
                            label="Название этапа">
            </v-autocomplete>
          </v-card-text>
        </v-card>
      </v-flex>


    </v-layout>

    <v-layout row wrap>
      <v-flex lg4>
        <v-card dark color="info">
          <v-card-text>
            <v-chip color="indigo lighten-2" text-color="white">
              <v-avatar class="indigo darken-3">0</v-avatar>
              Всего критических дефектов
            </v-chip>
            <v-chip color="teal lighten-2" text-color="white">
              <v-avatar class="teal darken-3">0</v-avatar>
              Всего незначительных дефектов
            </v-chip>
          </v-card-text>
        </v-card>
      </v-flex>

    </v-layout>
    <v-snackbar v-model="errorSnackbar" top>
      {{ snackbarText }}
      <v-btn color="pink"
             text
             @click="errorSnackbar = false">
        Закрыть
      </v-btn>
    </v-snackbar>

</v-container>
</template>


<script>
  export default {

    data() {
      return {
        selectedWafer: "",
        folderPath: "",
        errorSnackbar: false,
        snackbarText: "",
        selectedStage: "",
        stages: []


      }
    },

    methods:
    {
      async getFolderDefects()
      {
        await this.$http.get(`/api/massiveuploader/getfolderdefects?folderPath=${this.folderPath}`)
          .then((response) => {
            
            if (response.data.errorList.length > 0)
            {
                this.snackbarText = response.data.errorList[0].message;
                this.errorSnackbar = true;
            }
          })
          .catch((error) => {

          });
      }
    }

  }

</script>


<style>


</style>
