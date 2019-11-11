<template>
    <v-container grid-list-lg>
        <v-layout v-if="successfulLoading" row>
            <v-flex lg-8>
                <v-toolbar color="indigo" dark>
                <v-btn absolute
                        dark
                        fab
                        small
                        right
                        color="pink"
                        @click="openAddDialog()">
                    <v-icon>add</v-icon>
                </v-btn>
                <v-toolbar-title>Приборы для тестирования</v-toolbar-title>                
                </v-toolbar>

                <v-list>
                <v-divider></v-divider>
                <v-list-item v-for="device in devices"
                            :key="device.name">
                
                    <v-list-item-avatar size="100">
                    ID: {{ device.id }}
                    </v-list-item-avatar>
                    
                    <v-list-item-content>
                    <v-list-item-title>{{ device.name }}</v-list-item-title>
                    <v-list-item-subtitle>{{ device.address }}</v-list-item-subtitle>
                    </v-list-item-content>
                   
                    <div>
                        <v-tooltip bottom>
                            <template v-slot:activator="{ on }">
                                <v-btn icon ripple @click="openEditDialog(device)">
                                    <v-icon color="grey">edit</v-icon>
                                </v-btn>
                            </template>
                        <span>Редактировать адрес прибора</span>
                        </v-tooltip>

                        <v-tooltip bottom>
                            <template v-slot:activator="{ on }">
                                <v-btn icon ripple @click="deleteDevice(device.id)">
                                    <v-icon color="grey">delete</v-icon>
                                </v-btn>
                            </template>
                        <span>Удалить прибор</span>
                        </v-tooltip>                     
                      
                    </div>

                </v-list-item>
                <v-divider></v-divider>

                </v-list>
            </v-flex>
        </v-layout>
        <v-layout row v-else>
            <v-alert :value="true" type="error">
                Ошибка при загрузке информации о приборах
            </v-alert>
        </v-layout>
        <v-layout row justify-center>
            <v-dialog v-model="createDialog" persistent max-width="600px">

            <v-card>
                <v-card-title>
                    <span class="headline">Добавление нового прибора</span>
                </v-card-title>
                <v-card-text>
                    <v-layout row>                        
                        <v-flex lg4>
                             <v-text-field  v-model="newDevice.name" 
                                            :error-messages="rules.name.required || rules.name.duplicate
                                                                             ? rules.name.errorMessages[0]
                                                                             : []" 
                                            @change="validate('name', newDevice.name)" outlined label="Имя прибора:">
                            </v-text-field>
                        </v-flex>

                        <v-flex lg7 offset-lg1>
                            <v-text-field   v-model="newDevice.address" 
                                            :error-messages="rules.address.required || rules.address.duplicate
                                                                             ? rules.address.errorMessages[0]
                                                                             : []" 
                                            @change="validate('address', newDevice.address)" outlined label="Адрес:">
                            </v-text-field>
                        </v-flex>
                    </v-layout>
                    <v-layout row>
                        <v-flex lg7 offset-lg5>
                            <v-select   v-model="newDevice.deviceType"
                                        :items="deviceTypes"
                                        item-text="model"
                                        item-value="model"
                                        no-data-text="Нет данных"
                                        outlined
                                        label="Выберите модель прибора:">
                            </v-select>
                        </v-flex>
                    </v-layout>
                </v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn color="grey" text @click="createDialog = false">Отмена</v-btn>
                    <v-btn color="green" text @click="createDevice">Добавить</v-btn>
                </v-card-actions>

            </v-card>
            </v-dialog>

            <v-dialog v-model="editDialog" persistent max-width="600px">

            <v-card>
                <v-card-title>
                    <span class="headline">Редактирование адреса прибора</span>
                </v-card-title>
                <v-card-text>
                    <v-layout row>        
                        <v-flex lg7 offset-lg1>
                            <v-text-field v-model="editedAddress.oldValue" outlined readonly label="Предыдущий адрес:">
                            </v-text-field>
                        </v-flex>
                    </v-layout>
                     <v-layout row>        
                        <v-flex lg7 offset-lg1>
                            <v-text-field   v-model="editedAddress.newValue" 
                                            :error-messages="rules.address.required || rules.address.duplicate
                                                                             ? rules.address.errorMessages[0]
                                                                             : []" 
                                            @change="validate('address', editedAddress.newValue)" outlined label="Новый адрес:">
                            </v-text-field>
                        </v-flex>
                    </v-layout>                           
                </v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn color="grey" text @click="editDialog = false">Отмена</v-btn>
                    <v-btn color="green" text @click="editAddress">Изменить адрес</v-btn>
                </v-card-actions>

            </v-card>
            </v-dialog>
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
            devices: [],
            newDevice: {name: "", address: "", deviceType: ""},
            editedAddress: {deviceId: "", oldValue: "", newValue: ""},
            deviceTypes: [],
            createDialog: false,
            editDialog: false,
            successfulLoading: true,
            snackbar: {text: "", color: "success", visible: false},
            rules: { address: {require: false, duplicate: false, errorMessages: []}, name: {require: false, duplicate: false, errorMessages: []} }
        }
    },    

    watch:
    {
        createDialog: function(value) {
          
            if(!value)
                Object.keys(this.newDevice).forEach(_ => this.newDevice[_] = "")
            this.newDevice.deviceType = this.deviceTypes[0].model
        },
        editDialog: function(value) {
             if(!value)
               Object.keys(this.editedAddress).forEach(_ => this.editedAddress[_] = "")
        }
    },

    methods:
    {
      validate(parameter, value)
      {
        let validation = this.rules[parameter];
        if(!value) {
            validation.require = true
            validation.errorMessages.push("Поле не должно быть пустым")
        } else {
            validation.require = false
            this.$http
            .get(`/api/device/${parameter}/${value}`)
            .then((response) => {
                if(response.status === 200) {  
                    validation.errorMessages.push("Такой прибор уже есть")   
                    validation.duplicate = true
                }                        
            })
            .catch((error) => {
                
                if(error.response.status === 404) {
                    validation.duplicate = false   
                } else {
                    validation.errorMessages.push("Неизвестная ошибка")
                    validation.duplicate = true
                } 
            });                  
        }

      },
      openAddDialog()
      {
        this.createDialog = true;      
        if(this.deviceTypes.length === 0)  
        {
            this.$http
            .get(`/api/devicetype/all`)
            .then((response) => {
                             
                if(response.status === 200) {  

                    this.deviceTypes = response.data; 
                    this.newDevice.deviceType = response.data[0].model                              
                  
                }               
            })
            .catch((error) => {
                
            });   
        }
        
      },

      openEditDialog(device)
      {
          this.editDialog = true
          this.editedAddress.oldValue = device.address
          this.editedAddress.deviceId = device.id
      },

      async editAddress()
      {
            const id = this.editedAddress.deviceId
            const address = this.editedAddress.newValue
            const editedDevice  = this.devices.find(_ => _.id === id)
            const response = await this.$http({
                method: "post",
                url: `/api/device/edit`,
                data: {id: id, address: address, name: editedDevice.name, model: editedDevice.model}, 
                config: {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                }
            })
            .then((response) => {
                this.showSnackBar("Адрес удачно обновлен")
                let editedDevice  = this.devices.find(_ => _.id === id)
                editedDevice.address = address
            })
            .catch((error) => {
                this.showSnackBar("Невозможно обновить адрес", "error")
            });      
            this.editDialog = false 
      },

      async deleteDevice(id)
      {
         
         const response = await this.$http({
                method: "delete",
                url: `/api/device/` + id
            })
            .then((response) => {
                this.showSnackBar("Прибор успешно удален")
                this.devices = this.devices.filter(x => x.id !== id)
            })
            .catch((error) => {
                this.showSnackBar("Невозможно удалить прибор", "error")
            });            
    
      },

      showSnackBar(text, color)
      {
          this.snackbar.text = text
          this.snackbar.color = color
          this.snackbar.visible = true
      },

      async createDevice()
      {
          const {name, address} = this.newDevice
          const model = this.newDevice.deviceType
          const response = await this.$http({
                method: "put",
                url: `/api/device`, 
                data: {name: name, address: address, model: model}, 
                config: {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                }
            })
            .then((response) => {
                this.devices.push(response.data)
                this.createDialog = false
                this.showSnackBar("Прибор успешно добавлен")
            })
            .catch((error) => {
                this.showSnackBar("Ошибка при добавлении", "error")
            });   
      },

      openDeleteDialog(device) {
      
        this.deleteDialog = true;
       
      },
    },

    async mounted()
    {
         await this.$http
            .get(`/api/device/all`)
            .then((response) => {
                             
                if(response.status == 200) {  
                    this.devices = response.data                                  
                    this.successfulLoading = true
                }
                else {
                   this.successfulLoading = false
                }
            })
            .catch((error) => {
                this.successfulLoading = false
            });   
    }
}
</script>