<template>        
    <v-menu v-model="menu" :close-on-content-click="false" :nudge-width="300">
                    <template v-slot:activator="{ on }">
                        <v-btn icon color="primary" v-on="on"><v-icon>add_circle_outline</v-icon></v-btn>
                    </template>
                    <v-card>
                        <v-row>
                            <v-col lg="5" class="pl-8">
                                <v-text-field                                
                                        v-model="name"
                                        readonly
                                        label="Название элемента"
                                ></v-text-field>
                            </v-col>
                            <v-col lg="7" class="px-8">
                                <v-select                                
                                    :items="avElementTypes"
                                    v-model="typeId"
                                    no-data-text="Нет данных"
                                    item-text="name"
                                    item-value="id"
                                    label="Тип элемента"
                                ></v-select>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col lg="12" class="px-8">
                                <v-text-field v-model="comment" label="Описание элемента"></v-text-field>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col lg="6" offset-lg="6" class="pr-8">
                                <v-btn block color="success" @click="createElement(name, dieTypeId)">Создать элемент</v-btn>
                            </v-col>
                        </v-row>
                    </v-card>
                </v-menu>
           
       
       
  
</template>

<script>
export default {
    props: ["name", "dieTypeId"],

    data() {
        return {
            typeId: "",
            comment: "",
            avElementTypes: [],
            menu: false
        }
    },

    methods: {
        async createElement(name, dieTypeId) {
            let createdElement = {}
            await this.$http({
                method: "put",
                url: `/api/element`, 
                data: {name: name, comment: this.comment, typeId: this.typeId}, 
                config: {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                }
            })
            .then(async response => { 
               createdElement = response.data;
               await this.$http({
                    method: "put",
                    url: `/api/element/${createdElement.elementId}/dietype/${dieTypeId}`, 
                    config: {
                        headers: {
                            'Accept': "application/json",
                            'Content-Type': "application/json"
                            }
                        }
                })
                .then(response => {
                    this.$emit("show-snackbar", `Элемент ${createdElement.name} успешно добавлен`, "success")
                    this.$emit("element-created", createdElement)       
                    this.menu = false    
                })                
            })
            .catch(error => this.$emit("show-snackbar", error.response.data[0].message, "pink"))
        },

        async initElementTypes() {
            await this.$http
                .get(`/api/elementtype/all`)
                .then(response => { this.avElementTypes = response.data
                                    this.typeId = this.avElementTypes[0].id})
                .catch(err => console.log(err))
        }
    },

    async mounted() {
        this.initElementTypes()
    }
}
</script>

<style>

</style>