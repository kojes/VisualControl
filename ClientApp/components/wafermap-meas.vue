<template>
  <v-container grid-list-lg>
    <loading
      :active.sync="loading"
      :can-cancel="false"
      color="#fc0"
      :width="64"
      :height="64"
      loader="bars"
      :is-full-page="true"
    ></loading>
    <v-layout row wrap>
      <v-flex d-flex lg4>
        <v-layout justify-center column>
          <v-flex d-flex>
            <v-tabs color="indigo" dark slider-color="primary" icons-and-text>
              <v-tab href="#wafer">
                Выбор пластины
                <v-icon>table_chart</v-icon>
              </v-tab>

              <v-tab href="#extra">
                Доп.настройки
                <v-icon>timeline</v-icon>
              </v-tab>

              <v-tab href="#statistics">
                Статистика по годным
                <v-icon>equalizer</v-icon>
              </v-tab>

              <v-tab-item value="wafer">
                <v-card color="#303030" dark>
                  <v-card-text>
                    <v-layout justify-center column>
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
                    </v-layout>
                  </v-card-text>
                </v-card>
              </v-tab-item>
              <v-tab-item value="extra">
                <v-card color="#303030" dark>
                  <v-card-text>
                    <v-layout justify-center column>
                      <v-flex d-flex lg6>
                        <v-select
                          v-model="selectedGraphics"
                          :items="availiableGraphics"
                          chips
                          no-data-text="Нет данных"
                          item-text="graphicName"
                          item-value="keyGraphicState"
                          item-disabled="disabled"
                          label="Выберите графики"
                          multiple
                        >
                          <template v-slot:prepend-item>
                            <v-list-tile ripple @click="selectAllGraphics">
                              <v-list-tile-action>
                                <v-icon
                                  :color="selectedGraphics.length > 0 ? 'primary' : ''"
                                >{{ selectedGraphicsIcon }}</v-icon>
                              </v-list-tile-action>
                              <v-list-tile-content>
                                <v-list-tile-title>Выбрать все</v-list-tile-title>
                              </v-list-tile-content>
                            </v-list-tile>
                            <v-divider class="mt-2"></v-divider>
                          </template>
                          <template slot="selection" slot-scope="{ item, index }">
                            <v-chip v-if="index < 5">
                              <span>{{ item.graphicName }}</span>
                            </v-chip>
                          </template>
                        </v-select>
                      </v-flex>
                      <v-flex d-flex lg6>
                        <v-select
                          v-model="selectedDivider"
                          :items="dividers"
                          no-data-text="Нет данных"
                          item-text="name"
                          item-value="dividerK"
                          box
                          outline
                          label="Выберите приведение к мм"
                        ></v-select>
                      </v-flex>
                    </v-layout>
                  </v-card-text>
                </v-card>
              </v-tab-item>
              <v-tab-item value="statistics">
                <v-layout>
                  <v-flex d-flex>
                    <v-card color="#303030" dark>
                      <v-card-title primary-title>
                        <span class="font-weight-light">Годные по статистике</span>
                      </v-card-title>
                      <v-card-text>
                        <v-progress-circular
                          :rotate="360"
                          :size="90"
                          :width="7"
                          :value="dirtyCells.statPercentage"
                          color="primary"
                        >{{ dirtyCells.statPercentage + "%" }}</v-progress-circular>
                        <v-btn outline color="primary" @click="delDirtyCells(dirtyCells.statList)">
                          <v-icon>cached</v-icon>
                        </v-btn>
                      </v-card-text>
                    </v-card>
                  </v-flex>
                  <v-flex d-flex>
                    <v-card color="#303030" dark>
                      <v-card-title primary-title>
                        <span class="font-weight-light">Годные по границам</span>
                      </v-card-title>
                      <v-card-text>
                        <v-progress-circular
                          :rotate="360"
                          :size="90"
                          :width="7"
                          :value="dirtyCells.fixedPercentage"
                          color="indigo lighten-4"
                        >{{ dirtyCells.fixedPercentage + "%" }}</v-progress-circular>
                        <v-btn
                          outline
                          color="indigo lighten-4"
                          @click="delDirtyCells(dirtyCells.fixedList)"
                        >
                          <v-icon>cached</v-icon>
                        </v-btn>
                      </v-card-text>
                    </v-card>
                  </v-flex>
                </v-layout>
              </v-tab-item>
            </v-tabs>
          </v-flex>
        </v-layout>
      </v-flex>
      <v-flex d-flex lg4 offset-lg2>
        <wafermap-svg
          :waferId="selectedWafer"
          :avbSelectedDies="avbSelectedDies"
          :streetSize="streetSize"
          :fieldHeight="fieldHeight"
          :fieldWidth="fieldWidth"
        ></wafermap-svg>
      </v-flex>
      <v-flex d-flex lg2>
        <v-card color="#303030" dark v-if="avbSelectedDies.length > 0">
          <v-card-title primary-title>
            <span
              class="font-weight-light"
            >{{"Выбрано " + selectedDies.length + " из " + avbSelectedDies.length + " кристаллов" }}</span>
          </v-card-title>
          <v-card-text class="pa-3">
            <v-progress-circular
              :rotate="360"
              :size="90"
              :width="7"
              :value="(selectedDies.length / avbSelectedDies.length)*100"
              color="primary"
            >{{ Math.ceil((selectedDies.length / avbSelectedDies.length)*100) + "%" }}</v-progress-circular>
          </v-card-text>
          <v-card-actions>
            <v-btn outline color="primary" @click="selectAllDies()">Выбрать все кристаллы</v-btn>
          </v-card-actions>
        </v-card>
      </v-flex>
    </v-layout>

    <v-layout row wrap v-for="(keyGraphicState, key) in selectedGraphics" :key="`kgs-${key}`">
      <v-flex d-flex lg8>
        <stat-single
          :measurementId="selectedMeasurementId"
          :keyGraphicState="keyGraphicState"
          :divider="selectedDivider"
        ></stat-single>
        <v-divider light></v-divider>
      </v-flex>
      <v-flex d-flex lg4>
        <chart-lnr
          v-if="keyGraphicState.includes(`LNR`)"
          :measurementId="selectedMeasurementId"
          :keyGraphicState="keyGraphicState"
          :divider="selectedDivider"
        ></chart-lnr>
        <chart-hstg
          v-else
          :measurementId="selectedMeasurementId"
          :keyGraphicState="keyGraphicState"
          :divider="selectedDivider"
        ></chart-hstg>
        <v-divider light></v-divider>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
