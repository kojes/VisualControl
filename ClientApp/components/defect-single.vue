<template>
  <div id="container" class="container-fluid">
    <loading :active.sync="isLoading"
             :can-cancel="false"
             :color="overlayColor"
             :loader="overlayLoader"
             :is-full-page="false" ></loading>
             
    <form v-on:submit.prevent="">

     
      <div class="form-row">

        <div class="form-group col-md-2 col-lg-2 offset-3">
          <label for="waferSelect">Название пластины:</label>
          <model-list-select :list="wafers"
                             v-model="selectedWafer" id="waferSelect" placeholder="Выберите пластину для добавления дефекта" option-value="waferId"
                             option-text="waferId">
          </model-list-select>
        </div>
        <div class="form-group col-md-3 col-lg-3">
          <label for="stageSelect">Название технологического этапа:</label>
          <model-list-select :list="stages"
                             v-model="selectedStage" id="stageSelect" placeholder="Выберите технологический этап" option-value="stageId"
                             option-text="stageName">
          </model-list-select>

        </div>

       

      </div>

      <div class="form-row">

        <div class="form-group col-md-2 col-lg-2 offset-3">
          <label for="dieSelect">Код кристалла:</label>
          <model-list-select :list="dies"
                             v-model="selectedDie" id="dieSelect" placeholder="Выберите номер кристалла для добавления дефекта" option-value="dieId"
                             option-text="code">
          </model-list-select>
        </div>
        <div class="form-group col-md-3 col-lg-3">
          <label for="defecttypeSelect">Тип дефекта:</label>
          <model-list-select :list="defecttypes"
                             v-model="selectedDefectType" id="defecttypeSelect" placeholder="Выберите тип дефекта" option-value="defectTypeId"
                             option-text="description">
          </model-list-select>

        </div>
      
      </div>


      <div class="form-row">

        <div class="form-group col-md-2 col-lg-2 offset-3">
          <label for="dangerlevelSelect">Опасность дефекта:</label>
          <model-list-select :list="dangerlevels"
                             v-model="selectedDangerLevel" id="dangerlevelSelect" placeholder="Выберите опасность дефекта" option-value="dangerLevelId"
                             option-text="specification">
          </model-list-select>
          </div>

        <div class="form-group col-md-3 col-lg-3 d-flex align-items-stretch">
          <button type="submit" v-on:click="debouncedSaveDefect" class="btn btn-outline-primary btn-block">Сохранить дефект</button>
        </div>
       
         

        </div>

      <div class="form-row">
        <div class="form-group col-md-5 col-lg-5 offset-3">
          <photo-uploader v-on:fileLoaded="fileLoaded"></photo-uploader>
        </div>
       
      </div>
</form>
  </div>


</template>

<script>

  import photouploader from './photo-uploader.vue';
  import Loading from 'vue-loading-overlay';
  import 'vue-loading-overlay/dist/vue-loading.css';
  
  import { ModelListSelect } from 'vue-search-select';
  export default {
    data() {
      return {
        overlayColor: "#3434ff",
        overlayLoader: "spinner",
        isLoading: true,
        debouncedSaveDefect: {},
        wafers: [],
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
      'photo-uploader': photouploader, ModelListSelect, Loading
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
              position: 'center-start',
              timer: 4000
            });
        }
        else
        {
          let defectdata =
            {
                waferId: this.selectedDie.waferId,
                operator: "Strelnikov",
                dieId: this.selectedDie.dieId,
                defectTypeId: this.selectedDefectType.defectTypeId,
                dangerLevelId: this.selectedDangerLevel.dangerLevelId,
                stageId: this.selectedStage.stageId,
                LoadedPhotosList: this.loadedFiles
            };
          let response = await this.$http.post(`/api/defect/savenewdefect`, defectdata);
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
