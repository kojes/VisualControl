<template>
 <v-container>
    <v-layout row>
        <v-card>
        <v-card-title>
       <v-dialog v-model="createDialog" max-width="500px">
        <template v-slot:activator="{ on }">
          <v-btn color="indigo" dark class="mb-2" v-on="on">Добавить устройство</v-btn>
        </template>
        <v-card>
          <v-card-title>
            <span class="headline">{{ }}</span>
          </v-card-title>

          <v-card-text>
            <v-container grid-list-md>
              <v-layout wrap>
                  <v-flex lg9 offset-lg1>
                     <v-autocomplete v-model="newMeasuredDevice.waferId"
                        :items="wafers"
                        item-text="waferId"
                        item-value="waferId"
                        no-data-text="Нет данных"
                        filled
                        outlined
                        label="Выберите пластину">
                    </v-autocomplete>
                  </v-flex>                  
              </v-layout>
               <v-layout>
                   <v-flex lg9 offset-lg1>
                      <v-text-field v-model="newMeasuredDevice.name"         :error-messages="rules.name.required
                                                                             ? rules.name.errorMessages[0]
                                                                             : []" outlined label="Номер кристалла:">
                      </v-text-field>
                  </v-flex>
               </v-layout>
            </v-container>
          </v-card-text>

          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="white" outlined @click="createDialog=false">Отмена</v-btn>
            <v-btn color="green" outlined @click="createMeasuredDevice">Добавить</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
      <v-spacer></v-spacer>
      <v-text-field
        v-model="search"
        append-icon="search"
        label="Поиск"
        single-line
        hide-details
      ></v-text-field>
    </v-card-title>
    <v-data-table
      :loading="loading"
      :headers="headers"
      :items="measuredDevices"
      :search="search"
      rows-per-page-text = "Устройств на странице"
      class="elevation-1"
    >
      <template v-slot:items="props">
        <td class="text-xs-center">{{ props.item.measuredDeviceId }}</td>
        <td class="text-xs-center">{{ props.item.name }}</td>
        <td class="text-xs-center">{{ props.item.waferId }}</td>
      </template>
      <template v-slot:no-data>
       <v-alert v-if="!loading" :value="true" color="error" icon="warning">
            Не удалось загрузить данные
      </v-alert>
      </template>
      <template v-slot:no-results>
        <v-alert :value="true" color="error" icon="warning">
          Не найдено элементов удовлетворяющих условию: "{{ search }}"
        </v-alert>
      </template>
    </v-data-table>
    </v-card>
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
export default {
    data() {
        return {
            measuredDevices: [],
            wafers: [],
            newMeasuredDevice: {name: "", waferId: ""},
            headers:    [{ text: 'ID', value: 'id', align: 'center', sortable: false},
                        { text: 'Код кристалла', value: 'name', align: 'center', sortable: false },
                        { text: 'Номер пластины', value: 'waferId', align: 'center', sortable: false }],
            createDialog: false,
            loading: true,
            search: "",
            snackbar: {text: "", color: "success", visible: false},
            rules: {name: {require: false, errorMessages: []} }
        }
    },    
    watch: 
    {
        createDialog: function(value) {          
            if(!value) {
                this.rules.name.require = false
                this.rules.name.errorMessages = []  
                Object.keys(this.newMeasuredDevice).forEach(_ => this.newMeasuredDevice[_] = "")
            }               
            else
                this.getWafers()
        }
    },

    methods: 
    {
       async initialize () {
           await this.$http
            .get(`/api/measureddevice/all`)
            .then((response) => {                             
                if(response.status == 200) {  
                    this.measuredDevices = response.data 
                }
                this.loading = false      
            })
            .catch((error) => {
                this.loading = false
            });
               
       },
      
       async getWafers() {
           await this.$http
            .get(`/api/wafer/getall`)
            .then((response) => {                             
                if(response.status == 200) {  
                    this.wafers = response.data 
                }
                    
            })
            .catch((error) => {
                this.showSnackBar("Не удалось загрузить пластины", "error")
            });
       },

       async createMeasuredDevice() {
          const {name, waferId} = this.newMeasuredDevice    
          if(!name) {
            this.rules.name.require = true
            this.rules.name.errorMessages.push("Введите имя кристалла")   
          }
          else {
            
            const response = await this.$http({
                method: "put",
                url: `/api/measureddevice`, 
                data: {name: name, waferId: waferId}, 
                config: {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                }
            })
            .then((response) => {
                if(response.status === 201) {
                    this.measuredDevices.push(response.data)
                    this.createDialog = false
                    this.showSnackBar("Устройство успешно добавлено")
                }              
              
            })
            .catch((error) => {
                this.showSnackBar(error.response.data[0].message, "error")
            });  
          }
        },

        showSnackBar(text, color)
        {
          this.snackbar.text = text
          this.snackbar.color = color
          this.snackbar.visible = true
        }
    },
    
    async mounted()
    {
        this.initialize()
    }
}
</script>