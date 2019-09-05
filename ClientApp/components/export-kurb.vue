<template>
    <v-container grid-list-lg>
        <v-layout row>
            <v-flex lg-3 offset-lg1>
                <v-autocomplete
                        v-model="selectedWafer"
                        :items="wafers"
                        no-data-text="Нет данных"
                        item-text="waferId"
                        item-value="waferId"
                        box
                        outline
                        label="Номер пластины"
                      ></v-autocomplete>
                     
            </v-flex>
            <v-flex lg-3>
                <v-select
                        v-model="selectedMeasurementId"
                        :items="measurementRecordings"
                        no-data-text="Нет данных"
                        item-text="name"
                        item-value="id"
                        box
                        outline
                        label="Выберите измерение"
                      ></v-select>
            </v-flex>
        </v-layout>
        <v-layout row>
              <v-flex lg-3 offset-lg1>
                  <v-textarea
                    v-model="statNames"
                    auto-grow
                    box
                    label="Статистические параметры для экспорта"
                    rows="1"
                ></v-textarea>
              </v-flex>
        </v-layout>
         <v-layout row>
             <v-flex lg-3 offset-lg1>
                   <v-btn outline color="primary">                   
                      <download-csv
                        class = "btn btn-primary"
                        :data = "exportData"
                        :name = "filename">                   
                        Экспортировать       
                    </download-csv>
                    </v-btn>
                   
             </v-flex>
         </v-layout>
    </v-container>
</template>


<script>
import JsonCSV from 'vue-json-csv'
export default {

    data() {
        return {
            wafers: ["E740", "E740B", "E740DC_NU2"],
            selectedWafer: "E740",
            statNames: "S21<sub>(2.5GHz)</sub>(коэффициент передачи)$S<sub>22(5GHz)</sub>",
            measurementRecordings: [],
            selectedMeasurementId: "",
            exportData: ""
        }
    },  

    components:
    {
        "download-csv" : JsonCSV
    },

    computed:
    {
        filename()
        {
            if(this.measurementRecordings.length > 0 && this.selectedMeasurementId)
            {
                return this.selectedWafer + "_" + this.measurementRecordings.filter(x => x.id == this.selectedMeasurementId)[0].name.split('.')[1];
            }
            return "";
          
        }
    },

    methods:
    {
       
    },

    watch:
    {
        selectedWafer: function()
        {
            this.$http.get(`/api/measurementrecording/GetMeasurementRecordingsByWaferId?waferId=${this.selectedWafer}`)
                      .then(response => 
                      {
                            this.measurementRecordings = response.data;
                      });
        },

        selectedMeasurementId: function()
        {
             this.$http.get(`/api/export/get/kurbxls`)
                      .then(response => 
                      {
                          let data = response.data;
                          this.exportData = data;
                      });
        }


    },

    async created()
    {
         this.$http.get(`/api/measurementrecording/GetMeasurementRecordingsByWaferId?waferId=${this.selectedWafer}`)
                      .then(response => 
                      {
                            this.measurementRecordings = response.data;
                      });
    }
}
</script>