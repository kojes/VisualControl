<template>
  <v-container>
      <v-row>
          <v-col class="aTop">     
                <v-row>
                    <v-select   
                        v-model="selectedMonitor"
                        :items="monitors"
                        item-text="name"
                        item-value="id"                       
                        no-data-text="Нет данных"
                        label="Выберите монитор:">
                    </v-select>
                </v-row>     
                <v-row>
                    <v-text-field v-model="wafer" readonly label="Пластина:"></v-text-field> 
                </v-row>
                <v-row>
                    <v-text-field v-model="originalCodeProduct.name" readonly label="Шаблон:"></v-text-field> 
                </v-row>
                 <v-row>
                    <v-btn v-if="!readyToUploading" @mouseover="errorHighlight = true" @mouseleave="errorHighlight = false" block outlined color="pink">Загрузка невозможна</v-btn>
                    <v-btn v-else-if="simpleOperations.every(x=> x.uploadStatus==='already' || x.uploadStatus === 'done')" block color="success">Измерения загружены</v-btn>
                    <v-btn v-else-if="simpleOperations.filter(x => x.uploadStatus==='initial').length === 0" block color="indigo">Обновление статуса загрузки</v-btn>
                    <v-btn v-else block color="teal" @click="upload">{{`Загрузить ${simpleOperations.filter(x => x.uploadStatus==='initial').length} измерений`}}</v-btn>
                </v-row>   
                <v-row>
                    <v-col lg="12">
                        <v-card class="mt-6" elevation="8" tile>
                            <v-list>
                                <v-subheader>Выбор этапов</v-subheader>                   
                                <v-list-item v-for="measurementRecording in measurementRecordingsWithStage" :key="measurementRecording.id">                       
                                    <v-list-item-content>
                                        <v-tooltip top>
                                            <template v-slot:activator="{ on }">
                                                <v-chip v-on="on" label :color="measurementRecording.stage.name ? 'success' : 'pink'">
                                                    {{measurementRecording.id}}
                                                </v-chip>
                                            </template>
                                            <span v-if="measurementRecording.stage.name">{{measurementRecording.stage.name}}</span>
                                            <span v-else>Этап не выбран</span>
                                        </v-tooltip>
                                    </v-list-item-content>
                                    <v-list-item-icon>
                                        <v-menu v-model="measurementRecording.stage.menu" :close-on-content-click="false" :nudge-width="300">
                                            <template v-slot:activator="{ on }">
                                                <v-icon v-on="on" color="deep-purple lighten-5">edit</v-icon>   
                                            </template>
                                            <v-card>
                                                <v-row>
                                                    <v-col lg="12" class="px-8">
                                                            <v-text-field v-if="stageCreationMode" v-model="newStageName" :error-messages="newStageValidation" label="Название этапа:"></v-text-field> 
                                                            <v-select v-else          
                                                                :items="stages"
                                                                @change="stageChanged(measurementRecording)"
                                                                v-model="measurementRecording.stage.id"
                                                                no-data-text="Нет данных"
                                                                item-text="stageName"
                                                                item-value="stageId"
                                                                label="Этап">
                                                            </v-select>
                                                    </v-col>
                                                </v-row>  
                                                <v-row>
                                                    <v-col lg="10" offset-lg="2" class="px-8">
                                                        <v-btn v-if="stageCreationMode" v-show="!newStageValidation" block color="success" @click="createStage(measurementRecording)">Добавить этап в БД</v-btn>
                                                        <v-btn v-else block color="indigo" @click="stageCreationMode=true">Добавить новый этап</v-btn>
                                                    </v-col>
                                                </v-row>                                              
                                            </v-card>
                                        </v-menu>                                                                           
                                    </v-list-item-icon>
                                </v-list-item>                    
                            </v-list>
                        </v-card>
                    </v-col>                    
                </v-row>          
                     
          </v-col>
          <v-col lg="10" offset-lg="2">
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
                            <th class="text-left">Статус загрузки</th>
                                                
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-bind:class="{ rotatingBorder: errorHighlight && !(operation.fileName.graphicNames && operation.element.elementId)}"  v-for="operation in simpleOperations" :key="operation.guid">
                            <td><v-chip label color="grey darken-2"> {{operation.name + '_' + operation.element.name}}</v-chip></td>
                            <td>
                                <v-row>
                                        <v-col lg="9" class="text-lg-center">                                        
                                            <v-tooltip v-if="operation.element.elementId" bottom>
                                                <template v-slot:activator="{ on }">
                                                    <v-chip class="me-4" v-on="on" label color="indigo">
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
                                <td v-if="operation.fileName.graphicNames">
                                    <v-text-field v-if="operation.fileName.graphicNames.length === 1" class="mt-6" :value="operation.fileName.graphicNames[0]" readonly outlined></v-text-field>
                                    <v-select v-else class="mt-6"
                                        v-model="operation.fileName.selectedGraphicNames"
                                        :items="operation.fileName.graphicNames"
                                        outlined
                                        no-data-text="Нет данных">
                                    </v-select>
                                </td>    
                                <td v-else>
                                    <v-tooltip bottom>
                                        <template v-slot:activator="{ on }">
                                            <v-icon v-on="on" color="pink">warning</v-icon>
                                        </template>
                                        <span>Тип измерения не найден</span>
                                    </v-tooltip>                                     
                                </td>                            
                                <td><v-text-field class="mt-6" v-model="operation.mapType" outlined label="Тип карты:"></v-text-field></td>
                                <td><v-text-field class="mt-6" v-model="operation.comment" outlined label="Комментарий:"></v-text-field></td>  
                                <td>
                                    <v-chip v-if="operation.uploadStatus === 'done'" color="success" text-color="white" @click="copyShortLinkToClipboard(operation.shortLink)">
                                        <v-avatar left>
                                            <v-icon>check_circle_outline</v-icon>
                                        </v-avatar>
                                        Загружено
                                    </v-chip>
                                    <v-chip v-else-if="operation.uploadStatus === 'rejected'" color="pink" text-color="white" >
                                        <v-avatar left>
                                            <v-icon>error_outline</v-icon>
                                        </v-avatar>
                                        Ошибка при загрузке
                                    </v-chip>
                                    <v-chip v-else-if="operation.uploadStatus === 'pending'" color="indigo" text-color="white" >
                                        <v-avatar left>
                                            <v-progress-circular size="16" width="3" indeterminate color="white"></v-progress-circular>
                                        </v-avatar>
                                        Загрузка
                                    </v-chip>
                                    <v-chip v-else-if="operation.uploadStatus === 'already'" color="teal" text-color="white" close close-icon="delete" @click:close="deleteSpecific(operation)">
                                         <v-avatar left>
                                            <v-icon>check_circle_outline</v-icon>
                                        </v-avatar>                                        
                                        Уже загружено
                                    </v-chip>                                    
                                    <v-chip v-else-if="operation.uploadStatus === 'initial'" color="cyan lighten-2" text-color="white" close close-icon="delete" @click:close="deleteRow(operation.guid)">
                                        <v-avatar left>
                                            <v-icon>schedule</v-icon>
                                        </v-avatar>
                                        Ожидает загрузки
                                    </v-chip>
                                    <v-chip v-else color="indigo" text-color="white" >
                                        Обновление статуса
                                    </v-chip>       
                                </td>                           
                        </tr>
                    </tbody>
                </template>
            </v-simple-table>
          </v-col>
      </v-row>
  </v-container>
