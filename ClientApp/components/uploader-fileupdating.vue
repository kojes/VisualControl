<template>
    <v-container>
        <v-row>
        <v-col lg="6">
            <v-text-field v-model="fileName.name" readonly label="Название файла"></v-text-field>
        </v-col>
         <v-col lg="5" offset-lg="1">
            <v-btn color="pink" block outlined @click="deleteFileName(fileName, processId)">Удалить файл</v-btn> 
        </v-col>
      </v-row>
      <v-row>      
        <v-col lg="6">
            <v-row>
                <v-col lg="12">
                    <v-card elevation="10">
                        <v-list color="indigo">
                            <v-row>
                                <v-col lg="4" offset-lg="1">
                                    <v-subheader>Графики</v-subheader>                                    
                                </v-col>
                                <v-col lg="6">
                                    <v-menu v-model="menu" :close-on-content-click="false" :nudge-width="300">
                                        <template v-slot:activator="{ on }">
                                            <v-btn v-on="on" color="primary" outlined>Добавить график</v-btn> 
                                        </template>
                                        <v-card>
                                            <v-row>
                                                <v-col lg="11" class="px-8">
                                                    <v-text-field v-model="newGraphic" :error-messages="validateNewGraphic" label="Название графика"></v-text-field>
                                                </v-col>
                                            </v-row>
                                            <v-row>
                                                <v-col lg="6" offset-lg="5" class="pe-8">
                                                    <v-btn color="indigo" v-show="validateNewGraphic.length === 0" block @click="createGraphic(newGraphic, selectedVariant, fileName.id)">Создать график</v-btn> 
                                                </v-col>
                                            </v-row>
                                        </v-card>
                                    </v-menu>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col lg="10" offset-lg="1">
                                    <v-list-item-group v-model="selectedGraphic" color="primary">                               
                                        <v-list-item v-for="graphic in graphics.filter(x => x.variant === selectedVariant)" :key="graphic.name">
                                            <v-list-item-content>
                                                <v-list-item-title>{{graphic.name}}</v-list-item-title>
                                            </v-list-item-content>
                                            <v-list-item-action>
                                                <v-btn icon color="pink" @click="deleteGraphic(graphic, fileName.id)"><v-icon>delete_outline</v-icon></v-btn>
                                            </v-list-item-action>
                                        </v-list-item>
                                    </v-list-item-group>
                                </v-col>
                            </v-row>                         
                        </v-list>
                    </v-card>
                </v-col>
            </v-row>
        </v-col>
        <v-col lg="5" offset-lg="1">
            <v-row>
                <v-col lg="12">
                    <v-select  v-model="selectedVariant"
                        :items="variants"
                        no-data-text="Нет вариантов"
                        label="Выберите вариант:">
                    </v-select>
                </v-col>
            </v-row>
            <v-row>
                <v-col lg="10">
                    <v-btn color="primary" block outlined @click="createVariant">Добавить вариант</v-btn> 
                </v-col>
                <v-col lg="2">
                    <v-btn icon color="pink" @click="deleteVariant(graphics, fileName.id)"><v-icon>delete_outline</v-icon></v-btn>
                </v-col>
            </v-row>            
        </v-col>
      </v-row>
    </v-container>
</template>

<script>
export default {
    props: ["processId", "fileName"],

    data() {
        return {
            graphics: [],
            variants: [],
            newGraphic: "",
            selectedVariant: ""          
        }
    },

     computed: {

        validateNewGraphic() {
            let newGraphic = this.newGraphic
            if(!newGraphic) {
                return "Введите имя графика"
            }
            if(this.graphics.some(x => x.name === newGraphic)) {
                return "Такой график уже существует"
            }
            return []
        }
    },

    watch: {
        'fileName.id': {
            immediate: true,
            async handler(newVal, oldVal){
                await this.$http
                .get(`/api/filegraphicuploader/graphics/${newVal}`)
                .then(response => { 
                    this.graphics = response.data.map(x => ({id: x.id, name: x.name, variant: "Вариант " + x.variant}))
                    this.variants = _.uniq(response.data.map(x => "Вариант " + x.variant))
                    this.selectedVariant = this.variants[0]                    
                            
                })
                .catch(error => {                
                    if(error.response.status === 404) {
                        this.$emit("show-snackbar", `Графики не найдены`, "pink") 
                    }
                })
            }
        }
    },

    methods: {
         async deleteFileName(fileName, processId) {
           await this.$http({
                method: "delete",
                url: `/api/filegraphicuploader/filename`, 
                data: {id: fileName.id, processId: processId}, 
                config: {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                }
            })
            .then(response => { 
                this.$emit("show-snackbar", `Файл ${fileName.name} успешно удален`, "success") 
                this.$emit("file-deleted", fileName.id)          
            })
            .catch(error => this.$emit("show-snackbar", error.response.data[0].message, "pink"));  
        },

        

        async createGraphic(newGraphic, selectedVariant, fileNameId) {
            let graphic = {
                variant: selectedVariant,
                name: newGraphic
            }               
            await this.$http({
                method: "put",
                url: `/api/filegraphicuploader/graphicname/create/${fileNameId}`, 
                data: {...graphic, variant: graphic.variant.split(' ')[1]}, 
                config: {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                }
            })
            .then(response => { 
                this.$emit("show-snackbar", `График ${newGraphic} успешно создан`, "success") 
                this.menu = false
                this.newGraphic = ""
                this.graphics.push({...response.data, variant: selectedVariant})          
            })
            .catch(error => this.$emit("show-snackbar", error.response.data[0].message, "pink"));  

             
        },

        async deleteGraphic(graphic, fileNameId) {
            await this.$http({
                method: "delete",
                url: `/api/filegraphicuploader/graphicname/delete/${fileNameId}`, 
                data: {id: graphic.id, name: graphic.name, variant: graphic.variant.split(' ')[1]}, 
                config: {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                }
            })
            .then(response => { 
                this.$emit("show-snackbar", `График ${graphic.name} успешно удален`, "success") 
                this.graphics = this.graphics.filter(g => g.id !== graphic.id)    
            })
            .catch(error => this.$emit("show-snackbar", error.response.data[0].message, "pink"));  

            
        },

        createVariant() {
            let variantsKeys = this.variants.map(v => +v.split(' ')[1]).sort()
            let newVariant = "Вариант " + (_.last(variantsKeys) + 1)
            this.variants.push(newVariant)
            this.selectedVariant = newVariant
        },
///add api
        deleteVariant(graphics, fileNameId) {
            if(this.variants.length > 1) {  

                var graphicsVariant = graphics.filter(x => x.variant === this.selectedVariant)
                graphicsVariant.forEach(g => {
                    this.deleteGraphic(g, fileNameId)
                })
                this.variants = this.variants.filter(x => x !== this.selectedVariant)
                this.graphics = this.graphics.filter(x => x.variant !== this.selectedVariant)
            }
            this.selectedVariant = this.variants[0]
           
        }
    },

    async mounted() {
       
       
    }
}
</script>

<style>

</style>