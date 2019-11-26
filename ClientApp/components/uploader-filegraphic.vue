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
            <v-card v-if="selectedFileName" class="mx-auto">
                <file-creating v-if="selectedFileName === 'create'" @file-created="fileCreated" @show-snackbar="showSnackBar" :processId="selectedProcessId" :fileNames="fileNames"></file-creating>
                <file-updating v-else @show-snackbar="showSnackBar" @file-deleted="fileDeleted" :processId="selectedProcessId" :fileName="fileNames.find(x => x.id === selectedFileName)"></file-updating>
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
import FileCreating from './uploader-graphicsettings.vue'
import FileUpdating from './uploader-fileupdating.vue'
export default {
   
    data() {
        return {
            processes: [],
            selectedProcessId: "",
            snackbar: {text: "", color: "indigo", visible: false},
            selectedFileName: "",
            graphics: [],
            fileNames: []
        }
    },

    components : {
        "file-creating": FileCreating,
        "file-updating": FileUpdating
    },

    watch: {
        selectedProcessId: async function(newVal, oldVal) {
            this.selectedFileName = ""
            this.fileNames = []
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

        fileCreated(fileName) {
            this.fileNames.push(fileName)
        },

        fileDeleted(fileNameId) {
            this.fileNames = this.fileNames.filter(x => x.id !== fileNameId)
            this.selectedFileName = 'create'
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

