<template>
  <v-app>
    <div class="container w-100">
      <div class="row">
        <div class="col-6">
          <v-autocomplete v-model="selectedWafer"
                          :items="wafers"
                          box
                          :label="`Выберите пластину для просмотра дефектов`">

          </v-autocomplete>
        </div>
        <div class="col-3">
         
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


        </div>
        <div class="col-3">
          <v-btn :disabled="selectedWafer == null" block=true  @click="selectAllDies">{{selectAllDiesButtonText}}</v-btn>
          </div>
        </div>
      <div class="row">
        <loading :active.sync="filterLoading"
                 :can-cancel="false"
                 color="#4189C7"
                 loader="bars"
                 :is-full-page="false">

        </loading>

        <div class="col-3">
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
        </div>
        <div class="col-3">
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
        </div>
        <div class="col-3">
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
        </div>
        <div class="col-3">
          <v-btn block=true :disabled="defects.length === 0" @click="showDefects">Показать дефекты</v-btn>
        </div>
      </div>
      </div>

      <!--<defect-card :defectId="2"></defect-card>
      <defect-card :defectId="3"></defect-card>-->
    <div id="defectcards">
      <v-container grid-list-xl text-xs-center>
        <v-layout row wrap>
           <v-flex xs12 sm9 md9 lg10 offset-xs0 offset-lg1>
            <div v-for="defect in selectedDefects">
              <defect-card :defectId="defect.defectId"></defect-card>
            </div>
          </v-flex>
        </v-layout>
      </v-container>
     
    </div>
    </v-app>

    
</template>

<script>
  import DefectCard from './defect-card.vue'
  import Loading from 'vue-loading-overlay';
  import 'vue-loading-overlay/dist/vue-loading.css';

  export default {
    props: ['selectedWafer'],
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

     

      allDiesIsSelected()
      {
      
          return (this.defectFilter.avbDiesList) && (this.defectFilter.avbDiesList.length === this.selectedDies.length);
                
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
            this.$router.push({ name: 'defectsbywafer', params: { selectedWafer: val } })
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

                    this.$store.commit('setDefectFilter', response.data.body);
                  }
                  else {
                    this.defectFilter = [];
                    this.$router.push({ name: 'defects', params: { selectedWafer: val } })
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
