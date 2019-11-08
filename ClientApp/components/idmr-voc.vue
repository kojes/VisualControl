<template>
    <v-container grid-list-lg>
        <v-layout row>
            <v-flex lg3 offset-lg1>
                <v-autocomplete v-model="waferId"
                                    :items="wafers"
                                    no-data-text="Нет данных"
                                    item-text="waferId"
                                    item-value="waferId"
                                    box
                                    outline
                                    label="Номер пластины">
                </v-autocomplete>
            </v-flex>
            <v-flex lg3>
                <v-text-field v-model="dieType.name" readonly 
                                    label="Тип ячейки">
                </v-text-field>
            </v-flex>
        </v-layout>
       
        <v-layout row>
            <v-flex lg10 offset-lg1>
               <v-stepper v-if="stagesArray.length>0" v-model="e1" vertical>               
                    <template v-for="(stage, index) in stagesArray">
                    <v-stepper-step 
                        :key="`${index+1}-step`"
                        :step="index + 1"
                        color="indigo"
                        complete
                        editable>
                        {{stage.name}}
                    </v-stepper-step>
                    <v-stepper-content
                        :key="`${index+1}-content`"
                        :step="index + 1">
                       
                        <v-card>
                            <v-card-text>                              
                           
                             <v-layout v-for="idmr in stage.measurementRecordingList" :key="idmr.id">
                                <v-flex lg3>
                                    <v-text-field v-model="idmr.name" readonly outline label="Номер операции">
                                    </v-text-field>
                                </v-flex>
                                <v-flex lg7>
                                    <v-select :value="stage.id"
                                              :items="allStagesArray"
                                              no-data-text="Нет данных"
                                              item-value="stageId" 
                                              item-text="stageName"
                                              outline
                                              label="Выберите этап:"
                                              @change="updateStageOnIdmr(idmr.id, stage.id, $event)">
                                    </v-select>                                        
                                </v-flex>
                                 <v-flex lg2>
                                    <v-select v-model="idmr.element"
                                              :items="avElements"
                                              no-data-text="Нет данных"
                                              item-value="elementId" 
                                              item-text="name"
                                              outline
                                              label="Выберите элемент:"
                                              @change="updateElementOnIdmr(idmr.id, idmr.element)">
                                    </v-select>                                     
                                </v-flex>
                            </v-layout>
                          </v-card-text>
                        </v-card>                                        
                    </v-stepper-content>
                    </template>                          
            </v-stepper>
           </v-flex>
        </v-layout>
        <v-snackbar v-model="snackbar.visible" top>
        {{ snackbar.text }}
        <v-btn color="pink" flat @click="snackbar.visible = false">Закрыть</v-btn>
        </v-snackbar>
        <v-dialog
            v-model="loading"
            hide-overlay
            persistent
            width="300">
        <v-card color="indigo" dark>
            <v-card-text>
            Формирование этапов измерений
            <v-progress-linear
                indeterminate
                color="white"
                class="mb-0"
            ></v-progress-linear>
            </v-card-text>
        </v-card>
    </v-dialog>
    </v-container>    
</template>

<script>
export default {
    data() {
        return {
           snackbar: {visible: false, text: ""},
           e1: 0,
           waferId: "",
           wafers: [],
           codeProduct: "",
           dieType: { id: 1, name: "Монитор 1" },
           allStagesArray: [],
           avElements: [],
           stagesArray: [],
           loading: false
        }
    },

    methods: 
    {
        setStage(idmr) {

        },

        showSnackbar(text) {
            this.snackbar.visible = true
            this.snackbar.text = text
        },

        async initialize() {

            await this.getWafers()
            
         
        },

        async getStagesByWaferId(waferId) {
                await   this.$http
                        .get(`/api/measurementrecording/wafer/${waferId}/stage`)
                        .then(response => {
                            this.stagesArray = response.data
                        })
                        .catch((error) => {
                            alert(error)
                        });
        },


        async getAvElements(dieType) {
             
            await   this.$http
                        .get(`/api/element/dietype/${dieType.id}`)
                        .then(response => {
                            this.avElements = response.data
                        })
                        .catch((error) => {
                            alert(error)
                        });
        },

        async getAllStages(waferId) {
            await   this.$http
                        .get(`/api/stage/getstagesbywaferid?waferId=${waferId}`)
                        .then(response => {
                            this.allStagesArray = response.data
                        })
                        .catch((error) => {
                            alert(error)
                        });
        },

        async getWafers() {
            await this.$http.get(`/api/wafer/getall`).then(response => {
                 this.wafers = response.data;
            });
        },

        async updateStageOnIdmr(idmr, oldStageId, newStageId) {
            let measurementRecording = this.stagesArray.find(x => x.id === oldStageId).measurementRecordingList.find(x => x.id === idmr)
            let newStage = this.stagesArray.find(x => x.id === newStageId)
            if(!newStage) {
                let stage = this.allStagesArray.find(x => x.stageId === newStageId);
                this.stagesArray.push({id: stage.stageId, name: stage.stageName, measurementRecordingList: Array(1).fill(measurementRecording)})
                this.e1 = this.stagesArray.length
            } else {
                newStage.measurementRecordingList.push(measurementRecording)
                this.e1 = this.stagesArray.findIndex(x => x.id === newStageId) + 1
            }
            
            let newOldStage = this.stagesArray.find(x => x.id === oldStageId).measurementRecordingList.filter(x => x.id != idmr)
            if(newOldStage.length === 0) {
                this.stagesArray = this.stagesArray.filter(x => x.id !== oldStageId)
            } else {
                this.stagesArray.find(x => x.id === oldStageId).measurementRecordingList = newOldStage
            } 
          
           
            const response = await this.$http({
                method: "post",
                url: `/api/measurementrecording/update-stage`, 
                data: { stageId: newStageId, measurementRecordingId: idmr}, 
                config: {
                    headers: {
                                'Accept': "application/json",
                                'Content-Type': "application/json"
                             }
                }
            })
            .then(response => {
                this.showSnackbar(`Этап успешно изменен`)
            })
            .catch(error => {
                this.showSnackbar("Произошла ошибка при изменении этапа")
            });
        },

        async updateElementOnIdmr(idmr, elementId) {
            const response = await this.$http({
                method: "post",
                url: `/api/element/updateElementOnIdmr`, 
                data: { elementId: elementId, measurementRecordingId: idmr}, 
                config: {
                    headers: {
                                'Accept': "application/json",
                                'Content-Type': "application/json"
                             }
                }
            })
            .then(response => {
                this.showSnackbar(`Элемент успешно изменен на ${response.data.name}`)
            })
            .catch(error => {
                this.showSnackbar("Произошла ошибка при изменении элемента")
            });
        }
    },

    watch: {
        waferId: async function(newVal, oldVal) {
            this.loading = true
            await this.getAllStages(newVal)
            await this.getAvElements(this.dieType)
            await this.getStagesByWaferId(newVal).then(() => this.loading = false)
        },
    },

    async mounted()
    {
        this.initialize()
        
    }
}
</script>