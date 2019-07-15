<template>
  <v-container grid-list-lg>
    <v-layout wrap row>
        <v-btn
              fixed
              dark
              fab
              bottom
              right
              @click.stop="drawer = !drawer"
              color="indigo"
            >
              <v-icon>settings</v-icon>
            </v-btn>
      <v-flex lg8>
        <v-layout row>
          <v-flex lg6>
           
            <v-navigation-drawer
      v-model="drawer"
      fixed
      right
      temporary
    >
      <v-list>
        <v-chip>Сглаживание</v-chip>
        <v-divider></v-divider>
      </v-list>

      <v-list>
        <v-layout>
          <v-flex lg10 offset-lg1>
        <v-checkbox v-model="settings.smoothing.require" label="Сглаживание"></v-checkbox>
        <v-slider
          v-model="settings.smoothing.power"
          :disabled="!settings.smoothing.require"
          thumb-color="indigo"
          thumb-label="always"
          :max="8"
          :min="2"
          :step="2"
        ></v-slider>
        </v-flex>
        </v-layout>
      </v-list>
      <v-list>
        <v-chip>Настройки оси Y</v-chip>
        <v-divider></v-divider>
      </v-list>

       <v-list>
       
        <v-layout>
          <v-flex lg10 offset-lg1>
            <v-checkbox v-model="settings.axisY.strictMinMax" label="Ручная установка границ оси Y"></v-checkbox>
            
                   
           
            <v-text-field
            label="Максимум:"
            :disabled="!settings.axisY.strictMinMax"
            class="numberinput"
            v-on:keypress="isNumber(event)"
            v-model.number="settings.axisY.max"
            outline
          ></v-text-field>

           <v-text-field
            label="Минимум:"
            :disabled="!settings.axisY.strictMinMax"
            v-on:keypress="isNumber(event)"
            v-model.number="settings.axisY.min"
            class="numberinput"
            outline
          ></v-text-field>

          </v-flex>
        </v-layout>
    
      </v-list>
      <v-list class="pa-1">
        <v-divider></v-divider>
         <v-btn
              outline
              class="btn btn-primary"
              v-on:click="saveSettings"
            >Сохранить настройки</v-btn>
       </v-list>
   
    </v-navigation-drawer>
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
            <v-text-field :value="selectedMaterial.name" label="Материал" append-outer-icon="cached" outline readonly @click:append-outer="editMaterial"></v-text-field>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex lg6>
            <v-btn
              outline
              id="newgraphicButton"
              class="btn btn-primary"
              v-on:click="getPoints"
            >Построить график</v-btn>

            <v-btn
              outline
              id="addgraphicButton"
              class="btn btn-success"
              v-on:click="getPoints"
            >Добавить измерение к графику</v-btn>

            <v-btn 
               
              v-if="measurementSets.filter(x => !x.isGenerated).length > 0"
              class="btn btn-primary"
              outline
              @click="dialogAddToMeasurementSet=true"
            >Добавить к серии</v-btn>
          </v-flex>
           <v-flex lg6>
             <v-tooltip right>
              <template v-slot:activator="{ on }">
               
                 <v-chip :color="onlineStatusColor" text-color="white" width="200px" dark v-on="on">
                    {{selectedOnlineStatus.isOnline ? "Онлайн" : "Измерение окончено"}}
                    <v-icon right>{{selectedOnlineStatus.isOnline ? "live_tv" : "close"}}</v-icon>
                  </v-chip>

              </template>
              <span>{{"Начало измерений: " + selectedOnlineStatus.startTime }}</span>
              <v-divider></v-divider>
              <span v-if="!selectedOnlineStatus.isOnline">{{"Конец измерений: " + selectedOnlineStatus.lastTime}}</span>
              <v-divider></v-divider>
              <span>{{"Время испытания в часах: " + Math.floor(selectedOnlineStatus.fullTimeInSeconds / 3600)}}</span>
            </v-tooltip>
           
           </v-flex>
        </v-layout>
      </v-flex>
      <v-flex lg4>
        <v-card>
          <v-toolbar color="indigo" dark>
            <v-select
              v-model="selectedMeasurementSet"
              :items="measurementSets"
              no-data-text="Нет данных"
              item-text="name"
              item-value="measurementSetId"
              label="Название серии измерений:"
            ></v-select>
          </v-toolbar>

          <v-list style="max-height: 500px" class="scroll-y" two-line>
            <template v-for="item in selectedAtomics">
              <v-list-tile :key="item.atomicMeasurementId" ripple>
              <v-list-tile-action>
              <div v-if="item.isOnline">
                  <span class="ringringOnline"></span>
                  <span class="circleOnline"></span>
              </div>
              <div v-else>
                  <span class="circleDead"></span>
              </div>
              </v-list-tile-action>
            
                <v-list-tile-content>
                  <v-list-tile-title>Измерение:{{ item.measurementName }}</v-list-tile-title>
                  <v-list-tile-sub-title class="text--primary">Прибор:{{ item.deviceName }}</v-list-tile-sub-title>
                  <v-list-tile-sub-title class="text--primary">Порт:{{ item.portNumber }}</v-list-tile-sub-title>
                </v-list-tile-content>

                <v-list-tile-action>
                  <v-icon v-if="!measurementSets.find(x => x.measurementSetId === selectedMeasurementSet).isGenerated"
                    color="primary"
                    @click="deleteFromSet(item.atomicMeasurementId)"
                  >delete_outline</v-icon>
                </v-list-tile-action>
              </v-list-tile>
            </template>
          </v-list>
        </v-card>
        <v-btn
          v-if="selectedAtomics.length > 0"
          outline
          class="btn btn-primary"
          v-on:click="getPointsFromMeasurementSet"
        >Построить серию графиков</v-btn>

        <v-btn
          outline
          class="btn btn-primary"
          v-on:click="addNewMeasurementSet"
        >Добавить новую серию</v-btn>

        <v-btn
          outline
          v-if="!measurementSets.find(x => x.measurementSetId == selectedMeasurementSet).isGenerated"
          class="btn btn-primary"
          v-on:click="deleteThisMeasurementSet"
        >Удалить текущую серию</v-btn>

        <v-dialog v-model="dialogAddToMeasurementSet" max-width="500px">
          <v-card>
            <v-card-title>Добавление</v-card-title>
            <v-card-text>
              <v-select
                v-model="selectedMeasurementSetInDialog"
                :items="measurementSets.filter(x => !x.isGenerated)"
                no-data-text="Нет серий для добавления"
                item-text="name"
                item-value="measurementSetId"
                label="Название серии измерений:"
              ></v-select>
            
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn v-if="selectedMeasurementSetInDialog.length > 0" class="btn btn-primary" outline @click="addToMeasurementSet">Добавить</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>

        
        <v-dialog v-model="dialogSelectMaterial" max-width="500px">
          <v-card>
            <v-card-title>Изменение</v-card-title>
            <v-card-text>
              <v-select
                v-model="selectedMaterialInDialog"
                :items="avialiableMaterials"
                no-data-text="Нет данных"
                item-text="name"
                item-value="materialId"
                label="Название материала:"
              ></v-select>
            
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn class="btn btn-primary" outline @click="editMaterial">Изменить</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>

        <v-dialog v-model="dialogAddMeasurementSet" max-width="500px">
          <v-card>
            <v-card-title>Добавление новой серии</v-card-title>
            <v-card-text>
              <v-text-field v-model="newMeasurementSetName" label="Название новой серии"></v-text-field>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn  color="primary" outline @click="addNewMeasurementSet">Добавить</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>

        <v-dialog v-model="dialogDeleteMeasurementSet" max-width="500px">
          <v-card>
            <v-card-title>Удаление серии</v-card-title>
            <v-card-text>Вы действительно хотите удалить серию?</v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="pink" outline @click="deleteThisMeasurementSet">Удалить</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-flex>
    </v-layout>
    <v-layout row>
      <v-flex lg12>
        <div id="chart">
          <component
            :is="currentChart"
            :points="points"
            :graphic="selectedGraphic"
            :devices="avDevices"
            :settings="savedSettings"
          ></component>
        </div>
       
      </v-flex>
    </v-layout>

    <v-snackbar v-model="snackbar" top>
      {{ snackbarText }}
      <v-btn color="pink" flat @click="snackbar = false">Закрыть</v-btn>
    </v-snackbar>
  </v-container>
