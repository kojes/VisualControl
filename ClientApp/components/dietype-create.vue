<template>
    <v-container>
        <v-layout row>
           
            <v-flex lg6>
                <v-tabs vertical background-color="indigo">
                    <v-tab key="cp">
                        Шаблоны
                    </v-tab>
                    <v-tab-item key="cp">
                        <v-card>
                            <v-card-title>
                                <v-select v-model="selectedProcess"
                                    :items="processes"
                                    no-data-text="Нет данных"
                                    item-text="processName"
                                    item-value="processId"
                                    label="Выберите техпроцесс"
                                ></v-select>
                            </v-card-title>
                            <v-card-text>
                                <v-list 
                                subheader
                                two-line
                                >
                                   
                                    <div style="max-height: 300px" class="overflow-y-auto">
                                        <v-list-item v-for="cp in avCodeProducts" :key="cp.id" >
                                            <v-list-item-action>
                                                <v-checkbox @change="selectCodeProduct(cp)"></v-checkbox>
                                            </v-list-item-action>
                                            <v-list-item-content>
                                                <v-list-item-title>{{cp.name}}</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>
                                    </div>                        
                                </v-list>
                            </v-card-text>
                        </v-card>
                    </v-tab-item>
                    <v-tab key="elements">
                        Элементы
                    </v-tab>
                    <v-tab-item key="elements">   
                        <create-element></create-element>
                        <div style="max-height: 300px" class="overflow-y-auto">
                            <v-list-item v-for="element in elements" :key="element.name" >                             
                                <v-list-item-content>
                                    <v-list-item-title>{{element.name}}</v-list-item-title>
                                </v-list-item-content>
                                <v-list-item-action>
                                  <v-icon color="primary" @change="deleteElement(element)" >delete_outline</v-icon>
                                </v-list-item-action>
                            </v-list-item>
                        </div>                        
                    </v-tab-item>
                </v-tabs>
                
            </v-flex>
             <v-flex lg3 offset-lg1>
                 <v-text-field v-model="dieTypeName" :error-messages="dieTypeName ? [] : 'Введите название монитора'" 
                                    label="Введите название монитора" outlined
                 ></v-text-field>
            </v-flex>
        </v-layout>        
    </v-container>
</template>
<script>
import ElementCreation from './create-element.vue'
export default {    
    data() {
        return {
            dieTypeName: "",
            selectedProcess: "",
            processes: [],
            avCodeProducts: [],
            selectedCodeProducts: []

        }
    },

    components: {
        "create-element": ElementCreation
    },

    methods: {
        async getProcesses() {
            await this.$http
            .get(`/api/process/all`)
            .then(response =>  this.processes = response.data)
            .catch(err => console.log(err)); ///err
        },

        async getCodeProductsByDieTypeId() {

        },

        deleteElement(element) {
            this.$store.commit("elements/deleteFromElements", element.name)
        },

        selectCodeProduct(codeproductId) {

            this.selectedCodeProducts.includes(codeproductId) 
                ? this.selectedCodeProducts = this.selectedCodeProducts.filter(x => x!=codeproductId) 
                : this.selectedCodeProducts.push(codeproductId)
           
        }
    },

    computed: {
        elements() {
            return  this.$store.getters['elements/getElements']
        }
    },

    watch: {
        selectedProcess: async function(newVal, oldVal) {
            await this.$http
            .get(`/api/codeproduct/processid/${newVal}`)
            .then(response => this.avCodeProducts = response.data)
            .catch(err => console.log(err)) ///err
        }
    },

    async mounted() {
        this.getProcesses()
    }
}
</script>