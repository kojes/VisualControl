<template>
  <v-container>
  <v-row class="pt-8">           
            <v-col lg="6">
                <v-tabs v-if="dieType" vertical background-color="indigo">
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
                                                <v-checkbox v-model= "selectedCodeProducts" :value="cp.id" color="primary" @change="codeProductsChange(cp.id)"></v-checkbox>
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
                            <create-element mode="update" @create-element="createElement"></create-element>
                            <v-list two-line rounded style="max-height: 350px" class="overflow-y-auto">
                                <v-subheader v-if="elements && elements.length > 0">Список элементов</v-subheader>
                                <v-list-item v-for="element in elements" :key="element.name">                             
                                    <v-list-item-content>
                                        <v-list-item-title>{{element.name}}</v-list-item-title>
                                        <v-list-item-subtitle>{{element.comment}}</v-list-item-subtitle>
                                    </v-list-item-content>                                    
                                    <v-list-item-action>                                       
                                        <update-element :edited-element="Object.assign({}, element)" :key="element.name" @update-element="updateElement"></update-element>     
                                    </v-list-item-action>
                                    <v-list-item-action>     
                                        <v-icon v-if="element.isAvaliableToDelete" color="pink" @click="deleteElement(element)">delete_outline</v-icon>
                                        <v-tooltip bottom v-else>
                                            <template v-slot:activator="{ on }">
                                                <v-icon v-on="on" color="grey">delete_forever</v-icon>
                                            </template>
                                            <span>Элемент невозможно удалить, так как он привязан к измерению</span>
                                        </v-tooltip>         
                                    </v-list-item-action>
                                </v-list-item>
                            </v-list>
                        </v-card>                        
                    </v-tab-item>
                </v-tabs>
                
            </v-col>
            <v-col lg="4" offset-lg="1">
                <v-select v-model="dieType"
                    :items="dieTypes"
                    no-data-text="Нет данных"
                    item-text="name"
                    item-value="id"
                    outlined
                    label="Выберите монитор для редактирования"
                ></v-select>                
            </v-col>
            <v-col lg="1">
                <v-btn fab small outlined color="primary">
                    <v-icon>create</v-icon>
                </v-btn>
            </v-col>
           
        </v-row>
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
import ElementUpdating from './element-update.vue'
export default {
    data() {
        return {
            dieType: "",
            selectedProcess: "",
            dieTypes: [],
            processes: [],
            avCodeProducts: [],
            selectedCodeProducts: [],
            snackbar: {text: "", color: "success", visible: false}    
        }
    },

    components: {
        "create-element": ElementCreation,
        "update-element": ElementUpdating
    },

    methods: {
        async getProcesses() {
            await this.$http
            .get(`/api/process/all`)
            .then(response => this.processes = response.data)
            .catch(err => console.log(err)); ///err
        },

        async getDieTypes() {
            await this.$http
            .get(`/api/dietype/all`)
            .then(response => this.dieTypes = response.data)
            .catch(err => console.log(err)); ///err
        },

        async updateDieType(dieTypeName, selectedCodeProducts, elements) {
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

        async codeProductsChange(idcp) {
            await this.$http({
                method: "post",
                url: `/api/dietype/codeproduct/update-fk`, 
                data: {codeProductId: idcp, dieTypeId: this.dieType}, 
                config: {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                }
            })
            .then(response => { 
                let dieTypeName = this.dieTypes.find(x => x.id === this.dieType).name
                if(response.status === 201) {
                    this.showSnackBar(`Шаблон ${response.data} успешно добавлен к монитору ${dieTypeName}`)
                }
                if(response.status === 200) {
                    this.showSnackBar(`Шаблон ${response.data} успешно отвязан от монитора ${dieTypeName}`)
                }               
            })
            .catch(error => this.showSnackBar(error.response.data[0].message, "error"));  
        },

        async deleteElement(element) {
            if(element.elementId === 0) {
                this.deleteElementFromStore(element.name);
            }
            else {
                await this.$http({
                    method: "delete",
                    url: `/api/element/${element.elementId}`               
                })
                .then(response => { 
                    if(response.status === 200) {
                        this.showSnackBar(`Элемент ${element.name} успешно удален`)
                    }    
                    this.deleteElementFromStore(element.name);
                })
                .catch(error => this.showSnackBar(error.response.data[0].message, "error"));  
            }    
        },

        async createElement(element) {
            let createdElement = {}
            await this.$http({
                method: "put",
                url: `/api/element`, 
                data: element, 
                config: {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                }
            })
            .then(async response => { 
               createdElement = response.data;
               await this.$http({
                    method: "put",
                    url: `/api/element/${createdElement.elementId}/dietype/${this.dieType}`, 
                    config: {
                        headers: {
                            'Accept': "application/json",
                            'Content-Type': "application/json"
                            }
                        }
                })
                .then(response => {
                    this.showSnackBar(`Элемент ${createdElement.name} успешно добавлен`)
                    this.$store.commit("elements/addtoElements", createdElement)          
                })                
            })
            .catch(error => this.showSnackBar(error.response.data[0].message, "error"))
           
        },

        async updateElement(element) {
            await this.$http({
                method: "post",
                url: `/api/element/update `, 
                data: element, 
                config: {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                }
            })
            .then(response => {                 
                this.showSnackBar(`Элемент ${response.data.name} успешно изменен`)
                this.$store.commit("elements/updateElement", response.data)                         
            })
            .catch(error => this.showSnackBar(error.response.data[0].message, "error"));
        },

        deleteElementFromStore(elementName)
        {
            this.$store.commit("elements/deleteFromElements", elementName)
        },

        showSnackBar(text, color)
        {
          this.snackbar.text = text
          this.snackbar.color = color
          this.snackbar.visible = true
        }         

    },

    watch: {        
        selectedProcess: async function(newVal, oldVal) {
            await this.$http
            .get(`/api/codeproduct/processid/${newVal}`)
            .then(response => this.avCodeProducts = response.data)
            .catch(err => console.log(err)) ///err
        },

        dieType: async function(newVal, oldVal) {
            await this.$http
            .get(`/api/dietype/cp-el/${newVal}`)
            .then(response => {
                this.selectedCodeProducts = response.data.codeProductIdsList
                this.$store.commit("elements/fillElements", response.data.elementsList)
            })
            .catch(err => console.log(err)) 
        }

    },
    
    computed: {
        elements() {
            return this.$store.state.elements.elements
        }     
    },

    async mounted() {
        await this.getProcesses().then(() => this.selectedProcess = 333)    
        await this.getDieTypes()           
    }
}
</script>