</template>

<script>
import ElementCreate from './create-elementuploader.vue'
export default {

    props: ["codeProduct", "wafer", "measurementRecordings"],

    data() {
        return {
            loading: {dialog: false, text: ""},
            measurementRecordingsWithStage: [],
            newStageName: "",
            selectedMonitor: "",
            originalCodeProduct: {},
            errorHighlight: false,
            stageCreationMode: false,
            simpleOperations: [],
            stages: [],
            monitors: [],
        }
    },

    components: {
        "create-element": ElementCreate
    },

    watch: {
        selectedMonitor: async function(newVal, oldVal) {
            this.showLoading("Получение списка измерений")
            this.$http.get(`/api/folder/simpleoperation/${this.codeProduct}/${this.wafer}/${newVal}`, 
            {
                params: {
                  measurementRecordingsJSON: JSON.stringify(this.measurementRecordings)
                }
            })
            .then(response => {
                this.simpleOperations = response.data.map(d => ({ ...d, uploadStatus: '', shortLink: '' }))
                this.checkUploadingStatus(this.simpleOperations)
            })
            .catch(error => {
                this.showSnackBar("Ошибка при загрузке операций")
            })              
        }
    },

    computed: {
        userName() {
            return this.$store.state.authentication.user.firstName + " " + this.$store.state.authentication.user.surname
        },
        readyToUploading() {
            return this.simpleOperations.length > 0 && this.simpleOperations.every(so => so.fileName.graphicNames && so.element.elementId)
        },
        newStageValidation() {
            if(!this.newStageName)
                return "Введите название этапа"
            if(this.stages.some(x => x.stageName === this.newStageName))
                return "Такое название уже существует"
            return ""
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
            .catch(err => this.showSnackBar("Ошибка при загрузке списка мониторов"))
        },

        async getAllStages(waferId) {
            await this.$http
            .get(`/api/stage/wafer/${waferId}`)
            .then(response => {
                this.stages = response.data
            })
            .catch((error) => {
                
            });
        },

        async getOriginalCodeProduct(waferId) {
            await this.$http
            .get(`/api/codeproduct/getbywaferid?waferId=${waferId}`)
            .then(response => {
                this.originalCodeProduct = response.data
            })
            .catch((error) => {
                this.showSnackBar("Ошибка при загрузке шаблона")
            });
        },

        async createStage(measurementRecording) {
            let codeProductId = this.originalCodeProduct.id
            let stageName = this.newStageName
            await this.$http({
                    method: "put",
                    url: `/api/stage/create/name/${stageName}/codeProductId/${codeProductId}`, 
                    config: {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                }
            })
            .then(response => {
                this.stageCreationMode = false
                this.newStageName = ""
                this.stages.push(response.data)
                measurementRecording.stage.id = response.data.stageId
                this.stageChanged(measurementRecording)
                this.showSnackBar("Этап добавлен к текущему проекту")
            })            

        },

        async upload() {
            for (let index = 0; index < this.simpleOperations.length; index++) {
                let so = this.simpleOperations[index]
                if(so.uploadStatus !== "already")
                {
                    so.uploadStatus = "pending"
                    await this.$http({
                        method: "post",
                        url: `/api/uploading`, 
                        data: {operationName: `${so.name}_${so.element.name}`, 
                            bigMeasurementName: so.name,
                            stageId: so.stageId,
                            elementId: so.element.elementId, 
                            codeProductId: this.originalCodeProduct.id, 
                            waferId: this.wafer,
                            userName: this.userName,
                            map: so.mapType,
                            comment: so.comment,
                            path: so.path,
                            graphicNames: so.fileName.selectedGraphicNames.split(';')}, 
                        config: {
                            headers: {
                                'Accept': "application/json",
                                'Content-Type': "application/json"
                            }
                        }
                    })
                    .then(response => {
                        so.uploadStatus = "done"
                        so.shortLink = response.data
                    })
                    .catch(error => {
                        so.uploadStatus = "rejected"
                    });           
                }
            }
        },

        async checkUploadingStatus(simpleOperations) {
            this.showLoading("Получение статуса измерений")
            let dataSo = simpleOperations.map(so => ({
                        guid: so.guid,         
                        operationName: `${so.name}_${so.element.name}`, 
                        codeProductId: this.originalCodeProduct.id, 
                        waferId: this.wafer,                           
                        graphicNames: so.fileName.selectedGraphicNames ? so.fileName.selectedGraphicNames.split(';') : []})
            )
            await this.$http({
                method: "post",
                url: `/api/uploading/checkUploadingStatus`, 
                data: dataSo, 
                config: {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                }
            })
            .then(response => {
                response.data.forEach(x => {
                    let simpleOperation = simpleOperations.find(so => so.guid === x.guid)
                    simpleOperation.uploadStatus = x.uploadStatus
                    simpleOperation.alreadyData = x.alreadyData || []
                })                 
                this.closeLoading()
            })
            .catch(error => {
                this.showSnackBar("Ошибка соединения с БД")
                this.closeLoading()
            }); 
        },

        async deleteSpecific(simpleOperation) {
            simpleOperation.alreadyData.forEach(async ad => {
                await this.$http.delete(`/api/measurementrecording/deletespecific/${ad.measurementRecordingId}/${ad.graphicId}`, {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                })
                .then(response => {
                    if(response.status === 204) {
                        this.showSnackBar("График успешно удален")
                    }
                    else {
                        this.showSnackBar("Ошибка при удалении")
                    }                   
                })
            })
            simpleOperation.uploadStatus = "initial";        
        },

        copyShortLinkToClipboard(shortLink) {

        },

        stageChanged(measurementRecording) {
            measurementRecording.stage.menu = false
            measurementRecording.stage.name = this.stages.find(s => s.stageId === measurementRecording.stage.id).stageName
            this.simpleOperations.forEach(so => {
                if(so.name === measurementRecording.id) {
                    so.stageId = measurementRecording.stage.id
                }                
            });
        },

        deleteRow(guid) {
            this.simpleOperations = this.simpleOperations.filter(so => so.guid !== guid)
        },

        elementCreated(element) {
            this.simpleOperations.filter(so => so.element.name === element.name).map(so => {
                so.element.elementId = element.elementId
            })
        },

        showSnackBar(text) {
            this.$store.dispatch("alert/success", text)        
        },

        showLoading(text) {
            this.$store.dispatch("loading/show", text)        
        },

        closeLoading() {
            this.$store.dispatch("loading/cloak")        
        }
    },

    async mounted() {
        await this.initMonitors()
        await this.getOriginalCodeProduct(this.wafer)
        await this.getAllStages(this.wafer)
        this.measurementRecordingsWithStage = this.measurementRecordings.map(x => ({id: x, stage: {id: 0, name: "", menu: false}}))

    }
}
</script>

<style scoped>
    .aTop {       
        position: fixed; 
        top: 75px; 
        max-width: 14.5%;
    }

    .rotatingBorder {
        width: max-content;
        background: linear-gradient(90deg, #e91e63 50%, transparent 50%), linear-gradient(90deg, #e91e63 50%, transparent 50%), linear-gradient(0deg, #e91e63 50%, transparent 50%), linear-gradient(0deg, #e91e63 50%, transparent 50%);
        background-repeat: repeat-x, repeat-x, repeat-y, repeat-y;
        background-size: 5px 2px, 5px 2px, 2px 5px, 2px 5px;
        padding: 10px;
        animation: border-dance 15s infinite linear;
    }

    @keyframes border-dance {
        0% {
            background-position: 0 0, 100% 100%, 0 100%, 100% 0;
        }
        100% {
            background-position: 100% 0, 0 100%, 0 0, 100% 100%;
        }
    }
    
</style>