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
           <v-navigation-drawer
      v-model="drawer"
      fixed
      right
      temporary
    >
     
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
            outlined
          ></v-text-field>

           <v-text-field
            label="Минимум:"
            :disabled="!settings.axisY.strictMinMax"
            v-on:keypress="isNumber(event)"
            v-model.number="settings.axisY.min"
            class="numberinput"
            outlined
          ></v-text-field>

          </v-flex>
        </v-layout>
    
      </v-list>

       <v-list>
        <v-chip>Настройки цвета на графике</v-chip>
        <v-divider></v-divider>
      </v-list>

        <v-list>
          <v-layout row>
          <v-flex lg6 offset-lg1>
           <v-text-field
            label="Цвет фона:"
            disabled
            v-model="settings.colors.backgroundColor"
            outlined
          ></v-text-field>
          </v-flex>
          <v-flex lg4 offset-lg1>
           <swatches v-model="settings.colors.backgroundColor" colors="text-advanced" popover-to="left"></swatches>
          </v-flex>
          </v-layout>

        <v-layout row>
          <v-flex lg6 offset-lg1>
           <v-text-field
            label="Цвет текста:"
            disabled
            v-model="settings.colors.textColor"
            outlined
          ></v-text-field>
          </v-flex>
          <v-flex lg4 offset-lg1>
           <swatches v-model="settings.colors.textColor" colors="text-advanced" popover-to="left"></swatches>
          </v-flex>
          </v-layout>

           <v-layout row>
           <v-flex lg6 offset-lg1>
           <v-text-field
            label="Цвет сетки:"
            disabled
            v-model="settings.colors.gridColor"
            outlined
          ></v-text-field>
          </v-flex>
          <v-flex lg4 offset-lg1>
           <swatches v-model="settings.colors.gridColor" colors="text-advanced" popover-to="left"></swatches>
          </v-flex>     
        </v-layout>

        </v-list>

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

      <v-list class="pa-1">
        <v-divider></v-divider>
         <v-btn
              outlined
              class="btn btn-primary"
              v-on:click="saveSettings"
            >Сохранить настройки</v-btn>
       </v-list>
   
    </v-navigation-drawer>
        <v-layout row>
           <v-flex lg4>          
         
            <v-select
              v-model="selectedFacility"
              :items="facilities"
              no-data-text="Нет данных"
              item-text="name"
              item-value="facilityId"
              filled
              outlined
              label="Название установки:"
            ></v-select>
          </v-flex>
          <v-flex lg4>          
         
            <v-select
              v-model="selectedProcess"
              :items="processes"
              no-data-text="Нет данных"
              item-text="processName"
              item-value="processId"
              filled
              outlined
              label="Название процесса:"
            ></v-select>
          </v-flex>
          <v-flex lg4>
            <v-select
              v-model="selectedCodeProduct"
              :items="codeproducts.filter(x => x.processId === selectedProcess)"
              no-data-text="Нет данных"
              item-text="codeProductName"
              item-value="idCp"
              filled
              outlined
              label="Название шаблона:"
            ></v-select>
          </v-flex>
        </v-layout>

        <v-layout row>
          <v-flex lg4>
            <v-select
              v-model="selectedMeasuredDevice"
              :items="measureddevices.filter(x => x.codeProductId === selectedCodeProduct)"
              no-data-text="Нет данных"
              item-text="name"
              item-value="measuredDeviceId"
              filled
              outlined
              label="Название измеряемого устройства:"
            ></v-select>
          </v-flex>
          <v-flex lg4>
            <v-select 
              v-model="selectedMeasurement"
              :items="measurements.filter(x => x.measuredDeviceId === selectedMeasuredDevice)"
              no-data-text="Нет данных"
              item-text="name"
              item-value="measurementId"
              filled
              outlined
              label="Название измерения:"
            ></v-select>
          </v-flex>
          <v-flex class="justify-center" lg4>
              <v-tooltip right>
              <template v-slot:activator="{ on }">               
                 <v-chip label :color="onlineStatusColor" text-color="white" dark v-on="on">
                    {{selectedOnlineStatus.isOnline ? "Онлайн" : "Остановлено"}}
                    <v-icon right>{{selectedOnlineStatus.isOnline ? "live_tv" : "stop_screen_share"}}</v-icon>
                  </v-chip>
              </template>
              <span>{{"Начало измерений: " + selectedOnlineStatus.startTime }}</span>
              <v-divider></v-divider>
              <span v-if="!selectedOnlineStatus.isOnline">{{"Конец измерений: " + selectedOnlineStatus.lastTime}}</span>
              <v-divider></v-divider>
              <span>{{"Время испытания в часах: " + Math.ceil(selectedOnlineStatus.fullTimeInSeconds / 3600)}}</span>
            </v-tooltip>
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
              filled
              outlined
              label="Прибор:"
            ></v-select>
          </v-flex>
          <v-flex lg6>
            <v-select
              v-model="selectedPort"
              :items="ports"
              no-data-text="Нет данных"
              filled
              outlined
              label="Порт:"
            ></v-select>
          </v-flex>
        </v-layout>

        <v-layout row>
          <v-flex lg6>
            <v-select
              v-model="selectedGraphic"
              :items="graphics"
              no-data-text="Нет данных"
              item-text="russianName"
              item-value="graphicId"
              filled
              outlined
              label="Характеристика:"
            ></v-select>

            <label for="graphicSelect"></label>
          </v-flex>
          <v-flex lg6>
            <v-text-field :value="selectedMaterial.name" label="Материал" append-outer-icon="cached" outlined readonly @click:append-outer="editMaterial"></v-text-field>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex lg6>
            <v-btn
              outlined
              id="newgraphicButton"
              class="btn btn-primary"
              v-on:click="getPoints"
            >Построить график</v-btn>

            <v-btn
              outlined
              id="addgraphicButton"
              class="btn btn-success"
              v-on:click="getPoints"
            >Добавить измерение к графику</v-btn>

          
          </v-flex>
           <v-flex lg3>
               <v-btn 
               
              v-if="measurementSets.filter(x => !x.isGenerated).length > 0"
              class="btn btn-primary"
              outlined
              @click="dialogAddToMeasurementSet=true"
            >Добавить к серии</v-btn>
                </v-flex>
                   <v-flex lg3>
            
           
           </v-flex>
        </v-layout>
      </v-flex>
      <v-flex lg4>
        <v-card>
          <v-toolbar color="indigo" dark>
            <div class="pt-8">
              <v-select
                outlined
                v-model="selectedMeasurementSet.id"
                :items="measurementSets"
                no-data-text="Нет данных"
                item-text="name"
                item-value="measurementSetId"
                label="Название серии измерений:"
              ></v-select>
            </div>
          </v-toolbar>

          <v-list rounded style="max-height: 500px" class="overflow-y-auto" two-line>
            <template v-for="item in selectedAtomics">
              <v-list-item :key="item.atomicMeasurementId" ripple>
              <v-list-item-action>
              <div v-if="item.isOnline">
                  <span class="ringringOnline"></span>
                  <span class="circleOnline"></span>
              </div>
              <div v-else>
                  <span class="circleDead"></span>
              </div>
              </v-list-item-action>
            
                <v-list-item-content>
                  <v-list-item-title>{{ item.measurementName }}</v-list-item-title>
                  <v-list-item-subtitle class="text--primary">Прибор: {{ item.deviceName }} Порт: {{ item.portNumber }}</v-list-item-subtitle>
                 
                </v-list-item-content>

                <v-list-item-content>
                
                  <div v-if="item.isOnline">

                   
                       <v-progress-circular  v-if="item.live == '0'"
                        indeterminate
                        color="light-green accent-3"
                      ></v-progress-circular>
                      <strong class="yellow--text text--darken-2" v-else>{{item.live}} {{item.graphicUnit}}</strong>
                    
                    
                  </div>
              
  
            
              </v-list-item-content>

                <v-list-item-action>
                  <v-icon v-if="measurementSets.length > 0 && !measurementSets.find(x => x.measurementSetId === selectedMeasurementSet.id).isGenerated"
                    color="primary"
                    @click="deleteFromSet(item.atomicMeasurementId)"
                  >delete_outline</v-icon>
                </v-list-item-action>
              </v-list-item>
            </template>
          </v-list>
        </v-card>
        <v-btn
          v-if="selectedAtomics.length > 0"
          outlined
          class="btn btn-primary"
          v-on:click="getPointsFromMeasurementSet"
        >Построить серию</v-btn>

          <v-btn
          outlined
          class="btn btn-primary"
          v-on:click="getMeasurementStatistics"
        >Статистика по серии</v-btn>

        <v-btn
          outlined
          class="btn btn-primary"
          v-on:click="addNewMeasurementSet"
        >Добавить новую серию</v-btn>

        
      

        <v-btn
          outlined
          v-if="measurementSets.length > 0 && !measurementSets.find(x => x.measurementSetId == selectedMeasurementSet.id).isGenerated"
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
                <v-btn v-if="selectedMeasurementSetInDialog.length > 0" class="btn btn-primary" outlined @click="addToMeasurementSet">Добавить</v-btn>
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
                <v-btn class="btn btn-primary" outlined @click="editMaterial">Изменить</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>

      <v-dialog
      v-model=" dialogStatistics"
      hide-overlay
      persistent
      width="300"
    >
      <v-card
        color="indigo"
        dark
      >
        <v-card-text>
          Расчет статистики...
          <v-progress-linear
            indeterminate
            color="white"
            class="mb-0"
          ></v-progress-linear>
        </v-card-text>
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
              <v-btn  color="primary" outlined @click="addNewMeasurementSet">Добавить</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>

        <v-dialog v-model="dialogDeleteMeasurementSet" max-width="500px">
          <v-card>
            <v-card-title>Удаление серии</v-card-title>
            <v-card-text>Вы действительно хотите удалить серию?</v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="pink" outlined @click="deleteThisMeasurementSet">Удалить</v-btn>
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
            :graphic="graphics.find(x => x.graphicId === selectedGraphic)"
            :devices="avDevices"
            :settings="savedSettings"
          ></component>
        </div>
       
      </v-flex>
    </v-layout>

    <v-layout row>
      <v-flex lg12>
       <v-data-iterator
       v-if="selectedMeasurementSet.statistics.length > 0"
      :items="selectedMeasurementSet.statistics"
      content-tag="v-layout"
      hide-actions
      row
      wrap
    >
     <template v-slot:header>
        <v-toolbar
          class="mb-2"
          color="indigo"
          dark
          flat
        >
        <v-toolbar-title>Подробная статистика</v-toolbar-title>
        </v-toolbar>
      </template>
      <template v-slot:item="props">
        <v-flex lg3> 
          
          <v-card>
            <v-card-title><h4>Испытание: {{ props.item.measurementName }}</h4></v-card-title>
            <v-divider></v-divider>
            <v-list dense>
              <v-list-item>
                <v-list-item-content>Прибор:</v-list-item-content>
                <v-list-item-content class="align-end">Название: {{ props.item.deviceName }} Порт:{{ props.item.portNumber }}</v-list-item-content>
              </v-list-item>           
              
              <v-list-item>
                <v-list-item-content>Значение в начале испытания:</v-list-item-content>
                <v-list-item-content class="align-end yellow--text text--darken-2">{{  parseFloat(props.item.firstValue).toFixed(6) }} {{ props.item.graphicUnit }}</v-list-item-content>
              </v-list-item>

              <v-list-item>
                <v-list-item-content>Текущее значение:</v-list-item-content>
                <v-list-item-content class="align-end yellow--text text--darken-2">{{  parseFloat(props.item.lastValue).toFixed(6) }} {{ props.item.graphicUnit }}</v-list-item-content>
              </v-list-item>


               <v-list-item>
                <v-list-item-content>Падение за время испытания:</v-list-item-content>
                <v-list-item-content class="align-end yellow--text text--darken-2">{{ parseFloat(props.item.commonDifference).toFixed(6) * -1 }} {{ props.item.graphicUnit }}</v-list-item-content>
              </v-list-item>

                <v-list-item>
                <v-list-item-content>Изменение в процентах:</v-list-item-content>
                <v-list-item-content class="align-end yellow--text text--darken-2">{{ ((parseFloat(props.item.commonDifference) / parseFloat(props.item.firstValue)) * 100).toFixed(2) }} %</v-list-item-content>
              </v-list-item>
           
            </v-list>
          </v-card>
        </v-flex>
      </template>
    </v-data-iterator>
    </v-flex>
    </v-layout>

    <v-snackbar v-model="snackbar" top>
      {{ snackbarText }}
      <v-btn color="pink" text @click="snackbar = false">Закрыть</v-btn>
    </v-snackbar>

 

  </v-container>
