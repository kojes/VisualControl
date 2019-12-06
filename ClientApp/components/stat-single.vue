<template>
 
    <v-card>
      <v-container grid-list-lg>
        <v-layout align-start justify-space-between>
          <v-toolbar>
            <v-toolbar-title>{{graphicName}}</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-toolbar-items class="hidden-sm-and-down">
              <v-progress-circular
                :rotate="360"
                :size="60"
                :width="6"
                :value="mode === `stat` ? dirtyCellsStatPercentage : dirtyCellsFixedPercentage"
                :color="mode === `stat` ? 'primary' : 'indigo lighten-4'"
              >{{  mode === `stat` ? dirtyCellsStatPercentage + '%' : dirtyCellsFixedPercentage + '%' }}</v-progress-circular>
              <v-btn text icon :color="mode === `stat` ? 'primary' : 'indigo lighten-4'" @click="delDirtyCells(dirtyCells)">
                <v-icon>cached</v-icon>
              </v-btn>
              <v-switch
                color="primary"
                v-model="switchMode"
                :label="mode"
              ></v-switch>
               <!-- <v-btn
          color="indigo"
          dark
          @click="showPopoverClick"
        >
          Menu as Popover
        </v-btn>
        <v-menu
      v-model="showPopover"
      :close-on-content-click="false"
      
      
    >
     <v-card>
        <v-list>
          <v-list-item avatar>
            <v-list-item-avatar>
              <img src="https://cdn.vuetifyjs.com/images/john.jpg" alt="John">
            </v-list-item-avatar>

           
          </v-list-item>
        </v-list>

        
      </v-card>
    </v-menu> -->
            </v-toolbar-items>
          </v-toolbar>
        </v-layout>
        <v-layout align-start justify-space-between>
          <v-flex lg12>
          <v-tabs v-model="activeTab" color="indigo" dark slider-color="primary">
            <v-tab href="#commonTable">
              Сводная таблица
             
            </v-tab>

            <v-tab
              v-for="stat in statArray"
              :key="stat.statisticsName"
              :href="'#' + stat.statisticsName"
            >
            
             {{ stat.statisticsName }}
            
            </v-tab>

            <v-tab-item
        v-for="stat in statArray"
        :key="stat.statisticsName"
        :value="stat.statisticsName"
      >
        <v-card flat>
          <v-card-text>{{ stat.statisticsName }}</v-card-text>
        </v-card>
      </v-tab-item>

            <v-tab-item value="commonTable">
              <v-card flat>
                <v-card-text>
                  <v-layout>
                    <v-flex lg12>
                    <v-data-table
                      :headers="headers"
                      :items="statArray"
                      no-data-text = "Нет данных"
                      class="elevation-2 pa-0"
                      :loading="loading"
                      hide-actions
                      dark
                    >
                      <template v-slot:items="props">
                        <tr @click="showStatTab(props.item.statisticsName)">
                        <td class="text-sm-center pa-0" v-html="props.item.statisticsName"></td>
                        <td class="text-sm-center pa-0">{{ props.item.expectedValue }}</td>
                        <td class="text-xs-center pa-0">{{ props.item.standartDeviation }}</td>
                        <td class="text-xs-center pa-0">{{ props.item.minimum }}</td>
                        <td class="text-xs-center pa-0">{{ props.item.maximum }}</td>
                        <td class="text-xs-center pa-0">{{ props.item.median }}</td>

                        <td class="text-xs-center">
                          <v-progress-circular
                            :rotate="360"
                            :size="45"
                            :width="4"
                            :value = "mode === `stat` ? Math.ceil((1.0 - props.item.dirtyCells.statPercentage) * 100) : Math.ceil((1.0 - props.item.dirtyCells.fixedPercentage) * 100)"
                            :color= "mode === `stat` ? 'primary' : 'indigo lighten-4'"
                          >{{ mode === `stat` ? Math.ceil((1.0 - props.item.dirtyCells.statPercentage) * 100) + '%' : Math.ceil((1.0 - props.item.dirtyCells.fixedPercentage) * 100) + '%' }}</v-progress-circular>

                          <v-btn text icon :color="mode === `stat` ? 'primary' : 'indigo lighten-4'" @click="delDirtyCells(props.item.dirtyCells)">
                            <v-icon>cached</v-icon>
                          </v-btn>
                        </td>
                        </tr>
                      </template>
                    </v-data-table>
                    </v-flex>
                  </v-layout>
                </v-card-text>
              </v-card>
            </v-tab-item>
          </v-tabs>
          </v-flex>
        </v-layout>
      </v-container>
    </v-card>
  
