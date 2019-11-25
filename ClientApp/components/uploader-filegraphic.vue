<template>
  <v-container>
      <v-row>
        <v-col lg="2">
            <v-select   v-model="selectedProcessId"
                        :items="processes"
                        item-text="processName"
                        item-value="processId"
                        no-data-text="Нет данных"
                        outlined
                        label="Выберите процесс:">
            </v-select>
        </v-col>
        <v-col lg="3">
            <v-card v-show="selectedProcessId" class="mx-auto" tile>
                <v-list rounded>
                    <v-subheader>Названия файлов</v-subheader>
                    <v-list-item-group v-model="selectedFileName" color="primary">
                        <v-list-item value="create">
                            <v-list-item-content>
                                <v-list-item-title>Добавить новый</v-list-item-title>                           
                            </v-list-item-content>
                        </v-list-item>
                        <v-list-item v-for="file in fileNames" :key="file.id" :value="file.id">
                            <v-list-item-content>
                                <v-list-item-title>{{file.name}}</v-list-item-title>                           
                            </v-list-item-content>
                        </v-list-item>
                    </v-list-item-group>
                </v-list>
            </v-card>
        </v-col>
        <v-col lg="6">
            <v-card v-show="selectedFileName" class="mx-auto">
                <graphic-settings :mode="selectedFileName === 'create' ? 'creating' : 'updating'" :fileName="selectedFileName" :graphics="graphics"></graphic-settings>
            </v-card>
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
import GraphicSettings from './uploader-graphicsettings.vue'
export default {
   
    data() {
        return {
            processes: [],
            selectedProcessId: "",
            snackbar: {text: "", color: "indigo", visible: false},
            selectedFileName: "",
            graphics: []  
        }
    },

    components : {
        "graphic-settings": GraphicSettings
    },

    watch: {
        selectedProcessId: async function(newVal, oldVal) {
            this.selectedFileName = ""
            await this.$http
            .get(`/api/filegraphicuploader/process/${newVal}`)
            .then(response => { 
                this.fileNames = response.data                              
            })
            .catch(error => {                
                if(error.response.status === 404) {
                    this.showSnackBar("Имена файлов не найдены", "pink")
                }
            })
        },

        selectedFileName: async function(newVal, oldVal) {
            if(newVal && newVal !== "create") {
                await this.$http
                .get(`/api/filegraphicuploader/graphics/${newVal}`)
                .then(response => { 
                    this.graphics = response.data                              
                })
                .catch(error => {                
                   
                })
            }
        }
    },

    methods: {

        async initProcesses() {
            await this.$http
            .get(`/api/process/all`)
            .then(response => { 
                this.processes = response.data                              
            })
            .catch(error => {                
                if(error.response.status === 404) {
                    this.showSnackBar("Процессы не найдены", "pink")
                }
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
        await this.initProcesses()
    }
}
</script>

