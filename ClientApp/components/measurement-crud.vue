<template>
    <v-container>
        <v-layout row>
            <v-flex lg4 offset-lg1>
                <v-text-field v-model="newMeasurement.name" outlined label="Название измерения:">
                </v-text-field>
            </v-flex>
            <v-flex lg4 offset-lg1>
                <v-text-field v-model="newMeasurement.intervalInSeconds" outlined label="Интервал измерений в секундах:">
                </v-text-field>
            </v-flex>
        </v-layout>
        <v-layout row>
            <v-flex lg4 offset-lg1>
                <v-select   v-model="newMeasurement.materialId"
                            :items="materials"
                            no-data-text="Нет данных"
                            outlined
                            item-text="name"
                            item-value="materialId"
                            label="Выберите материал:">
                </v-select>
            </v-flex>
            <v-flex lg4 offset-lg1>
                <v-select   v-model="newMeasurement.facilityId"
                            :items="facilities"
                            no-data-text="Нет данных"
                            outlined
                            item-text="name"
                            item-value="facilityId"
                            label="Выберите установку:">
                </v-select>
            </v-flex>
        </v-layout>
        <v-layout row>
            <v-flex lg4 offset-lg1>
                 <v-select  v-model="wafers.selected"
                            :items="wafers.items"
                            no-data-text="Нет данных"
                            outlined
                            v-on:change="setDieCodes(wafers.selected)"
                            label="Выберите пластину:">
                </v-select>
            </v-flex>
            <v-flex lg4 offset-lg1> 
                <v-select  v-model="dieCodes.selected"
                            :items="dieCodes.items"
                            no-data-text="Нет данных"
                            outlined
                            label="Выберите номер кристалла:">
                </v-select>
            </v-flex>
        </v-layout>
        <v-layout row>
             <v-flex lg4 offset-lg1>
             </v-flex>
             <v-flex lg4 offset-lg1>
                <v-btn color="indigo" outlined @click="selectPattern()">Выбрать</v-btn>
             </v-flex>
        </v-layout>
    </v-container>
</template>

<script>
    export default {
        data() {
            return {
                measuredDevices: [],
                materials: [],
                facilities: [],
                dieCodes: {selected: "", items: []},
                selectedDie: "",
                newMeasurement: {name: "", materialId: 0, measuredDeviceId: 0, facilityId: 0, intervalInSeconds: 60}
            }
        },  
        
    computed: 
    {
        wafers()
        {
            const items = [...new Set(this.measuredDevices.map(x => x.waferId))]
            const selectedWaferId = items[0]
            this.setDieCodes(selectedWaferId)
            return {selected: selectedWaferId, items: items}
        }
    },

    methods: 
    {
        initalize() {
            this.initMeasuredDevices()
            this.initMaterials()
            this.initFacilities()
        },

        async initMeasuredDevices() {
            await this.$http
            .get(`/api/measureddevice/all`)
            .then((response) => {                             
                if(response.status == 200) {  
                    this.measuredDevices = response.data
                   
                }
                 
            })
            .catch((error) => {
                //snack
            });
        },

        async initMaterials() {
            await this.$http
            .get(`/api/material/getall`)
            .then((response) => {                             
                if(response.status == 200) {  
                    this.materials = response.data 
                    this.newMeasurement.materialId = this.materials[0].materialId
                }
                 
            })
            .catch((error) => {
                //snack
            });
        },

        async initFacilities() {
            await this.$http
            .get(`/api/facility/getall`)
            .then((response) => {                             
                if(response.status == 200) {  
                    this.facilities = response.data 
                    this.newMeasurement.facilityId = this.facilities[0].facilityId
                }
                 
            })
            .catch((error) => {
                //snack
            });
        },

        async createMeasurement()
        {
          const {name, materialId, measuredDeviceId, facilityId, intervalInSeconds} = this.newMeasurement
          const response = await this.$http({
                method: "put",
                url: `/api/measurement/create`, 
                data: {name: name, materialId: materialId, measuredDeviceId: measuredDeviceId, facilityId: facilityId, intervalInSeconds: intervalInSeconds}, 
                config: {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                }
            })
            .then((response) => {
                this.createDialog = false
                this.showSnackBar("Измерение успешно добавлено")
            })
            .catch((error) => {
                this.showSnackBar("Ошибка при добавлении", "error")
            });   
        },

        setDieCodes(selectedWaferId) {
             this.dieCodes.items = this.measuredDevices.filter(x => x.waferId === selectedWaferId).map(x => x.name)
             this.dieCodes.selected = this.dieCodes.items[0]
        }
    },

    async mounted() {
        this.initalize()
    }
    
}
</script>