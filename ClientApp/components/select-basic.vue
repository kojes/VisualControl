<template>
  <div class="cascading-dropdown">
    
    <div class="form-group">
      <label for="processSelect">Название процесса:</label>
      <select v-model="selectedProcess" class="form-control customSelect" id="processSelect">
        <option v-for="(option,index) in processes" v-bind:value="option.processId" >{{ option.processName }}</option>
      </select>
    </div>
    <div class="form-group">
      <label for="codeProductSelect">Название шаблона:</label>
      <select v-model="selectedCodeProduct" class="form-control customSelect" id="codeProductSelect">
        <option v-for="(option,index) in codeproducts.filter(x => x.processId === selectedProcess)" v-bind:value="option.idCp" >{{ option.codeProductName }}</option>
      </select>
    </div>

    <div class="form-group">
      <label for="mdSelect">Название измеряемого устройства:</label>
      <select v-model="selectedMeasuredDevice" class="form-control customSelect" id="mdSelect">
        <option v-for="(option,index) in measureddevices.filter(x => x.codeProductId === selectedCodeProduct)" v-bind:value="option.measuredDeviceId">{{ option.name }}</option>
      </select>
    </div>
    
    <div class="form-group">
      <label for="measurementSelect">Название измерения:</label>
      <select v-model="selectedMeasurement" class="form-control customSelect" id="measurementSelect">
        <option v-for="(option,index) in measurements.filter(x => x.measuredDeviceId === selectedMeasuredDevice)" v-bind:value="option.measurementId">{{ option.name }}</option>
      </select>
    </div>
    <div class="form-group">
      <label for="deviceSelect">Прибор:</label>
      <select v-model="selectedDevice" class="form-control customSelect" id="deviceSelect">
        <option v-for="(option,index) in devices"  v-bind:value="option.deviceId">{{ option.model + " IP:" + option.address.split("::")[1] }}</option>
      </select>
    </div>
    <div class="form-group">
      <label for="portSelect">Порт:</label>
      <select v-model="selectedPort" class="form-control customSelect" id="portSelect">
        <option v-for="(option,index) in ports" v-bind:value="option">{{ option }}</option>
      </select>
    </div>
    <div class="form-group">
      <label for="graphicSelect">Характеристика:</label>
      <select v-model="selectedGraphicId" class="form-control customSelect" id="graphicSelect">
        <option v-for="(option,index) in graphics" v-bind:value="option.graphicId">{{ option.russianName}}</option>
      </select>
     </div>
      <div class="form-group">
        <button id="newgraphicButton" class="btn btn-primary" v-on:click="getPoints">Построить график</button> <button id="addgraphicButton" class="btn btn-success" v-on:click="getPoints">Добавить измерение к графику</button>
      </div>
    <div class="form-group">
      <component :is="currentChart" :points="points" :graphic ="selectedGraphic" :devices ="devices"></component>
    </div>
    
    </div>
 
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
