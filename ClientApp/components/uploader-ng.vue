<template>
  <v-container>
      <v-row>
          <v-col lg="3" offset-lg="1">
            <v-radio-group v-model="selectedCodeProductFolder" @change="clearMeasurementRecordings">
                <v-radio v-for="cpf in codeProductFolders" :key="cpf.folderName" :value="cpf.folderName" :label="cpf.folderName"></v-radio>
            </v-radio-group>
          </v-col>
          <v-col lg="3" offset-lg="1">
            <v-radio-group v-model="selectedWaferFolder" @change="selectedMeasurementRecordings = []">
                <v-radio v-for="wf in waferFolders" :key="wf.folderName" :value="wf.folderName" :label="wf.folderName" ></v-radio>
            </v-radio-group>
          </v-col>
          <v-col lg="3" offset-lg="1">
              <v-checkbox v-model="selectedMeasurementRecordings" v-for="mr in measurementRecordings" :key="mr" :label="mr" :value="mr"></v-checkbox>
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

export default {
    data() {
        return {
            codeProductFolders: [],
            waferFolders: [],
            measurementRecordings: [],
            selectedCodeProductFolder: "",
            selectedWaferFolder: "",
            selectedMeasurementRecordings: [],
            snackbar: {text: "", color: "indigo", visible: false}    
        }
    },

    methods: {
        async initCodeProductFolders() {
            await this.$http
            .get(`/api/folder/folders-cp`)
            .then(response =>  this.codeProductFolders = response.data)
            .catch(err => console.log(err))
        },

        clearMeasurementRecordings() {
            this.measurementRecordings = []
           
        },

        showSnackBar(text, color)
        {
          this.snackbar.text = text
          this.snackbar.color = color
          this.snackbar.visible = true
        }
    },

    computed: {

    },

    watch: {
        

        selectedCodeProductFolder: async function(newVal, oldVal) {
            this.$router.push({ name: 'uploader-cp', params: {selectedCodeProductFolder: newVal}})     
            await this.$http
            .get(`/api/folder/folders-wafer/${newVal}`)
            .then(response => { 
                this.waferFolders = response.data
                              
            })
            .catch(error => {                
                if(error.response.status === 500) {
                    this.showSnackBar("Ошибка при поиске папки", "pink")
                }
            })
            
        },

        selectedWaferFolder: async function(newVal, oldVal) {
            let codeProductFolderName = this.selectedCodeProductFolder          
            if(newVal) {
                this.$router.push({ name: 'uploader-cpw', params: {selectedCodeProductFolder: codeProductFolderName, selectedWaferFolder: newVal}})
                await this.$http
                .get(`/api/folder/folders-idmr/${codeProductFolderName}/${newVal}`)
                .then(response =>  {
                    this.measurementRecordings = response.data.sort((a,b) => (Number(a.match(/(\d+)/g)[0]) - Number((b.match(/(\d+)/g)[0]))))            
                })
                .catch(error => {                
                    if(error.response.status === 500) {
                        this.showSnackBar("Ошибка при поиске папки", "pink")
                    }
                })
            }
          
        },

        selectedMeasurementRecordings: function(newVal, oldVal) {
            let mrArray = newVal.join("$")
            let waferFolderName = this.selectedWaferFolder
            let codeProductFolderName = this.selectedCodeProductFolder
            if(mrArray.length === 0) {
                if(waferFolderName)
                {
                    this.$router.push({ name: 'uploader-cpw', params: {selectedCodeProductFolder: codeProductFolderName, selectedWaferFolder: waferFolderName}})
                }
                else
                {
                     this.$router.push({ name: 'uploader-cp', params: {selectedCodeProductFolder: codeProductFolderName}})
                }
                
            }
            else {
                this.$router.push({ name: 'uploader-cpwi', params: {selectedCodeProductFolder: codeProductFolderName, selectedWaferFolder: waferFolderName, mrArray: mrArray}})
            }
            
        }
    },

    async mounted() {
        
        this.initCodeProductFolders()
        if(this.$route.name === "uploader-cp") {
            this.selectedCodeProductFolder = this.$route.params.selectedCodeProductFolder
        }
        if(this.$route.name === "uploader-cpw") {
            this.selectedCodeProductFolder = this.$route.params.selectedCodeProductFolder
            this.selectedWaferFolder = this.$route.params.selectedWaferFolder
        }
        if(this.$route.name === "uploader-cpwi") {
            this.selectedCodeProductFolder = this.$route.params.selectedCodeProductFolder
            this.selectedWaferFolder = this.$route.params.selectedWaferFolder
            this.selectedMeasurementRecordings = this.$route.params.mrArray.split('$')
        }
    }
}
</script>

<style>

</style>