<template>
  <v-container>
      <v-layout>
        <v-flex lg10 lg-offset1>
            <v-tabs dark fixed-tabs>
                <v-tab key="edit">
                    Редактирование параметров
                </v-tab>
                <v-tab-item key="edit">
                    <v-layout>
                        <v-flex lg12>
                            <v-tabs v-model="activeSettings" grow>
                                <v-tab key="measuredDeviceTab">
                                    MeasuredDevice
                                </v-tab>
                                <v-tab-item key="measuredDeviceTab">
                                    <v-card flat>
                                        <v-card-text><measureddevice-crud v-if="activeSettings===0"></measureddevice-crud></v-card-text>
                                    </v-card>
                                </v-tab-item>
                                <v-tab key="measurementTab">
                                    Measurement
                                </v-tab>
                                <v-tab-item key="measurementTab">
                                    <v-card flat>
                                        <v-card-text>measurementTab</v-card-text>
                                    </v-card>
                                </v-tab-item>
                                <v-tab key="deviceTab">
                                    Device
                                </v-tab>
                                <v-tab-item key="deviceTab">
                                    <v-card flat>
                                        <v-card-text>
                                            <device-crud v-if="activeSettings===2"></device-crud>
                                        </v-card-text>
                                    </v-card>
                                </v-tab-item>
                                <v-tab key="graphicTab">
                                    Graphic
                                </v-tab>
                                <v-tab-item key="graphicTab">
                                    <v-card flat>
                                        <v-card-text>graphicTab</v-card-text>
                                    </v-card>
                                </v-tab-item>
                            </v-tabs>
                        </v-flex>
                    </v-layout>
                </v-tab-item>
                <v-tab key="create">
                    Добавление нового испытания
                </v-tab>
                <v-tab-item key="edit">
                    <v-card flat>
                        <v-card-text><measurement-crud></measurement-crud></v-card-text>
                    </v-card>
                </v-tab-item>
            </v-tabs>
        </v-flex>
      </v-layout>
  </v-container>
</template>

<script>
import DevicesSettings from './device-crud.vue';
import MeasuredDevicesSettings from './measureddevice-crud.vue';
import MeasurementCreating from './measurement-crud.vue'
export default {
    
    data() {
        return {
           activeSettings: 0
        }
    },

    components:
    {
        "device-crud": DevicesSettings,
        "measureddevice-crud": MeasuredDevicesSettings,
        "measurement-crud": MeasurementCreating 
    },

    methods: {

    },

    async created() {

    }

}
</script>

<style>

</style>
