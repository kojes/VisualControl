<template>

 <v-container grid-list-lg>
    <v-layout row>
      <v-flex lg6>
                        <v-select
                        v-model="selectedProcess"
                        :items="processes"
                        no-data-text="Нет данных"
                        item-text="processName"
                        item-value="processId"
                        box
                        outline
                        label="Название процесса:"
                      ></v-select>


     
      </v-flex>
       <v-flex lg6>
           <v-select
                        v-model="selectedCodeProduct"
                        :items="codeproducts.filter(x => x.processId === selectedProcess)"
                        no-data-text="Нет данных"
                        item-text="codeProductName"
                        item-value="idCp"
                        box
                        outline
                        label="Название шаблона:"
                      ></v-select>
 
       </v-flex>

   </v-layout>

   <v-layout row>
     <v-flex lg6>
        <v-select
                        v-model="selectedMeasuredDevice"
                        :items="measureddevices.filter(x => x.codeProductId === selectedCodeProduct)"
                        no-data-text="Нет данных"
                        item-text="name"
                        item-value="measuredDeviceId"
                        box
                        outline
                        label="Название измеряемого устройства:"
                      ></v-select>
        
     </v-flex>
     <v-flex lg6>
        <v-select
                        v-model="selectedMeasurement"
                        :items="measurements.filter(x => x.measuredDeviceId === selectedMeasuredDevice)"
                        no-data-text="Нет данных"
                        item-text="name"
                        item-value="measurementId"
                        box
                        outline
                        label="Название измерения:"
                      ></v-select>
      
     </v-flex>
   </v-layout>

     <v-layout row>
     <v-flex lg6>
          <v-select
                        v-model="selectedDevice"
                        :items="devices"
                        no-data-text="Нет данных"
                        item-text="address"
                        item-value="deviceId"
                        box
                        outline
                        label="Прибор:"
                      ></v-select>
       
     </v-flex>
     <v-flex lg6>
        <v-select
                        v-model="selectedPort"
                        :items="ports"
                        no-data-text="Нет данных"
                        box
                        outline
                        label="Порт:"
                      ></v-select>
      
     </v-flex>
   </v-layout>

     <v-layout row>
     <v-flex lg6>
        <v-select
                        v-model="selectedGraphicId"
                        :items="graphics"
                        no-data-text="Нет данных"
                        item-text="russianName"
                        item-value="graphicId"
                        box
                        outline
                        label="Характеристика:"
                      ></v-select>
      
            <label for="graphicSelect"></label>
     
     </v-flex>
     <v-flex lg6>
       <v-btn outline id="newgraphicButton" class="btn btn-primary" v-on:click="getPoints">Построить график</v-btn> 
       <v-btn outline id="addgraphicButton" class="btn btn-success" v-on:click="getPoints">Добавить измерение к графику</v-btn>
     </v-flex>
   </v-layout>

    <v-layout row>
    
     <v-flex lg12>
         <component :is="currentChart" :points="points" :graphic ="selectedGraphic" :devices ="devices"></component>
     </v-flex>
   </v-layout>

 </v-container>

</template>
<script>
  import chart from './time-chart.vue';
  props: ['points', 'graphic', 'devices']
 
  export default {
    data() {
      return {
        selectedMeasurement: "",
        selectedDevice: "",
        selectedPort: "",
        selectedGraphic: "",
        selectedGraphicId: "",
        selectedProcess: "",
        selectedCodeProduct: "",
        selectedMeasuredDevice: "",
        measurements: [],
        processes: [],
        codeproducts: [],
        measureddevices: [],
        devices: [],
        graphics: [],
        ports: [],
        points: {},
        currentChart: ""
        
       
      }
    },
    components: {
      chart
    },
    methods:
    {
      
      getPoints: async function (event)
      {

        
          var graphicId = this.selectedGraphicId;
          var deviceId = this.selectedDevice;
          var port = this.selectedPort;
          var measurementId = this.selectedMeasurement;
          if (event.target.id == "newgraphicButton")
          {
            this.points = {};
            this.selectedGraphic = this.graphics.find(x => x.graphicId === graphicId);
          }

        
                 
          this.currentChart = "";
          let response = await this.$http.get(`/api/measurement/getpoints?measurementid=${measurementId}&deviceid=${deviceId}&graphicid=${graphicId}&port=${port}`);
          if (!this.points.hasOwnProperty(Object.getOwnPropertyNames(response.data)[0]))
          {
              Object.assign(this.points, response.data);
          }
          else
          {
            if (this.selectedGraphic.graphicId === this.selectedGraphicId)
            {
              this.$swal({
                type: 'warning',
                text: 'Идентичный график, невозможно добавить',
                toast: true,
                showConfirmButton: false,
                position: 'center-start',
                timer: 4000
              });
            }
            else
            {
              this.$swal({
                type: 'warning',
                text: 'Другой тип графика, невозможно добавить',
                toast: true,
                showConfirmButton: false,
                position: 'center-start',
                timer: 4000
              });
           
            }
          
          }
        
          for (var prop in this.points) {
            if (this.points[prop].length == 0)
            {
                delete this.points[prop];
            }
          }
          if (Object.keys(this.points).length > 0)
          {
            
            this.currentChart = "chart";
            if (response.status === 204)
            {
              this.$swal({
                type: 'warning',
                text: 'Измерения не найдены, невозможно добавить',
                toast: true,
                showConfirmButton: false,
                position: 'center-start',
                timer: 4000
              });
            }
          }
          else
          {
            
            this.$swal({
              type: 'error',
              text: 'Измерения не найдены, невозможно построить',
              toast: true,
              showConfirmButton: false,
              position: 'center-start',
              timer: 4000
            });
          }
          
      }
    },
    watch:
    {
      selectedMeasurement: async function ()
      {
        this.currentChart = "";
        var measurementId = this.selectedMeasurement;
        let response = await this.$http.get(`/api/measurement/getextrainfo?measurementid=${measurementId}`);
        this.devices = response.data.devices;
        this.selectedDevice = this.devices[0].deviceId;
        this.ports = response.data.ports;
        this.selectedPort = this.ports[0];
        this.graphics = response.data.graphic;
        this.selectedGraphicId = this.graphics[0].graphicId;

      },

      selectedProcess: function () {
        let selectedProcess = this.selectedProcess;
        this.selectedCodeProduct = this.codeproducts.filter(x => x.processId === selectedProcess)[0].idCp;
       
      },

      selectedCodeProduct: function () {
        let selectedCodeProduct = this.selectedCodeProduct;
        this.selectedMeasuredDevice = this.measureddevices.filter(x => x.codeProductId === selectedCodeProduct)[0].measuredDeviceId;
      },

      selectedMeasuredDevice: function () {
        let selectedMeasuredDevice = this.selectedMeasuredDevice;
        this.selectedMeasurement = this.measurements.filter(x => x.measuredDeviceId === selectedMeasuredDevice)[0].measurementId;
      }
    },
    async created() {
      try {
       
        let response = await this.$http.get(`/api/measurement/fullinfo`);
        let data = response.data;
        this.processes = data.item1;
        this.codeproducts = data.item2;
        this.measureddevices = data.item3;
        this.measurements = data.item4;
        this.selectedProcess = this.processes[0].processId;
      
      } catch (err) {
        window.alert(err);
        console.log(err);
      }
    }
  }

</script>
<style>
  .customSelect{
    width: 30%;
  }
</style>
