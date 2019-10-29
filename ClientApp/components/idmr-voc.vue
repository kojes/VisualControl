<template>
    <v-container grid-list-lg>
        <v-layout row>
            <v-flex lg3>
                <v-text-field v-model="waferId" readonly 
                                    label="Номер пластины">
                </v-text-field>
            </v-flex>
            <v-flex lg3>
                <v-text-field v-model="dieType.name" readonly 
                                    label="Тип ячейки">
                </v-text-field>
            </v-flex>
        </v-layout>
        <v-layout row>
            <v-card>
                <v-list>
                    <v-list-tile v-for="idmr in unfilledIdmrArray" :key="idmr.idmr">
                        <v-list-tile-content>
                            <v-layout>
                                <v-flex lg3 offset-lg1>
                                    <v-text-field v-model="idmr.name" readonly label="Номер операции">
                                    </v-text-field>
                                </v-flex>
                                <v-flex lg3 offset-lg1>
                                    <v-select v-model="idmr.stage"
                                              :items="stagesArray"
                                              no-data-text="Нет данных"
                                              item-value="id"
                                              item-text="name"
                                              outline
                                              label="Выберите этап:">
                                    </v-select>    
                                </v-flex>
                            </v-layout>
                        </v-list-tile-content>
                    </v-list-tile> 
                </v-list>                 
            </v-card>
        </v-layout>
        <v-layout row>
            <v-flex lg11 offset-lg1>
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
                              
                            </v-card-text>
                        </v-card>                                        
                    </v-stepper-content>
                    </template>                          
            </v-stepper>
           </v-flex>
        </v-layout>
    </v-container>    
</template>

<script>
export default {
    data() {
        return {
           e1: 0,
           waferId: "E907",
           dieType: { id: 1, name: "Монитор 1" },
           unfilledIdmrArray: [],
           stagesArray: []
        }
    },

    methods: 
    {
        setStage(idmr) {

        },

        async initialize() {
            await this.getUnfilledIdmrs()
            await this.getStages()
        },

        async getUnfilledIdmrs() {

        },

        async getStages() {
            
        }
    },

    async mounted()
    {
        this.initialize()
    }
}
</script>