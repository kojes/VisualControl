<template>
  <v-row :class="responsiveSettings.rowClass">
        <v-col :lg="responsiveSettings.vToolbarFlexSize">
          <v-toolbar extended>          
              <v-container>
                <v-row>    
                  <v-col lg="6" class="d-flex align-center justify-start" v-if="!loading">  
                    <v-chip class="elevation-8" label x-large color="#303030">
                      {{graphicName}}
                    </v-chip>
                  </v-col>  
                  <v-col lg="6" class="d-flex align-center justify-space-around" v-if="!loading">
                  <div class="d-flex flex-column" >
                    <v-chip class="elevation-8" color="#303030">
                      Годны по всей пластине
                    </v-chip>
                    <div class="d-flex flex-row mt-4">   
                      <v-progress-circular
                        :rotate="360"
                        :size="60"
                        :width="2"
                        :value="viewMode==='Мониторинг' ? dirtyCellsFullWafer.stat.percentage : dirtyCellsFullWafer.fixed.percentage"
                        :color="viewMode==='Мониторинг' ? this.$store.getters['wafermeas/calculateColor'](dirtyCellsFullWafer.stat.percentage / 100) : this.$store.getters['wafermeas/calculateColor'](dirtyCellsFullWafer.fixed.percentage / 100)">
                      {{ viewMode==='Мониторинг' ? dirtyCellsFullWafer.stat.percentage + '%' : dirtyCellsFullWafer.fixed.percentage + '%'}}
                      </v-progress-circular>
                    </div>   
                  </div>
                <div class="d-flex flex-column align-self-center">
                  <v-chip class="elevation-8" color="#303030">
                    Годны из выбранных
                  </v-chip>
                  <div class="d-flex flex-row mt-4">                
                    <v-progress-circular
                      :rotate="360"
                      :size="60"
                      :width="2"
                      :value="viewMode === 'Мониторинг' ? dirtyCellsStatPercentage : dirtyCellsFixedPercentage"
                      :color="viewMode === 'Мониторинг' ? 'primary' : '#80DEEA'">
                      {{ viewMode === 'Мониторинг' ? dirtyCellsStatPercentage + '%' : dirtyCellsFixedPercentage + '%' }}
                    </v-progress-circular>
                    <v-btn text icon :color="viewMode === 'Мониторинг' ? 'primary' : '#80DEEA'" @click="delDirtyCells(dirtyCells)">
                      <v-icon>cached</v-icon>
                    </v-btn>    
                </div>   
                </div>                            
                <!-- <v-switch color="primary" v-model="switchMode" :label="mode"></v-switch> -->
                </v-col> 
                <v-col v-else>
                  <v-skeleton-loader
                    class="mx-auto"
                    type="date-picker-options">
                  </v-skeleton-loader>
                </v-col>
                </v-row>
              </v-container>
          </v-toolbar>
        </v-col>
        <v-col :lg="responsiveSettings.waferMiniFlexSize">
          <wafer-mini v-if="(viewMode === 'Мониторинг' && dirtyCellsFullWafer.stat.percentage >= 0) || (viewMode === 'Поставка' && dirtyCellsFullWafer.fixed.percentage >= 0)"
            :keyGraphicState="keyGraphicState"
            :rowViewMode="rowViewMode"
            :viewMode="viewMode"
            :key="`wfm-${keyGraphicState}`">
          </wafer-mini>
        </v-col>
    </v-row>
</template>

<script>
import WaferMap from "./wafer-mini.vue";
export default {
    props: ["loading", "graphicName", "dirtyCellsFullWafer", "dirtyCellsStatPercentage", "dirtyCellsFixedPercentage", "dirtyCells", "viewMode", "rowViewMode", "keyGraphicState"],
    data() {
        return {
            
        }
    },
    components: {
        "wafer-mini": WaferMap,
    },

    computed: {
        responsiveSettings() {
            if(this.rowViewMode === 'bigChart') {
                return {
                    rowClass: "d-flex flex-column",
                    vToolbarFlexSize: 12,
                    waferMiniFlexSize: 12
                }
            }
            else {
                return {
                    rowClass: "",
                    vToolbarFlexSize: 8,
                    waferMiniFlexSize: 4
                }
            }
        }
    }
}
</script>

<style>

</style>