</template>

<script>
export default {
  props: ["keyGraphicState", "measurementId", "divider"],

  data() {
    return {
      showPopover: false,
      PopoverX: 0,
      PopoverY: 0,
      switchMode: true,
      statArray: [],
      graphicName: "",
      activeTab : "commonTable",
      loading: false,
      headers: [
        {
          text: "Название",
          align: "center",
          sortable: false,
          value: "statisticsName"
        },
        {
          text: "Мат.ожидание",
          align: "center",
          sortable: false,
          value: "expectedValue"
        },
        {
          text: "Ст.отклонение",
          align: "center",
          sortable: false,
          value: "standartDeviation"
        },
        {
          text: "Минимум",
          align: "center",
          sortable: false,
          value: "minimum"
        },
        {
          text: "Максимум",
          align: "center",
          sortable: false,
          value: "maximum"
        },
        {
          text: "Медиана",
          align: "center",
          sortable: false,
          value: "median"
        },
        {
          text: "Годные по статистике",
          align: "center",
          sortable: false,
          value: "dirtyCells.statPercentage"
        }
      ]
    };
  },

  created()
  {
       this.$http
            .get(`api/graphicsrv6/GetGraphicNameByKeyGraphicState?=${this.keyGraphicState}`)
            .then(response => {
              this.graphicName = response.data;
              this.getStatArray();
            }).catch(error => {});
  },

  methods: {
    delDirtyCells: function(dirtyCells)
    {
       let deletedDies = []; 
       if(this.mode === "stat")
       {
           deletedDies = dirtyCells.statList;
       }
       else
       {
           deletedDies = dirtyCells.fixedList;
       }
       

       let selectedDies = this.selectedDies.filter((el) => !deletedDies.includes( el ) );
       this.$store.commit('wafermeas/updateSelectedDies', selectedDies);
    },

    showStatTab(statisticsName)
    {
        this.activeTab = statisticsName;
    },

    showPopoverClick(e)
    {
     
        e.preventDefault()
        this.showPopover = false
        this.PopoverX = e.clientX
        this.PopoverY = e.clientY
        this.$nextTick(() => {
          this.showPopover = true
        })
      
    },

    getStatArray: function()
    { 
          if (this.measurementId != 0 && this.selectedDies.length > 0 ) {
          this.loading = true;
          var singlestatModel = {};
          singlestatModel.divider = this.divider;
          singlestatModel.keyGraphicState = this.keyGraphicState;
          singlestatModel.measurementId = this.measurementId;
          singlestatModel.dieIdList = this.selectedDies;
          this.$http
            .get(
              `api/statistic/GetStatisticSingleGraphic?statisticSingleGraphicViewModelJSON=${JSON.stringify(
              singlestatModel
            )}`
            )
            .then(response => {
              var singleStat = response.data;
              this.loading = false;
              this.statArray = singleStat;
             
            }).catch(error => {});
            
      }
       
    }

  },

  watch:
  {
     
      

      divider: function()
      {
          this.getStatArray();
      },
      
      selectedDies: function()
      {
          this.getStatArray();
      }

          

  },

  computed: 
  {  


     mode()
     {
        if(this.switchMode)
        {
           return "stat";
        } 
        else
        {
            return "fixed";  
        }
     },
     
     selectedDies()
     {
         return this.$store.state.wafermeas.selectedDies
     },

     dirtyCells()
     {
         var statArray = [];
         var fixedArray = [];
         this.statArray.forEach(s => { statArray = statArray.concat(s.dirtyCells.statList), fixedArray = fixedArray.concat(s.dirtyCells.fixedList)});
         return { statList: [...new Set(statArray)], fixedList: [...new Set(fixedArray)]}
     },

     dirtyCellsStatPercentage()
     {
          var percentage =  Math.ceil((1.0 - this.dirtyCells.statList.length / this.selectedDies.length) * 100)
          if(isNaN(percentage))
          {
              return 0;
          }
          else
          {
             return percentage;
          }
     },

     dirtyCellsFixedPercentage()
     {    
          var percentage = Math.ceil((1.0 - this.dirtyCells.fixedList.length / this.selectedDies.length) * 100)
          if(isNaN(percentage))
          {
              return 0;
          }
          else
          {
             return percentage;
          }
     }
  }

 
  };
</script>

<style>
.card-shadow {
  --box-shadow-color: palegoldenrod;
  box-shadow: 1px 2px 3px var(--box-shadow-color);
}
</style>
