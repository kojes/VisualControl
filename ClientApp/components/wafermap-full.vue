<template>
  <v-container grid-list-lg>
    <v-layout row>
      <v-flex d-flex lg7>
        <wafermap-trtd @show-footer="showDefects" :defectiveDiesSearchProps="defectiveDiesSearchProps" :waferId="waferId" :streetSize="streetSize" :fieldHeight="fieldHeight" :fieldWidth="fieldWidth">
        </wafermap-trtd>
      </v-flex>
      <v-flex d-flex lg5>
        <v-layout justify-center column>

       
          <v-flex d-flex>
            <v-tabs background-color="indigo"
                    dark
                    slider-color="primary"
                    icons-and-text>


              <v-tab href="#wafer">
                Выбор пластины
                <v-icon>table_chart</v-icon>
              </v-tab>

              <v-tab href="#parameters">
                Выбор параметров
                <v-icon>opacity</v-icon>
              </v-tab>

              <v-tab href="#statistics">
                Статистика по пластине
                <v-icon>watch_later</v-icon>
              </v-tab>

              <v-tab-item value="wafer">
                <v-card color="#303030" dark>
                  <v-card-text>
                    <v-autocomplete v-model="selectedWafer"
                                    :items="wafers"
                                    no-data-text="Нет данных"
                                    filled
                                    outlined
                                    label="Номер пластины">
                    </v-autocomplete>
                  </v-card-text>
                </v-card>
              </v-tab-item>
              <v-tab-item value="parameters">
              <v-layout>
                <v-flex d-flex>
                  <v-card color="#303030" dark>
                    <v-card-text>
                      <v-select v-model="selectedDefectType"
                                :items="availableDefectTypes"
                                no-data-text="Нет данных"
                                item-text="Description"
                                item-value="DefectTypeId"
                                outlined
                                :disabled="checkboxAllTypes"
                                :label="selectedDefectTypeLabel">
                      </v-select>
                      <v-checkbox color="primary" label="Показать все типы дефектов"
                                  v-model="checkboxAllTypes">
                      </v-checkbox>
                    </v-card-text>
                  </v-card>
                </v-flex>
                <v-flex d-flex>
                  <v-card color="#303030" dark>
                    <v-card-text>
                      <v-select v-model="selectedDangerLevel"
                                :items="availableDangerLevels"
                                no-data-text="Нет данных"
                                item-text="Specification"
                                item-value="DangerLevelId"
                                outlined
                                :disabled="checkboxOnlyBad"
                                :label="selectedDangerLevelLabel">
                      </v-select>
                      <v-checkbox color="primary" label="Показать только бракованные"
                                  v-model="checkboxOnlyBad">
                      </v-checkbox>
                    </v-card-text>
                  </v-card>
                  </v-flex>
                 </v-layout>
             </v-tab-item>
              <v-tab-item value="statistics">
                
                  
                    <v-card  color="#303030" dark>
                      <v-card-text>
                        <donut-bg :routeApi="badgood3DChartApi" :parametersApi="badgood3DChartParameters" :waferId="selectedWafer"></donut-bg>
                      </v-card-text>
                     </v-card>
                  
</v-tab-item>

            </v-tabs>
          </v-flex>
         
        </v-layout>
      </v-flex>
    </v-layout>
    <v-bottom-sheet persistent v-model="footer">
     
  <v-container fill-height class="bottomsheet" grid-list-lg>
   
  <v-layout row wrap>
   
    <v-toolbar flat class="transparent">
     

      <v-spacer></v-spacer>
      <v-btn icon color="pink" @click="footer = false">
        <v-icon>close</v-icon>
      </v-btn>
    </v-toolbar>
    <v-flex lg3 v-for="defect in selectedDefects.defects" :key="defect.defectId">
     
      <defect-card :defectId="defect.defectId" :dieCode="selectedDefects.dieCode"></defect-card>
      <v-divider light></v-divider>

    </v-flex>
  </v-layout>
  </v-container>

  </v-bottom-sheet>
  </v-container>
</template>


<script>
  import WaferMap from './wafermap-trtd.vue'
  import DefectCard from './defect-card.vue'
  import BadGoodDonutChart from './donut-amcharts.vue';
  export default {

    data() {
      return {

        defectiveDiesSearchProps: {dangerLevel: "", defectType: ""},
        waferId: "",
        footer: false,
        streetSize: 6,
        fieldHeight: 720,
        fieldWidth: 720,
        selectedDefects: {},
        selectedWafer: "",
        selectedDefectType: "all",
        selectedDangerLevel: 1,
        wafers: [],
        availableDefectTypes: [],
        availableDangerLevels: [],
        checkboxAllTypes: true,
        checkboxOnlyBad: true
        

      }
    },

    components: {
      'wafermap-trtd': WaferMap, 'defect-card': DefectCard, 'donut-bg': BadGoodDonutChart
    },

    methods:
    {
      showDefects(selectedDefects)
      {
        this.footer = true;
        this.selectedDefects = selectedDefects;
        
      }
    },


    created()
    {
      this.$http.get(`/api/wafer/getallwithdefects`).then((response) => {
        this.wafers = response.data;
      });
    },

    computed:
    {
      selectedDangerLevelLabel()
      {
         return this.checkboxOnlyBad ? "Выбрано" : "Выберите опасность дефекта";
      },

      selectedDefectTypeLabel() {
        return this.checkboxAllTypes ? "Все типы дефектов" : "Выберите тип дефекта";
      },

      badgood3DChartApi()
      {
        return '/api/chart/getbadgood';
      },

      badgood3DChartParameters()
      {
          return { type: "amcharts" };
      }

    },

    watch:
    {
      selectedWafer: function (val, oldVal) {
        if (val != null) {
          this.waferId = this.selectedWafer;
          this.checkboxOnlyBad = true;
          this.checkboxAllTypes = true;
         

        }
      },

      checkboxOnlyBad: function ()
      {
         this.selectedDangerLevel = 1;
      },


      checkboxAllTypes: function ()
      {
         this.selectedDefectType = "all";
      },

      selectedDefectType: function ()
      {
         this.defectiveDiesSearchProps = { dangerLevel: this.selectedDangerLevel, defectType: this.selectedDefectType };
      },

      selectedDangerLevel: function ()
      {
         this.defectiveDiesSearchProps = { dangerLevel: this.selectedDangerLevel, defectType: this.selectedDefectType };
      },

      waferId: async function ()
      {

           await this.$http.get(`/api/defecttype/getbywaferid?waferId=${this.waferId}`)
             .then((response) => {
               this.availableDefectTypes = response.data;
          })
          .catch((error) => {
          
          });

           await this.$http.get(`/api/dangerlevel/getbywaferid?waferId=${this.waferId}`)
             .then((response) => {
               this.availableDangerLevels = response.data;
          })
          .catch((error) => {


             });
          this.defectiveDiesSearchProps = { dangerLevel: 1, defectType: "all" };
      }
    }


  }

</script>

<style scoped>
  .bottomsheet {
    background-color: rgba(48, 48, 48, 0.9);
    
  }
</style>
