<template>
  <v-container>
      <v-row>
          <v-col lg="2">
            <v-select   v-model="selectedMonitor"
                        :items="monitors"
                        item-text="name"
                        item-value="id"
                        outlined
                        no-data-text="Нет данных"
                        label="Выберите монитор:">
            </v-select>
          </v-col>
          <v-col lg="10">
            <v-simple-table dark>
                <template v-slot:default>
                    <thead>
                        <tr>
                            <th class="text-left">Операция</th>
                            <th class="text-left">Элемент</th>
                            <th class="text-left">Имя файла</th>
                            <th class="text-left">Тип измерения</th>
                            <th class="text-center">Тип карты</th>
                            <th class="text-center">Комментарий</th>
                            <th class="text-center">Статус загрузки</th>
                            <th class="text-left"></th>
                            
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="operation in simpleOperations" :key="operation.guid">
                            <td><v-chip label color="grey darken-2"> {{operation.name + '_' + operation.element.name}}</v-chip></td>
                            <td><v-row>
                                    <v-col lg="9" class="text-lg-center">                                        
                                        <v-tooltip v-if="operation.element.elementId" bottom>
                                            <template v-slot:activator="{ on }">
                                                <v-chip v-on="on" label color="indigo">
                                                    {{operation.element.name}}
                                                </v-chip>
                                            </template>
                                            <span v-if="operation.element.comment">{{operation.element.comment}}</span>
                                            <span v-else>Нет описания</span>
                                        </v-tooltip>   
                                        <v-tooltip v-else bottom>
                                            <template v-slot:activator="{ on }">
                                                <v-chip v-on="on" label color="pink">
                                                    {{operation.element.name}}
                                                </v-chip>
                                            </template>
                                            <span>Элемента не существует на этом мониторе. Нажмите плюс для создания</span>
                                        </v-tooltip>          
                                    </v-col>
                                     <v-col lg="3" class="text-lg-left">
                                        <create-element v-if="!operation.element.elementId" @show-snackbar="showSnackBar" @element-created="elementCreated" :name="operation.element.name" :dieTypeId="selectedMonitor"></create-element>                            
                                    </v-col>
                                </v-row>
                            </td>
                            <td>{{operation.fileName.name}}</td>
                            <td>
                                <v-btn v-if="operation.fileName.graphicNames" icon color="success"><v-icon>done_outline</v-icon></v-btn>
                                <v-btn v-else icon color="pink"><v-icon>warning</v-icon></v-btn>
                            </td>
                            <td><v-text-field v-model="operation.mapType" outlined label="Тип карты:"></v-text-field></td>
                            <td><v-text-field v-model="operation.comment" outlined label="Комментарий:"></v-text-field></td>                           
                            <td><v-progress-circular :rotate="360" :size="50" :width="7" :value="100" color="success">100</v-progress-circular></td>
                            <td><v-btn icon color="pink" @click="deleteRow(operation.guid)"><v-icon>delete_outline</v-icon></v-btn></td>
                        </tr>
                    </tbody>
                </template>
            </v-simple-table>
          </v-col>
      </v-row>
      <v-snackbar v-model="snackbar.visible"
                    :color="snackbar.color"
                    right
                    top>
            {{ snackbar.text }}
            <v-btn color="white"
                text
                @click="snackbar.visible = false">
            Закрыть
            </v-btn>
        </v-snackbar>  
  </v-container>
</template>

<script>
import ElementCreate from './create-elementuploader.vue'
export default {

    props: ["codeProduct", "wafer", "measurementRecordings"],

    data() {
        return {
            selectedMonitor: "",
            simpleOperations: [],
            monitors: [],
            snackbar: {text: "", color: "indigo", visible: false}
        }
    },

    components: {
        "create-element": ElementCreate
    },

    watch: {
        selectedMonitor: async function(newVal, oldVal) {
            this.$http.get(`/api/folder/simpleoperation/${this.codeProduct}/${this.wafer}/${newVal}`, 
            {
                params: {
                  measurementRecordingsJSON: JSON.stringify(this.measurementRecordings)
                }
            })
            .then(response => {
                this.simpleOperations = response.data
            })
            .catch(error => {

            })              
        }
    },

    methods: {
        async initMonitors() {
            await this.$http
            .get(`/api/dietype/wafer/${this.wafer}`)
            .then(response => { 
                this.monitors = response.data
                this.selectedMonitor = this.monitors[0].id 
            })
            .catch(err => console.log(err)) ///err
        },

        deleteRow(guid) {
            this.simpleOperations = this.simpleOperations.filter(so => so.guid !== guid)
        },

        elementCreated(element) {
            this.simpleOperations.filter(so => so.element.name === element.name).map(so => {
                so.element.elementId = element.elementId
            })
        },

        showSnackBar(text, color)
        {
          this.snackbar.text = text
          this.snackbar.color = color
          this.snackbar.visible = true
        }
    },

    async mounted() {
        await this.initMonitors()
    }
}
</script>

<style>

</style>