</template>
<script>

import chart from "./time-chart.vue";
import date from 'date-and-time';
date.locale('ru')
export default {
  data() {
    return {
      drawer: null,
        items: [
          { title: 'Home', icon: 'dashboard' },
          { title: 'About', icon: 'question_answer' }
        ],
      fab: false,
      dialogAddToMeasurementSet: false,
      dialogAddMeasurementSet: false,
      dialogDeleteMeasurementSet: false,
      dialogSelectMaterial: false,
      dialogConfirm: false,
      dialogConfirmText: "",
      snackbar: false,
      snackbarText: "",
      selectedMeasurement: "",
      selectedMaterial: "",
      newMeasurementSetName: "",
      selectedMeasurementSet: "",
      selectedMeasurementSetInDialog: "",
      selectedDevice: "",
      selectedMaterialInDialog:"",
      selectedPort: "",
      selectedOnlineStatus: {},
      selectedGraphic: "",
      selectedGraphicId: "",
      selectedProcess: "",
      selectedCodeProduct: "",
      selectedMeasuredDevice: "",
      measurements: [],
      processes: [],
      settings: {
           smoothing:
           {
               require: true,
               power: 8
           },
           axisY:
           {
               strictMinMax: false,
               min: 0,
               max: 0
              
           }
      },
      savedSettings: {},
      codeproducts: [],
      measureddevices: [],
      measurementSets: [],
      selectedAtomics: [],
      avialiableMaterials: [],
      devices: [],
      graphics: [],
      ports: [],
      points: {},
      avDevices: "",
      currentChart: ""
    };
  },
  components: {
    chart
  },

  computed:
  {
     onlineStatusColor: function()
     {
         if(this.selectedOnlineStatus.isOnline)
         {
            return "green";
         }
         else
         {
            return "pink";
         }
     }

  },
  methods: {

    saveSettings()
    {
       this.savedSettings = JSON.parse(JSON.stringify(this.settings));
       this.drawer = false;
       
    },

     isNumber: function(evt) {
      evt = (evt) ? evt : window.event;
      var charCode = (evt.which) ? evt.which : evt.keyCode;
      if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode !== 46) {
        evt.preventDefault();;
      } else {
        return true;
      }
    },

    editMaterial()
    {
       if (this.dialogSelectMaterial === false) {
       
        this.$http.get(`/api/material/getall`).then(response => this.avialiableMaterials = response.data);
        this.selectedMaterialInDialog = this.selectedMaterial.materialId;
        this.dialogSelectMaterial = true;
      }
      else{
      let changematerialViewModel =
      {
         measurementId: this.selectedMeasurement,
         materialId: this.selectedMaterialInDialog
      }

        this.$http({
        method: "post",
        url: `/api/measurement/changematerial`,
        data: changematerialViewModel,
        config: {
          headers: {
            Accept: "application/json",
            "Content-Type": "application/json"
          }
        }
      })
        .then(response => {
          if (response.status === 200) {
            this.snackbarText = "Успешно изменено";
            this.selectedMaterial = response.data;
            this.dialogSelectMaterial = false;
            
          }

          this.snackbar = true;
        })
        .catch(error => {
          this.snackbar = true;
          this.snackbarText = "Ошибка";
        });
          
      }
    },

    addNewMeasurementSet() {
      if (this.dialogAddMeasurementSet === false) {
        this.dialogAddMeasurementSet = true;
        this.newMeasurementSetName = "";
      } else {
        this.$http({
          method: "put",
          url: `/api/measurementset/AddNewSet`,
          params: { name: this.newMeasurementSetName },
          config: {
            headers: {
              "Content-Type": "text/plain"
            }
          }
        })
          .then(response => {
            if (response.status === 201) {
              this.snackbarText = `Серия ${
                this.newMeasurementSetName
              } успешно добавлена`;
              this.measurementSets.push(response.data);
              this.selectedMeasurementSet = this.measurementSets[
                this.measurementSets.length - 1
              ].measurementSetId;
             
            }

            if (response.status === 200) {
              this.snackbarText = response.data;
            }

            this.dialogAddMeasurementSet = false;
            this.snackbar = true;
          })
          .catch(error => {
            this.snackbar = true;
            this.snackbarText = error;
          });
      }
    },

    deleteThisMeasurementSet() {
      if (this.dialogDeleteMeasurementSet === false) {
        if (this.measurementSets.filter(x => !x.isGenerated).length === 0) {
          this.snackbar = true;
          this.snackbarText = "Нет серий для удаления";
        } else {
          this.dialogDeleteMeasurementSet = true;
        }
      } else {
        this.$http({
          method: "delete",
          url: `/api/measurementset/DeleteSet`,
          params: { measurementsetid: this.selectedMeasurementSet },
          config: {
            headers: {
              "Content-Type": "text/plain"
            }
          }
        })
          .then(response => {
            if (response.status === 200) {
              this.snackbarText = `Серия ${
                this.measurementSets.find(_ => _.measurementSetId === this.selectedMeasurementSet).name} успешно удалена`;
              this.measurementSets = this.measurementSets.filter(_ => _.measurementSetId !== this.selectedMeasurementSet);
              if (this.measurementSets.length > 0) {
                this.selectedMeasurementSet = this.measurementSets[0].measurementSetId;
              
              } else {
                this.selectedAtomics = [];
              }
            }

            this.dialogDeleteMeasurementSet = false;
            this.snackbar = true;
          })
          .catch(error => {
            this.snackbar = true;
            this.snackbarText = error;
          });
      }
    },

    deleteFromSet(key) {
      var atomicsetViewModel = {
        atomicid: key,
        measurementsetid: this.selectedMeasurementSet
      };
      this.$http({
        method: "post",
        url: `/api/measurementset/deleteatomicfromset`,
        data: atomicsetViewModel,
        config: {
          headers: {
            Accept: "application/json",
            "Content-Type": "application/json"
          }
        }
      })
        .then(response => {
          if (response.status === 200) {
            this.snackbarText = "Успешно удалено";
            this.selectedAtomics = this.selectedAtomics.filter(
              i => i.atomicMeasurementId !== key
            );
          }

          this.snackbar = true;
        })
        .catch(error => {
          this.snackbar = true;
          this.snackbarText = "Ошибка";
        });
    },

    addToMeasurementSet() {
      let atomicViewModel = {
        measurementSetId: this.selectedMeasurementSetInDialog,
        measurementId: this.selectedMeasurement,
        portNumber: this.selectedPort,
        graphicId: this.selectedGraphicId,
        deviceId: this.selectedDevice
      };

      this.$http({
        method: "post",
        url: `/api/measurementset/addnewatomictoset`,
        data: atomicViewModel,
        config: {
          headers: {
            Accept: "application/json",
            "Content-Type": "application/json"
          }
        }
      })
        .then(response => {
          if (response.status === 200) {
            this.snackbarText = "Уже добавлен в эту серию";
          }

          if (response.status === 201) {
            this.snackbarText = "Успешно добавлено";
            var route = this.measurementSets.filter(x => x.measurementSetId == this.selectedMeasurementSet)[0].route;
            this.$http.get(`/api/measurementset/getatomics/${route}`).then(response => this.selectedAtomics = response.data);
            
          }

          this.dialogAddToMeasurementSet = false;
          this.snackbar = true;
        })
        .catch(error => {
          this.snackbar = true;
          this.snackbarText = "Ошибка";
          this.dialogAddToMeasurementSet = false;
        });
    },

    getPointsFromMeasurementSet: async function() {
      this.currentChart = "";
      this.points = {};
      let queries = [];
      for (let index = 0; index < this.selectedAtomics.length; index++) {
        let query = this.$http.get(
          `/api/measurement/getpoints?measurementid=${
            this.selectedAtomics[index].measurementId
          }&deviceid=${this.selectedAtomics[index].deviceId}&graphicid=${
            this.selectedAtomics[index].graphicId
          }&port=${this.selectedAtomics[index].portNumber}`
        );
        queries.push(query);
        
      }
      let response = await this.$http.all(queries);
      response.forEach(_ => Object.assign(this.points, _.data))
      this.currentChart = "chart";
      this.$vuetify.goTo('#chart')
    },

    getPoints: async function(event) {
      this.currentChart = "";
      var graphicId = this.selectedGraphicId;
      var deviceId = this.selectedDevice;
      var port = this.selectedPort;
      var measurementId = this.selectedMeasurement;
      if (event.currentTarget.id == "newgraphicButton") {
        this.points = {};
        this.selectedGraphic = this.graphics.find(
          x => x.graphicId === graphicId
        );
      }

      let response = await this.$http.get(
        `/api/measurement/getpoints?measurementid=${measurementId}&deviceid=${deviceId}&graphicid=${graphicId}&port=${port}`
      );
      if (
        !this.points.hasOwnProperty(
          Object.getOwnPropertyNames(response.data)[0]
        )
      ) {
        Object.assign(this.points, response.data);
      } else {
        if (this.selectedGraphic.graphicId === this.selectedGraphicId) {
          this.$swal({
            type: "warning",
            text: "Идентичный график, невозможно добавить",
            toast: true,
            showConfirmButton: false,
            position: "center-start",
            timer: 4000
          });
        } else {
          this.$swal({
            type: "warning",
            text: "Другой тип графика, невозможно добавить",
            toast: true,
            showConfirmButton: false,
            position: "center-start",
            timer: 4000
          });
        }
      }

      for (var prop in this.points) {
        if (this.points[prop].length == 0) {
          delete this.points[prop];
        }
      }
      if (Object.keys(this.points).length > 0) {
        this.currentChart = "chart";
        this.$vuetify.goTo('#chart', { offset: 300 })
        if (response.status === 204) {
          this.$swal({
            type: "warning",
            text: "Измерения не найдены, невозможно добавить",
            toast: true,
            showConfirmButton: false,
            position: "center-start",
            timer: 4000
          });
        }
      } else {
        this.$swal({
          type: "error",
          text: "Измерения не найдены, невозможно построить",
          toast: true,
          showConfirmButton: false,
          position: "center-start",
          timer: 4000
        });
      }
    }
  },

  

  watch: {

    'settings.axisY': 
    {
            handler: function() {

                     
              if(this.settings.axisY.min > this.settings.axisY.max)
              {
                   this.settings.axisY.min = this.settings.axisY.max;
              }
                
            },
            deep: true
    },

    selectedMeasurement: async function() {
      var measurementId = this.selectedMeasurement;
      let response = await this.$http.get(
        `/api/measurement/getextrainfo?measurementid=${measurementId}`
      );
      this.devices = response.data.devices;
      this.selectedDevice = this.devices[0].deviceId;
      this.ports = response.data.ports;
      this.selectedPort = this.ports[0];
      this.graphics = response.data.graphic;
      this.selectedGraphicId = this.graphics[0].graphicId;
      let material = await this.$http.get(
        `/api/measurement/getmaterial?measurementid=${measurementId}`
      );
      this.selectedMaterial = material.data;
      let onlinestatus = await this.$http.get(
        `/api/measurement/getonlinestatus?measurementid=${measurementId}`
      );
      this.selectedOnlineStatus = onlinestatus.data;
      this.selectedOnlineStatus.startTime = date.format(date.parse(this.selectedOnlineStatus.startTime.split('T')[0], 'YYYY-MM-DD'), 'DD MMM YYYY');
      this.selectedOnlineStatus.lastTime = date.format(date.parse(this.selectedOnlineStatus.lastTime.split('T')[0], 'YYYY-MM-DD'), 'DD MMM YYYY');
    },

    selectedProcess: function() {
      let selectedProcess = this.selectedProcess;
      this.selectedCodeProduct = this.codeproducts.filter(
        x => x.processId === selectedProcess
      )[0].idCp;
    },

    selectedCodeProduct: function() {
      let selectedCodeProduct = this.selectedCodeProduct;
      this.selectedMeasuredDevice = this.measureddevices.filter(
        x => x.codeProductId === selectedCodeProduct
      )[0].measuredDeviceId;
    },

    selectedMeasurementSet: async function() {
      var route = this.measurementSets.filter(x => x.measurementSetId == this.selectedMeasurementSet)[0].route;
      let response = await this.$http.get(
        `/api/measurementset/getatomics/${route}`
      );
      this.selectedAtomics = response.data;
     
     
    },

    selectedMeasuredDevice: function() {
      let selectedMeasuredDevice = this.selectedMeasuredDevice;
      this.selectedMeasurement = this.measurements.filter(
        x => x.measuredDeviceId === selectedMeasuredDevice
      )[0].measurementId;
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
      response = await this.$http.get(`/api/device/getall`);
      this.avDevices = response.data;
      response = await this.$http.get(`/api/measurementset/getall`);
      this.measurementSets = response.data;

      if (this.measurementSets.length > 0) {
        this.selectedMeasurementSet = this.measurementSets[0].measurementSetId;
        
      }

      this.savedSettings = JSON.parse(JSON.stringify(this.settings));
    } catch (err) {
      window.alert(err);
      console.log(err);
    }
  }
};
</script>
<style>


