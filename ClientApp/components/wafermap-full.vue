<template>
  <v-container grid-list-lg>
    <v-layout row>
      <v-flex d-flex lg7>
        <wafermap-trtd :defectiveDiesSearchProps="defectiveDiesSearchProps" :waferId="waferId" :streetSize="streetSize" :fieldHeight="fieldHeight" :fieldWidth="fieldWidth">
        </wafermap-trtd>
      </v-flex>
      <v-flex d-flex lg3>
        <v-layout justify-center column>

          <v-flex d-flex>
            <v-layout column>
              <v-flex d-flex>
                <v-card color="#303030" dark>
                  <v-card-text>
                    <v-autocomplete v-model="selectedWafer"
                                    :items="wafers"
                                    box
                                    outline
                                    label="Выберите пластину">
                   </v-autocomplete>
                  </v-card-text>
                </v-card>
              </v-flex>
              <v-flex d-flex>
                <v-card color="#303030" dark>
                  <v-card-text>
                    <v-select v-model="selectedDefectType"
                              :items="availableDefectTypes"
                              no-data-text="Нет данных"
                              item-text="Description"
                              item-value="DefectTypeId"
                              outline
                              :disabled="checkboxAllTypes"
                              :label="selectedDefectTypeLabel">
                    </v-select>
                    <v-checkbox label="Показать все типы дефектов"
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
                              outline
                              :disabled="checkboxOnlyBad"
                              :label="selectedDangerLevelLabel">
                    </v-select>
                    <v-checkbox label="Показать только бракованные"
                                v-model="checkboxOnlyBad">
                    </v-checkbox>
                  </v-card-text>
                </v-card>
              </v-flex>
            </v-layout>

          </v-flex>
          <v-flex d-flex>

          </v-flex>
          <v-flex d-flex>

          </v-flex>
        </v-layout>
      </v-flex>
    </v-layout>
  </v-container>
</template>


<script>
  import WaferMap from './wafermap-trtd.vue'
  export default {

    data() {
      return {

        defectiveDiesSearchProps: {dangerLevel: "", defectType: ""},
        waferId: "",
        streetSize: 7,
        fieldHeight: 840,
        fieldWidth: 840,
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
      'wafermap-trtd': WaferMap
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
         return this.checkboxOnlyBad ? "Выбран" : "Выберите опасность дефекта";
      },

      selectedDefectTypeLabel() {
        return this.checkboxAllTypes ? "Все типы дефектов" : "Выберите тип дефекта";
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
