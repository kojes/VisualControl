<template>
  
    <v-container grid-list-lg>
      <loading :active.sync="isLoading"
               :can-cancel="false"
               :color="overlayColor"
               :loader="overlayLoader"
               :is-full-page="false"></loading>



     
        <v-layout align-start justify-center row>

          <v-flex lg4>
            <v-autocomplete :items="wafers" v-model="selectedWafer" label="Выберите пластину для добавления дефекта" return-object item-text="waferId"></v-autocomplete>
          </v-flex>
          <v-flex lg4>

            <v-select :items="stages"
                      v-model="selectedStage"  label="Выберите технологический этап" return-object item-value="stageId"
                      item-text="stageName">
            </v-select>

          </v-flex>



        </v-layout>
     

        <v-layout align-start justify-center row>

          <v-flex lg4>

            <v-autocomplete :items="dies" v-model="selectedDie" label="Выберите номер кристалла для добавления дефекта" return-object item-text="code"></v-autocomplete>

          </v-flex>
          <v-flex lg4>

            <v-select :items="defecttypes"
                      v-model="selectedDefectType" label="Выберите тип дефекта" return-object item-value="defectTypeId"
                      item-text="description">
            </v-select>

          </v-flex>
        </v-layout>


        <v-layout align-start justify-center row>

          <v-flex lg4>

            <v-select :items="dangerlevels"
                      v-model="selectedDangerLevel" label="Выберите опасность дефекта" return-object item-value="dangerLevelId"
                      item-text="specification">
            </v-select>
          </v-flex>

          <v-flex lg4>
            <v-btn block @click="debouncedSaveDefect" color="indigo">Сохранить дефект</v-btn>
          </v-flex>
        </v-layout>
        <v-layout align-start justify-center row>
          <v-flex lg8>

            <photo-uploader :reset="resetPhotoUploader" v-on:fileLoaded="fileLoaded"></photo-uploader>


          </v-flex>
        </v-layout>

</v-container>
  

</template>

<script>

  import photouploader from './photo-uploader.vue';
  import Loading from 'vue-loading-overlay';
  import 'vue-loading-overlay/dist/vue-loading.css';
  export default {
    data() {
      return {
        overlayColor: "#3434ff",
        overlayLoader: "spinner",
        isLoading: true,
        debouncedSaveDefect: {},
        wafers: [],
        resetPhotoUploader: "",
        dies: [],
        stages: [],
        dangerlevels: [],
        defecttypes: [],
        loadedFiles: [],
        selectedStage: {},
        selectedDie: {},
        selectedWafer: {},
        selectedDangerLevel: {},
        selectedDefectType: {}
      }
    },
    components: {
      'photo-uploader': photouploader, Loading
    },
    watch:
    {
      selectedWafer: async function () {
        var selectedWaferId = this.selectedWafer.waferId;
        let response = await this.$http.get(`/api/die/getbywaferid?waferid=${selectedWaferId}`);
        this.dies = response.data;
        this.selectedDie = this.dies[0];
        var codeproductid = this.selectedWafer.codeProductId;
        let responseStages = await this.$http.get(`/api/stage/getstagesbycodeproductid?codeproductid=${codeproductid}`);
        this.stages = responseStages.data;
        this.selectedStage = this.stages[0];
      }
    },
    methods:
    {
      savedefect: async function ()
      {
        if (this.loadedFiles.length == 0)
        {
            this.$swal({
              type: 'error',
              text: 'Загрузите фото дефекта',
              toast: true,
              showConfirmButton: false,
              position: 'top-end',
              timer: 4000
            });
        }
        else
        {
          this.resetPhotoUploader = "noreset";
          let defectdata =
            {
                waferId: this.selectedDie.waferId,
                operator: "Strelnikov",
                dieCode: this.selectedDie.code,
                dieId: this.selectedDie.dieId,
                defectTypeId: this.selectedDefectType.defectTypeId,
                dangerLevelId: this.selectedDangerLevel.dangerLevelId,
                stageId: this.selectedStage.stageId,
                LoadedPhotosList: this.loadedFiles
            };
          this.isLoading = true;
          let response = await this.$http.post(`/api/defect/savenewdefect`, defectdata);
          this.loadedFiles = [];                            
          this.resetPhotoUploader = "reset";          
          this.$swal({
            type: response.data.responseType,
            text: response.data.message,
            toast: true,
            showConfirmButton: false,
            position: 'top-end',
            timer: 4000
          });
          this.isLoading = false;     

        }
      },

      fileLoaded: function (fileId)
      {
         this.loadedFiles.push(fileId);
      }

    },
    async created() {


      let response = await this.$http.get(`/api/wafer/getall`);
      this.wafers = response.data;
      this.selectedWafer = this.wafers[0];

      response = await this.$http.get(`/api/dangerlevel/getall`);
      this.dangerlevels = response.data;
      this.selectedDangerLevel = this.dangerlevels[0];

      response = await this.$http.get(`/api/defecttype/getall`);
      this.defecttypes = response.data;
      this.selectedDefectType = this.defecttypes[0];

      this.isLoading = false;
      this.debouncedSaveDefect = this._.debounce(this.savedefect, 2000);
    }
    
  }

</script>
<style>
  .customSelect {
    width: 20%;
  }
</style>
