<template>
  <div id="container" class="container-fluid">


    <form>

      <div class="form-row">

        <div class="form-group col-md-2 col-lg-2 offset-2">
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

        <div class="form-group col-md-2 col-lg-2 offset-2">
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

        <div class="form-group col-md-2 col-lg-2 offset-2">
          <label for="dangerlevelSelect">Опасность дефекта:</label>
          <model-list-select :list="dangerlevels"
                             v-model="selectedDangerLevel" id="dangerlevelSelect" placeholder="Выберите опасность дефекта" option-value="dangerLevelId"
                             option-text="specification">
          </model-list-select>
          </div>

        <div class="form-group col-md-3 col-lg-3 d-flex align-items-stretch">
          <button type="submit" class="btn btn-outline-primary btn-block">Сохранить дефект</button>
        </div>
       
         

        </div>

      <div class="form-row">
        <div class="form-group col-md-5 col-lg-5 offset-2">
          <photo-uploader></photo-uploader>
        </div>
       
      </div>
</form>
  </div>


</template>

<script>

  import photouploader from './photo-uploader.vue';
  import { ModelListSelect } from 'vue-search-select';
  export default {
    data() {
      return {
        wafers: [],
        dies: [],
        stages: [],
        dangerlevels: [],
        defecttypes: [],
        selectedStage: {},
        selectedDie: {},
        selectedWafer: {},
        selectedDangerLevel: {},
        selectedDefectType: {}
      }
    },
    components: {
      'photo-uploader': photouploader, ModelListSelect
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

    }
  }

</script>
<style>
  .customSelect {
    width: 20%;
  }
</style>
