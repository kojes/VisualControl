<template>
  <v-container grid-list-lg>
    
    <v-layout align-start justify-center row>
      <v-flex lg6 >
        <v-autocomplete v-model="selectedWafer"
                        :items="wafers"
                        box
                        :label="`Выберите пластину для просмотра дефектов`">

        </v-autocomplete>
      </v-flex>
      <v-flex lg3>

        <v-select v-model="selectedDies"
                  :items="defectFilter.avbDiesList"
                  chips
                  item-text="code"
                  no-data-text="Нет данных"
                  item-value="dieId"
                  item-disabled="disabled"
                  label="Выберите кристалл"
                  multiple>
        
          <template slot="selection"
                    slot-scope="{ item, index }">
            <v-chip v-if="index === 0">
              <span>{{ item.code }}</span>
            </v-chip>
            <span v-if="index === 1"
                  class="grey--text caption">(+{{ selectedDies.length - 1 }} выбрано)</span>
          </template>
        </v-select>

      </v-flex>

      <v-flex lg3>
        <v-btn :disabled="selectedWafer == null" block @click="selectAllDies">{{selectAllDiesButtonText}}</v-btn>
      </v-flex>
      </v-layout>

    <v-layout align-start justify-center row>
      <loading :active.sync="filterLoading"
               :can-cancel="false"
               color="#4b0082"
               loader="bars"
               :is-full-page="false">

      </loading>

      <v-flex lg3>
        <v-select v-model="selectedStages"
                  :items="defectFilter.avbStagesList"
                  chips
                  item-text="stageName"
                  no-data-text="Нет данных"
                  item-value="stageId"
                  item-disabled="disabled"
                  label="Выберите тех.этап"
                  multiple>
          <template slot="selection"
                    slot-scope="{ item, index }">
            <v-chip v-if="index === 0">
              <span>{{ item.stageName }}</span>
            </v-chip>
            <span v-if="index === 1"
                  class="grey--text caption">(+{{ selectedStages.length - 1 }} выбрано)</span>
          </template>
        </v-select>
      </v-flex>
      <v-flex lg3>
        <v-select v-model="selectedDefectType"
                  :items="defectFilter.avbDefectTypesList"
                  chips
                  no-data-text="Нет данных"
                  item-text="description"
                  item-value="defectTypeId"
                  item-disabled="disabled"
                  label="Выберите тип дефекта"
                  multiple>
          <template slot="selection"
                    slot-scope="{ item, index }">
            <v-chip v-if="index === 0">
              <span>{{ item.description }}</span>
            </v-chip>
            <span v-if="index === 1"
                  class="grey--text caption">(+{{ selectedDefectType.length - 1 }} выбрано)</span>
          </template>
        </v-select>
      </v-flex>
      <v-flex lg3>
        <v-select v-model="selectedDangerLevel"
                  :items="defectFilter.avbDangerLevelList"
                  chips
                  no-data-text="Нет данных"
                  item-text="specification"
                  item-value="dangerLevelId"
                  item-disabled="disabled"
                  label="Выберите опасность дефекта"
                  multiple>

        </v-select>
      </v-flex>
      <v-flex lg3>
        <v-btn block :disabled="defects.length === 0" @click="showDefects">Показать дефекты</v-btn>
        <v-btn block :disabled="defects.length === 0" @click="showAllAvailableDefects">Показать все доступные дефекты</v-btn>
      </v-flex>
      </v-layout>



     
        <v-container grid-list-lg>
          <v-layout row wrap>
            <v-flex lg3 v-for="defect in defectsOnCurrentPage" :key="defect.defectId">

              <defect-card :defectId="defect.defectId" :dieCode="defectFilter.avbDiesList.find(x=>x.dieId===defect.dieId).code"></defect-card>
              <v-divider light></v-divider>

            </v-flex>
            </v-layout>
            <v-layout row>
            <v-pagination  v-if="Math.ceil(selectedDefects.length/4) > 1" dark v-model="pageIndex"
                          :length="Math.ceil(selectedDefects.length/4)"></v-pagination>
            </v-layout>
</v-container>

      
</v-container>


</template>