import ChartLNR from "./chart-lnr-cjs.vue";
import ChartHSTG from "./chart-bar-cjs.vue";
import StatSingle from "./stat-single.vue";
import Loading from "vue-loading-overlay";
import WaferMap from "./wafermap-svg.vue";
export default {
  data() {
    return {
      loading: false,
      wafers: [],
      dividers: [],
      avbSelectedDies: [],
      measurementRecordings: [],
      availiableGraphics: [],
      selectedWafer: "",
      selectedDivider: "1.0",
      selectedMeasurementId: 0,
      selectedGraphics: [],
      dirtyCells: [],
      streetSize: 3,
      fieldHeight: 320,
      fieldWidth: 320
    };
  },

  components: {
    "stat-single": StatSingle,
    "wafermap-svg": WaferMap,
    "chart-lnr": ChartLNR,
    "chart-hstg": ChartHSTG,
    Loading
  },

  async created() {
    await this.$http.get(`/api/wafer/getall`).then(response => {
      this.wafers = response.data;
    });

    await this.$http.get(`/api/divider/all`).then(response => {
      this.dividers = response.data;
    });
  },

  computed: {
    selectedDies() {
      return this.$store.state.wafermeas.selectedDies;
    },
    selectedGraphicsIcon() {
      if (this.availiableGraphics.length === this.selectedGraphics.length)
        return "check_box";
      if (this.selectedGraphics.length > 0) return "indeterminate_check_box";
      return "check_box_outline_blank";
    }
  },

  methods: {
    delDirtyCells: function(dirtyCellsList) {
      var deletedDies = dirtyCellsList;
      var selectedDies = this.selectedDies.filter(
        el => !deletedDies.includes(el)
      );
      this.$store.commit("wafermeas/updateSelectedDies", selectedDies);
    },

    selectAllDies: function() {
      this.$store.commit(
        "wafermeas/updateSelectedDies",
        this.avbSelectedDies.slice()
      );
    },

    selectAllGraphics: function() {
      this.$nextTick(() => {
        if (this.selectedGraphics.length === this.availiableGraphics.length) {
          this.selectedFruits = [];
        } else {
          var kgsArray = this.availiableGraphics.map(function(g) {
            return g.keyGraphicState;
          });
          this.selectedGraphics = kgsArray;
        }
      });
    }
  },

  watch: {
    selectedWafer: function() {
      this.availiableGraphics = []
      this.$http
        .get(`/api/measurementrecording?waferId=${this.selectedWafer}`)
        .then(response => {
          this.measurementRecordings = response.data;
        });
    },

    selectedMeasurementId: function() {
      this.availiableGraphics = []
      this.loading = true
      this.$http.get(`api/dievalue/GetByMeasurementRecordingId?measurementRecordingId=${this.selectedMeasurementId}`)
        .then(response => {
          ///Можно и не считывать DieValueList
          var dieValues = response.data;
          var keyGraphicStateJSON = JSON.stringify(Object.keys(dieValues));

          this.$http
            .get(
              `api/graphicsrv6/GetAvailiableGraphicsByKeyGraphicStateList?keyGraphicStateJSON=${keyGraphicStateJSON}`
            )
            .then(response => {
              this.availiableGraphics = response.data;
            })
            .catch(error => {});

          this.$http
            .get(
              `api/statistic/GetDirtyCellsByMeasurementRecording?measurementRecordingId=${
                this.selectedMeasurementId
              }`
            )
            .then(response => {
              var dirtyCells = response.data;
              this.dirtyCells = dirtyCells;
              this.loading = false;
              this.$http
                .get(
                  `api/dievalue/GetSelectedDiesByMeasurementRecordingId?measurementRecordingId=${
                    this.selectedMeasurementId
                  }`
                )
                .then(response => {
                  var diesList = response.data;
                  this.avbSelectedDies = diesList.slice();
                  this.$store.commit("wafermeas/updateSelectedDies", diesList);
                })
                .catch(error => {});
            })
            .catch(error => {});
        })
        .catch(error => {});
    },

    availiableGraphics: function() {
      if (this.availiableGraphics.length == 0) {
        this.selectedGraphics = [];
      }
    },

    selectedDies: function() {
      var statList = this.dirtyCells.statList;
      var fixedList = this.dirtyCells.fixedList;
      this.dirtyCells.statPercentage = Math.ceil(
        (1.0 -
          this.selectedDies.filter(value => statList.includes(value)).length /
            this.selectedDies.length) *
          100
      );
      this.dirtyCells.fixedPercentage = Math.ceil(
        (1.0 -
          this.selectedDies.filter(value => fixedList.includes(value)).length /
            this.selectedDies.length) *
          100
      );
    }
  }
};
</script>