</template>
<script>

import chart from "./time-chart.vue";
import date from 'date-and-time';
import * as signalR from '@aspnet/signalr';
import Swatches from 'vue-swatches' 
import "vue-swatches/dist/vue-swatches.min.css"

date.locale('ru');

export default {
  data() {
    return {
      drawer: null,
       fab: false,
      dialogAddToMeasurementSet: false,
      dialogStatistics: false,
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
      selectedMeasurementSet: {
         id: '',
         statistics: []
      },
      selectedMeasurementSetInDialog: "",
      selectedMaterialInDialog:"",
      selectedOnlineStatus: {},
      selectedFacility: 0,
      selectedProcess: "",
      selectedCodeProduct: "",
      selectedMeasuredDevice: "",
      measurements: [],
      processes: [],
      settings: {
           smoothing:
           {
               require: false,
               power: 8
           },
           axisY:
           {
               strictMinMax: false,
               min: 0,
               max: 0
              
           },
           colors:
           {
              backgroundColor: "#303030",
              textColor: "#ffffff",
              gridColor: "#ffcc00"
           }
      },
      savedSettings: {},
      codeproducts: [],
      measureddevices: [],
      facilities: [],
      measurementSets: [],
      selectedAtomics: [],
      avialiableMaterials: [],
      devices: [],
      graphics: [],
      ports: [],
      polling: null,
      connection: null,
      livepoints: [],
      points: {},
      avDevices: "",
      currentChart: ""
    };
  },
  components: {
    chart, Swatches 
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
     },

     selectedDevice : function()
     {
         return this.devices === undefined || this.devices.length === 0  ? 0 : this.devices[0].deviceId
     },
     
     selectedGraphic: function()
     {
        return this.graphics === undefined || this.graphics.length === 0 ? 0 : this.graphics[0].graphicId
     },

     selectedPort: function()
     {
         return this.ports === undefined || this.ports.length === 0 ? 0 : this.ports[0]
     }

  },
  methods: {

    pollData () {
        this.polling = setInterval(() => {
            
            this.connection.send("getLastValues", this.selectedAtomics);
             
        }, 10000)
   },
   
   livePointsRedraw(liveArray)
   {
       for (let index = 0; index < liveArray.length; index++) {
         
          this.selectedAtomics.find(x => x.measurementId == liveArray[index].measurementId)["live"] = liveArray[index].value;
         
       }      

   },

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

    getMeasurementStatistics()
    {
         this.dialogStatistics= true;
         let atomicList = JSON.stringify(this.selectedAtomics);
         this.$http.get(`/api/measurement/getmeasurementstatistics?atomiclist=${atomicList}`).then(response => {this.selectedMeasurementSet.statistics = response.data; this.dialogStatistics = false});
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
        url: `/api/material/changematerial`,
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
              this.selectedMeasurementSet.id = this.measurementSets[
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
          params: { measurementsetid: this.selectedMeasurementSet.id },
          config: {
            headers: {
              "Content-Type": "text/plain"
            }
          }
        })
          .then(response => {
            if (response.status === 200) {
              this.snackbarText = `Серия ${
                this.measurementSets.find(_ => _.measurementSetId === this.selectedMeasurementSet.id).name} успешно удалена`;
              this.measurementSets = this.measurementSets.filter(_ => _.measurementSetId !== this.selectedMeasurementSet.id);
              if (this.measurementSets.length > 0) {
                this.selectedMeasurementSet.id = this.measurementSets[0].measurementSetId;
              
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
        measurementsetid: this.selectedMeasurementSet.id
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
        graphicId: this.selectedGraphic,
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
            var route = this.measurementSets.filter(x => x.measurementSetId == this.selectedMeasurementSet.id)[0].route + "/" + this.selectedFacility;
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
          `/api/point/get/withoutspaces?measurementid=${
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
      var graphicId = this.selectedGraphic;
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
        `/api/point/get/withoutspaces?measurementid=${measurementId}&deviceid=${deviceId}&graphicid=${graphicId}&port=${port}`
      );
      if (
        !this.points.hasOwnProperty(
          Object.getOwnPropertyNames(response.data)[0]
        )
      ) {
        Object.assign(this.points, response.data);
      } else {
        if (this.selectedGraphic.graphicId === this.selectedGraphic) {
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

    selectedFacility: async function(val, oldVal)
    {
      let response = await this.$http.get(`/api/measurement/fullinfo/${val}`);
      let data = response.data;
      this.processes = data.item1;
      this.codeproducts = data.item2;
      this.measureddevices = data.item3;
      this.measurements = data.item4;
      this.selectedProcess = this.processes[0].processId;
      response = await this.$http.get(`/api/device/all`);
      this.avDevices = response.data;
      response = await this.$http.get(`/api/measurementset/getall/${val}`);
      this.measurementSets = response.data;
      this.selectedMeasuredDevice = this.measureddevices.filter(
        x => x.codeProductId === this.selectedCodeProduct
      )[0].measuredDeviceId
      if (this.measurementSets.length > 0) {
        this.selectedMeasurementSet.id = this.measurementSets[0].measurementSetId;
        
      }

    },

    selectedMeasurement: async function() {

      var measurementId = this.selectedMeasurement;
      let self = this;
      await this.$http.get(`/api/device/av/measurementid/${measurementId}`).then(function (response) {self.devices = response.data});
      await this.$http.get(`/api/graphic/av/measurementid/${measurementId}`).then(response => this.graphics = response.data); 
      await this.$http.get(`/api/measurement/getports/av/${measurementId}`).then(function (response) {this.ports = response.data}.bind(this)); 
    
          

      let material = await this.$http.get(
        `/api/material/getbymeasurementid/${measurementId}`
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

    'selectedMeasurementSet.id': async function() {
      this.selectedMeasurementSet.statistics = [];
      let route = this.measurementSets.filter(x => x.measurementSetId == this.selectedMeasurementSet.id)[0].route + "/" + this.selectedFacility;
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
      
      let response = await this.$http.get(`/api/facility/getall`);
     
      if(response.status == 200)
      {
          let data = response.data;
          this.facilities = data;
          this.selectedFacility = this.facilities[0].facilityId;      
          this.connection = new signalR.HubConnectionBuilder()
                                            .withUrl("/livepoint")
                                            .build();

          this.connection.start().catch(err => console.log(err));
          this.connection.on("lastValues", value => this.livePointsRedraw(value));     
          this.savedSettings = JSON.parse(JSON.stringify(this.settings));
      }
      else
      {
         
          this.snackbar = true;
          this.snackbarText = response.statusText;
          
      }
    

      this.pollData()

    } catch (error) {
        this.snackbar = true;
        this.snackbarText = error;
    }
  }
};
</script>
<style>



.circleOnline {
    width: 15px;
    height: 15px;
    background-color: #76FF03;
    border-radius: 50%;
    position: absolute;
    top: 23px;
    left: 23px;
}

.ringringOnline {
    border: 3px solid #76FF03;
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
