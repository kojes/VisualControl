<template>
  <v-app>
    <div class="container w-100">
      <div class="row">
        <div class="col-6">
          <v-autocomplete v-model="selectedWafer"
                          :items="wafers"
                          :label="`Выберите пластину для просмотра дефектов`">

          </v-autocomplete>
        </div>
        <div class="col-3">
          <v-autocomplete v-model="selectedDie"
                          :items="defectFilter.avbDiesList"
                          :label="`Выберите кристалл`"
                          chips
                          no-data-text="Нет данных"
                          item-text="code"
                          item-value="dieId">

          </v-autocomplete>
        </div>
      </div>
      <div class="row">
        <loading :active.sync="filterLoading"
                 :can-cancel="false"
                 color="#4189C7"
                 loader="bars"
                 :is-full-page="false">

        </loading>
        
        <div class="col-3">
          <v-select v-model="selectedStages"
                    :items="defectFilter.avbStagesList"
                    chips
                    item-text="stageName"
                    no-data-text = "Нет данных"
                    item-value="stageId"
                    label="Выберите тех.этап"
                    multiple>

          </v-select>
        </div>
        <div class="col-3">
          <v-select v-model="selectedDefectType"
                    :items="defectFilter.avbDefectTypesList"
                    chips
                    no-data-text = "Нет данных"
                    item-text="description"
                    item-value="defectTypeId"
                    label="Выберите тип дефекта"
                    multiple>

          </v-select>
        </div>
        <div class="col-3">
          <v-select v-model="selectedDangerLevel"
                    :items="defectFilter.avbDangerLevelList"
                    chips
                    no-data-text = "Нет данных"
                    item-text="specification"
                    item-value="dangerLevelId"
                    label="Выберите опасность дефекта"
                    multiple>

          </v-select>
        </div>
      </div>

      <defect-card :defectId="29"></defect-card>
      <defect-card :defectId="30"></defect-card>
    </div>
    </v-app>
</template>

<script>
  import DefectCard from './defect-card.vue'
  import Loading from 'vue-loading-overlay';
  import 'vue-loading-overlay/dist/vue-loading.css';

  export default {
    components: {
      'defect-card': DefectCard, Loading
    },

    created()
    {
      this.$http.get(`/api/wafer/getall`).then((response) => {
        this.wafers = response.data.map(x=>x.waferId);
      });
    },

    watch:
    {
      selectedWafer: function ()
      {
          let selectedWafer = this.selectedWafer; 
          this.$http.get(`/api/defect/getbywaferid?waferId=${selectedWafer}`).then((response) => {
            this.defects = response.data;
            let defects = this.defects;
            this.filterLoading = true;
            this.$http.get('/api/defectfilter/GetDefectFilter', {
              params: {
                defects: JSON.stringify(this.defects)
              }
            })
              .then((response) => {
                if (response.data.errorCode === "OK") {
                  this.defectFilter = response.data.body;
                }
                else {
                  this.defectFilter = [];
                  this.$swal({
                    type: response.data.responseType,
                    text: 'Дефектов на пластине не найдено',
                    toast: true,
                    showConfirmButton: false,
                    position: 'top-end',
                    timer: 4000
                  });
                }
                this.filterLoading = false;
              });
          });

         
        
      },

      selectedDie: function ()
      {

      }

      

    },

   

    data() {
      return {
        filterLoading: false,
        selectedWafer: null,
        selectedDie: null,
        selectedStages: null,
        selectedDefectType: null,
        selectedDangerLevel: null,
        wafers: [],
        defects: [],
        defectFilter: {},
        dies: []

      }
    }
  }
</script>
