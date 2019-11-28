<template>
  <v-container>
      <v-row>
          <v-col lg="2" offset-lg="1">
            <v-select   v-model="selectedMonitor"
                        :items="monitors"
                        item-text="name"
                        item-value="id"
                        no-data-text="Нет данных"
                        label="Выберите монитор:">
            </v-select>
          </v-col>
          <v-col lg="9">
            <v-simple-table dark>
                <template v-slot:default>
                    <thead>
                        <tr>
                            <th class="text-left">Операция</th>
                            <th class="text-left">Элемент</th>
                            <th class="text-left">Имя файла</th>
                            <th class="text-left">Тип измерения</th>
                            <th class="text-left">Тип карты</th>
                            <th class="text-left">Комментарий</th>
                            <th class="text-left"></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="operation in simpleOperations" :key="operation.name">
                            <td>{{operation.name}}</td>
                            <td>{{operation.element.name}}</td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </tbody>
                </template>
            </v-simple-table>
          </v-col>
      </v-row>
  </v-container>
</template>

<script>
export default {

    props: ["codeProduct", "wafer", "measurementRecordings"],

    data() {
        return {
            selectedMonitor: "",
            simpleOperations: [],
            monitors: []
        }
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
        } 
    },

    async mounted() {
        await this.initMonitors()
    }
}
</script>

<style>

</style>