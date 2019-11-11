<template>
    <v-container>
        <v-layout row>
            <v-flex lg3 offset-lg1>
                 <v-text-field v-model="dieTypeName" :error-messages="dieTypeName ? [] : 'Введите название монитора'" 
                                    label="Введите название монитора"
                 ></v-text-field>
            </v-flex>
            <v-flex lg6>
                <v-card>
                    <v-card-title>
                        <v-select v-model="selectedProcess"
                            :items="processes"
                            no-data-text="Нет данных"
                            item-text="processName"
                            item-value="processId"
                            label="Выберите техпроцесс"
                        ></v-select>
                    </v-card-title>
                    <v-card-text>
                        <v-list
                        subheader
                        two-line
                        >
                            <v-subheader>Шаблоны</v-subheader>
                            <v-list-item v-for="cp in avCodeProducts" :key="cp.id">
                                <v-list-item-action>
                                    <v-checkbox></v-checkbox>
                                </v-list-item-action>
                                <v-list-item-content>
                                    <v-list-item-title>{{cp.name}}</v-list-item-title>
                                </v-list-item-content>
                            </v-list-item>                        
                        </v-list>
                    </v-card-text>
                </v-card>
            </v-flex>
        </v-layout>        
    </v-container>
</template>
<script>
export default {
    data() {
        return {
            dieTypeName: "",
            selectedProcess: "",
            processes: [],
            avCodeProducts: [],
            selectedCodeProducts: []

        }
    },

    methods: {
        async getProcesses() {
            await this.$http
            .get(`/api/process/all`)
            .then(response =>  this.processes = response.data)
            .catch(err => console.log(err)); ///err
        },

        async getCodeProductsByDieTypeId() {

        }
    },

    watch: {
        selectedProcess: async function(newVal, oldVal) {
            await this.$http
            .get(`/api/codeproduct/processid/${newVal}`)
            .then(response => this.avCodeProducts = response.data)
            .catch(err => console.log(err)) ///err
        }
    },

    async mounted() {
        this.getProcesses()
    }
}
</script>