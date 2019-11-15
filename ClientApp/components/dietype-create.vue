<template>
    <v-container>
        <v-layout row class="pt-8">
           
            <v-flex lg6>
                <v-tabs vertical background-color="indigo">
                    <v-tab key="cp">
                        Шаблоны
                    </v-tab>
                    <v-tab-item key="cp">
                        <v-card elevation="8">
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
                                                <v-checkbox v-model= "selectedCodeProducts" :value="cp.id" color="primary"></v-checkbox>
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
                        <v-card elevation="8">
                            <create-element></create-element>
                            <v-list two-line rounded style="max-height: 350px" class="overflow-y-auto">
                                <v-subheader v-if="elements && elements.length > 0">Список элементов</v-subheader>
                                <v-list-item v-for="element in elements" :key="element.name">                             
                                    <v-list-item-content>
                                        <v-list-item-title>{{element.name}}</v-list-item-title>
                                        <v-list-item-subtitle>{{element.comment}}</v-list-item-subtitle>
                                    </v-list-item-content>
                                    <v-list-item-action>
                                    <v-icon color="primary" @click="deleteElement(element)">delete_outline</v-icon>
                                    </v-list-item-action>
                                </v-list-item>
                            </v-list>
                        </v-card>                        
                    </v-tab-item>
                </v-tabs>
                
            </v-flex>
            <v-flex lg4 offset-lg1>
                 <v-text-field v-model="dieTypeName" label="Введите название монитора" outlined></v-text-field>
                 <v-checkbox :value="dieTypeName" color="success" on-icon="done_outline" off-icon="report" readonly class="mx-2" label="У монитора должно быть название"></v-checkbox>
                 <v-checkbox :value="dieTypeName && !isDuplicateDieTypeExist" color="success" on-icon="done_outline" off-icon="report" readonly class="mx-2" label="Название не должно совпадать с существующим"></v-checkbox>
                 <v-checkbox :value="selectedCodeProducts.length > 0" color="success" on-icon="done_outline" off-icon="report" readonly class="mx-2" label="Выберите шаблоны на которых расположен монитор"></v-checkbox>
                 <v-checkbox :value="elements.length > 0" color="success" readonly  on-icon="done_outline" off-icon="report" class="mx-2" label="Создайте элементы для монитора"></v-checkbox>
                 <v-btn v-if="readyToCreate" color="success" @click="createDieType(dieTypeName, selectedCodeProducts, elements)">Создать монитор</v-btn>
            </v-flex>
           
        </v-layout>
        <v-snackbar v-model="snackbar.visible"
                    :color="snackbar.color"
                    right
                    top>
            {{ snackbar.text }}
            <v-btn color="pink"
                text
                @click="snackbar.visible = false">
            Закрыть
            </v-btn>
        </v-snackbar>        
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
            selectedCodeProducts: [],
            snackbar: {text: "", color: "success", visible: false},
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
        
        async createDieType(dieTypeName, selectedCodeProducts, elements) {
            await this.$http({
                method: "put",
                url: `/api/dietype`, 
                data: {name: dieTypeName, codeProductIdsList: selectedCodeProducts, elementsList: elements}, 
                config: {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                }
            })
            .then(response => { 
                this.showSnackBar(`Монитор ${response.data} успешно добавлен`)
                this.dieTypeName = ""
                this.selectedCodeProducts = []
                this.$store.commit("elements/clearElements")
            
            })
            .catch(error => this.showSnackBar(error.response.data[0].message, "error"));  
        },            

        deleteElement(element) {
            this.$store.commit("elements/deleteFromElements", element.name)
        },

       

        showSnackBar(text, color)
        {
          this.snackbar.text = text
          this.snackbar.color = color
          this.snackbar.visible = true
        }
    },

    computed: {
        elements() {
            return this.$store.state.elements.elements
        },

        readyToCreate() {
            return this.dieTypeName && this.selectedCodeProducts.length > 0 && this.elements.length > 0 && !this.isDuplicateDieTypeExist
        }
    },

    asyncComputed: {
        isDuplicateDieTypeExist: {
            async get() {
              return await this.$http.get(`/api/dietype/all`).then(response => response.data.some(x => x.name === this.dieTypeName))  
            },
            default() {
                return true
            },
            watch: ['dieTypeName']
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
        await this.getProcesses().then(() => this.selectedProcess = 333)         
    }
}
</script>