<script>
  import DefectCard from './defect-card.vue'
  import Loading from 'vue-loading-overlay';
  import 'vue-loading-overlay/dist/vue-loading.css';

  export default {
    props: ['selectedWafer', 'selectedsingledieId', 'dangerlevelspec'],
    components: {
      'defect-card': DefectCard, Loading
    },
   
    created()
    {
    
      this.$http.get(`/api/wafer/getallwithdefects`).then((response) => {
        this.wafers = response.data;
      });

    
    },

    computed:
    {

      availableDangerLevels()
      {
        switch (this.dangerlevelspec) {
          case "all":
            return [0,1,2]
            break;
          case "bad":
            return [2]
            break;
          case "notgood":
            return [1,2]
            break;
          case "good":
            return [0]
            break;
          default:
            return [0,1,2]
        }
      },

      allDiesIsSelected()
      {
      
          return (this.defectFilter.avbDiesList) && (this.defectFilter.avbDiesList.length === this.selectedDies.length);
                
      },

      someDiesIsSelected() {

        return (this.defectFilter.avbDiesList) && (this.defectFilter.avbDiesList.length !== this.selectedDies.length) && (this.selectedDies.length > 0);

      },

    

      selectAllDiesButtonText()
      {
        if(!this.allDiesIsSelected)
        {
          return "Выбрать все кристаллы";
        }
        else
        {
          return "Снять выбор с кристаллов";
        }
      },

      defectsOnCurrentPage()
      {
         return this.selectedDefects.slice((this.pageIndex - 1) * 4, (this.pageIndex - 1) * 4 + 4)
      }
           

    },

    watch:
    {

      '$route'(to, from) {
        
      },

      selectedWafer:
      {
        handler: function (val, oldVal) {
          if (val != null) {


            let selectedWafer = this.selectedWafer;
         
                    
              this.selectedDies = [];
              this.selectedDefects = [];
              this.$http.get(`/api/defect/getbywaferid?waferId=${selectedWafer}`).then((response) => {
              this.defects = response.data;
              let defects = this.defects;
              this.filterLoading = true;
              this.$http.get('/api/defectfilter/GetDefectFilter', {
                params: {
                  defects: JSON.stringify(this.defects)
                }
              })
                .then((response) => {
                  if (response.data.errorCode === "OK") {
                    this.defectFilter = response.data.body;
                   
                    let currentdie = this.defectFilter.avbDiesList.find(x => x.dieId == this.selectedsingledieId);
                    if (this.selectedsingledieId && currentdie)
                    {
                                          
                        this.selectedDies.push(currentdie.dieId);
                                         
                    }
                    else
                    {
                       this.$router.push({ name: 'defectsbywafer', params: { selectedWafer: val } });
                    }
                  }
                  else {
                    this.defectFilter = [];
                    this.$router.push({ name: 'defects' })
                    this.$swal({
                      type: response.data.responseType,
                      text: 'Дефектов на пластине не найдено',
                      toast: true,
                      showConfirmButton: false,
                      position: 'top-end',
                      timer: 4000
                    });
                  }
                  this.filterLoading = false;
                });
            });
           
          }


        }, immediate: true
      },

      selectedDies: function () {
        
        this.defectFilter.avbStagesList.forEach(x => x["disabled"] = false);
        this.defectFilter.avbDefectTypesList.forEach(x => x["disabled"] = false);
        this.defectFilter.avbDangerLevelList.forEach(x => x["disabled"] = false);

        if (this.selectedDies.length != this.defectFilter.avbDiesList.length) {
          let avbDefects = this.defects.filter(d => this.selectedDies.some(t => t == d.dieId));

          let avbStages = [...new Set(avbDefects.map(d => d = d.stageId))];
          this.defectFilter.avbStagesList.forEach(x => {
            if (!avbStages.some(a => a == x.stageId)) {
              x.disabled = true;
              this.selectedStages = this.selectedStages.filter(c => c != x.stageId);
            }
          });


          let avbTypes = [...new Set(avbDefects.map(d => d = d.defectTypeId))];
          this.defectFilter.avbDefectTypesList.forEach(x => {
            if (!avbTypes.some(a => a == x.defectTypeId)) {
              x.disabled = true;
              this.selectedDefectType = this.selectedDefectType.filter(c => c != x.defectTypeId);
            }
          });


          this.defectFilter.avbDangerLevelList.forEach(x => {
            if (![...new Set(avbDefects.map(d => d = d.dangerLevelId))].some(a => a == x.dangerLevelId)) {
              x.disabled = true;
              this.selectedDangerLevel = this.selectedDangerLevel.filter(c => c != x.dangerLevelId);
            }
          });
          if (this.selectedsingledieId)
          {
            this.showAllAvailableDefects();
            this.$router.push({ name: 'defectsbywafer', params: { selectedWafer: this.selectedWafer } });
          }

        }




      }



    },

    methods:
    {
      selectAllDies: function ()
      {
        if (!this.allDiesIsSelected)
        {
          this.selectedDies = this.defectFilter.avbDiesList.map(d => { return d.dieId });
        }
        else
        {
          this.selectedDies = [];
        }
          
      },

      showAllAvailableDefects: function ()
      {
      
        this.selectedStages = this.defectFilter.avbStagesList.filter(x => !x.disabled).map(c => c.stageId);
        this.selectedDangerLevel = this.defectFilter.avbDangerLevelList.filter(x => !x.disabled && this.availableDangerLevels.some(t => t == x.danger)).map(c => c.dangerLevelId);
        this.selectedDefectType = this.defectFilter.avbDefectTypesList.filter(x => !x.disabled).map(c => c.defectTypeId);
        this.showDefects();
        

      },

   
      showDefects: function ()
      {
        
        this.selectedDefects = this.defects.filter(x => this.selectedStages.some(s => s == x.stageId) && this.selectedDies.some(d => d == x.dieId) && this.selectedDefectType.some(t => t == x.defectTypeId) && this.selectedDangerLevel.some(t => t == x.dangerLevelId));
        if (this.selectedDefects.length == 0)
        { 
          this.$swal({
            type: "warning",
            text: 'Дефектов не найдено',
            toast: true,
            showConfirmButton: false,
            position: 'top-end',
            timer: 4000
          });
        }
        
      }

    },

   

    data() {
      return {
        pageIndex: 1,
        filterLoading: false,
        selectedDies: [],
        selectedDefects: [],
        selectedStages: [],
        selectedDefectType: [],
        selectedDangerLevel: [],
        wafers: [],
        defects: [],
        defectFilter: {},
        dies: []

      }
    }
  }
</script>