.circleOnline {
    width: 15px;
    height: 15px;
    background-color: #62bd19;
    border-radius: 50%;
    position: absolute;
    top: 23px;
    left: 23px;
}

.ringringOnline {
    border: 3px solid #62bd19;
    -webkit-border-radius: 30px;
    border-radius: 30px;
    height: 25px;
    width: 25px;
    position: absolute;
    left: 17.5px;
    top: 17.5px;
    animation: pulsate 2s ease-out;
    -webkit-animation: pulsate 2s ease-out;
    animation-iteration-count: infinite;
    -webkit-animation-iteration-count: infinite; 
    opacity: 0.0
}

.circleDead {
    width: 15px;
    height: 15px;
    background-color: #E91E63;
    border-radius: 50%;
    position: absolute;
    top: 23px;
    left: 23px;
}


@-webkit-keyframes pulsate {
    0% {-webkit-transform: scale(0.1, 0.1); opacity: 0.0;}
    50% {opacity: 1.0;}
    100% {-webkit-transform: scale(1.2, 1.2); opacity: 0.0;}
}

@keyframes pulsate {
    0% {transform: scale(0.1, 0.1); opacity: 0.0;}
    50% {opacity: 1.0;}
    100% {transform: scale(1.2, 1.2); opacity: 0.0;}
}


.v-speed-dial {
  position: absolute;
}

.v-btn--floating {
  position: relative;
}
.customSelect {
  width: 30%;
}

.numberinput input[type='number'] {
    -moz-appearance:textfield;
}
.numberinput input::-webkit-outer-spin-button,
.numberinput input::-webkit-inner-spin-button {
    -webkit-appearance: none;
}
</style>
