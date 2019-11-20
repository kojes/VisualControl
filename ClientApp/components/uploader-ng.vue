<template>
  <v-container>
      <v-row>
          <v-col lg="3" offset-lg="1">
            <v-radio-group v-model="selectedCodeProductFolder">
                <v-radio v-for="cpf in codeProductFolders" :key="cpf.folderName" :value="cpf.folderName" :label="cpf.folderName"></v-radio>
            </v-radio-group>
          </v-col>
          <v-col lg="3" offset-lg="1">
            <v-radio-group v-model="selectedWaferFolder">
                <v-radio v-for="wf in waferFolders" :key="wf.folderName" :value="wf.folderName" :label="wf.folderName"></v-radio>
            </v-radio-group>
          </v-col>
          <v-col lg="3" offset-lg="1">
              <v-checkbox v-model="selectedMeasurementRecordings" v-for="mr in measurementRecordings" :key="mr" :label="mr" :value="mr"></v-checkbox>
          </v-col>
      </v-row>
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
            selectedMeasurementRecordings: []
        }
    },

    methods: {
        async initCodeProductFolders() {
            await this.$http
            .get(`/api/folder/folders-cp`)
            .then(response =>  this.codeProductFolders = response.data)
            .catch(err => console.log(err))
        }
    },

    computed: {

    },

    watch: {
        selectedCodeProductFolder: async function(newVal, oldVal) {
            await this.$http
            .get(`/api/folder/folders-wafer/${newVal}`)
            .then(response =>  this.waferFolders = response.data)
            .catch(err => console.log(err))
        },

        selectedWaferFolder: async function(newVal, oldVal) {
            let codeProductFolderName = this.selectedCodeProductFolder
            await this.$http
            .get(`/api/folder/folders-idmr/${codeProductFolderName}/${newVal}`)
            .then(response =>  this.measurementRecordings = response.data.sort((a,b) => (Number(a.match(/(\d+)/g)[0]) - Number((b.match(/(\d+)/g)[0])))))
            .catch(err => console.log(err))
        }
    },

    async mounted() {
        this.initCodeProductFolders()
    }
}
</script>

<style>

</style>