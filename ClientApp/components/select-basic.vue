<template>
  <v-container grid-list-lg>
    <v-layout wrap row>
      <v-flex lg8>
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
              v-if="measurementSets.length > 0"
              class="btn btn-primary"
              outline
              @click="dialogAddToMeasurementSet=true"
            >Добавить к серии</v-btn>
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
                <v-list-tile-content>
                  <v-list-tile-title>Измерение:{{ item.measurementName }}</v-list-tile-title>
                  <v-list-tile-sub-title class="text--primary">Прибор:{{ item.deviceName }}</v-list-tile-sub-title>
                  <v-list-tile-sub-title class="text--primary">Порт:{{ item.portNumber }}</v-list-tile-sub-title>
                </v-list-tile-content>

                <v-list-tile-action>
                  <v-icon
                    color="yellow darken-2"
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
          v-if="measurementSets.length > 0"
          class="btn btn-primary"
          v-on:click="deleteThisMeasurementSet"
        >Удалить текущую серию</v-btn>

        <v-dialog v-model="dialogAddToMeasurementSet" max-width="500px">
          <v-card>
            <v-card-title>Добавление</v-card-title>
            <v-card-text>
              <v-select
                v-model="selectedMeasurementSetInDialog"
                :items="measurementSets"
                no-data-text="Нет данных"
                item-text="name"
                item-value="measurementSetId"
                label="Название серии измерений:"
              ></v-select>
            
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn class="btn btn-primary" outline @click="addToMeasurementSet">Добавить</v-btn>
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
              <v-btn color="primary" outline @click="addNewMeasurementSet">Добавить</v-btn>
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

export default {
  data() {
    return {
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
      selectedGraphic: "",
      selectedGraphicId: "",
      selectedProcess: "",
      selectedCodeProduct: "",
      selectedMeasuredDevice: "",
      measurements: [],
      processes: [],
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
  methods: {

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
              this.selectedMeasurementSetInDialog = this.measurementSets[
                this.measurementSets.length - 1
              ].measurementSetId;
            }

            if (response.status === 200) {
              this.snackbarText = `Серия ${
                this.newMeasurementSetName
              } уже существует`;
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
        if (this.measurementSets.length === 0) {
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
                this.measurementSets.find(
                  _ => _.measurementSetId === this.selectedMeasurementSet
                ).name
              } успешно удалена`;
              this.measurementSets = this.measurementSets.filter(
                _ => _.measurementSetId !== this.selectedMeasurementSet
              );
              if (this.measurementSets.length > 0) {
                this.selectedMeasurementSet = this.measurementSets[0].measurementSetId;
                this.selectedMeasurementSetInDialog = this.measurementSets[0].measurementSetId;
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
            var selectedMeasurementSetId = this.selectedMeasurementSet;
            this.$http.get(`/api/measurementset/getatomicsbyid?measurementsetid=${selectedMeasurementSetId}`).then(response => this.selectedAtomics = response.data);
            
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
      for (let index = 0; index < this.selectedAtomics.length; index++) {
        let response = await this.$http.get(
          `/api/measurement/getpoints?measurementid=${
            this.selectedAtomics[index].measurementId
          }&deviceid=${this.selectedAtomics[index].deviceId}&graphicid=${
            this.selectedAtomics[index].graphicId
          }&port=${this.selectedAtomics[index].portNumber}`
        );
        Object.assign(this.points, response.data);
      }
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
      response = await this.$http.get(
        `/api/measurement/getmaterial?measurementid=${measurementId}`
      );
      this.selectedMaterial = response.data;
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
      var selectedMeasurementSetId = this.selectedMeasurementSet;
      let response = await this.$http.get(
        `/api/measurementset/getatomicsbyid?measurementsetid=${selectedMeasurementSetId}`
      );
      this.selectedAtomics = response.data;
      this.selectedMeasurementSetInDialog = selectedMeasurementSetId;
     
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
        this.selectedMeasurementSetInDialog = this.measurementSets[0].measurementSetId;
      }
    } catch (err) {
      window.alert(err);
      console.log(err);
    }
  }
};
</script>
<style>
.v-speed-dial {
  position: absolute;
}

.v-btn--floating {
  position: relative;
}
.customSelect {
  width: 30%;
}
</style>
