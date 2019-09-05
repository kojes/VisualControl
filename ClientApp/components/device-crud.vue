<template>
    <v-container grid-list-lg>
        <v-layout row>
            <v-flex lg-8>
                <v-toolbar color="indigo" dark>
                <v-btn absolute
                        dark
                        fab
                        small
                        right
                        color="pink"
                        @click="openAddDialog">
                    <v-icon>add</v-icon>
                </v-btn>
                <v-toolbar-title>Приборы для тестирования</v-toolbar-title>                
                </v-toolbar>

                <v-list>
                <v-divider></v-divider>
                <v-list-tile v-for="device in devices"
                            :key="device.name"
                            @click="">
                
                    <v-list-tile-avatar>
                    <avatar :username="device.model"
                            background-color="#FFFEEE"
                            :size ="30">
                    </avatar>
                    </v-list-tile-avatar>

                    <v-list-tile-content>
                    <v-list-tile-title>{{ device.name}}</v-list-tile-title>
                    <v-list-tile-sub-title>{{ device.address }}</v-list-tile-sub-title>
                    </v-list-tile-content>
                   
                    <v-list-tile-action>
                    <v-btn icon ripple @click="openDeleteDialog(device)">
                        <v-icon color="grey">delete</v-icon>
                    </v-btn>
                    </v-list-tile-action>

                </v-list-tile>
                <v-divider></v-divider>

                </v-list>
            </v-flex>
        </v-layout>
    </v-container>
</template>


<script>
import Avatar from 'vue-avatar'
export default {

    data() {
        return {
            devices: [],
            openDialog: false,
            deleteDialog: false
        }
    },  

    components:
    {
        Avatar
    },

    methods:
    {
      openAddDialog()
      {
        this.openDialog = true;        
        
      },
      openDeleteDialog(device) {
      
        this.deleteDialog = true;
       
      },
    },

    async created()
    {
        let devices = await this.$http.get(`/api/device/getall`);
        if(devices.status == 200)
        {
            this.devices = devices.data;
        }
        else
        {
            //Ошибка 404: нет девайсов
        }



    }
}
</script>