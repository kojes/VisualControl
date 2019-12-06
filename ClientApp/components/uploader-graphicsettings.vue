<template>
  <v-container>
      <v-row>
        <v-col lg="6">
            <v-text-field v-model="fileName" :error-messages="validateNewFileName" label="Название файла"></v-text-field>
        </v-col>
         <v-col lg="5" offset-lg="1">
            <v-btn v-show="validateNewFileName.length === 0 && graphics.length > 0" color="success" block @click="createFileName(fileName, processId, graphics)">Создать новый файл</v-btn> 
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
                                                    <v-btn color="indigo" v-show="validateNewGraphic.length === 0" block @click="createGraphic(newGraphic, selectedVariant)">Создать график</v-btn> 
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
                                                <v-btn icon color="pink" @click="deleteGraphic(graphic.name)"><v-icon>delete_outline</v-icon></v-btn>
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
                    <v-btn icon color="pink" @click="deleteVariant"><v-icon>delete_outline</v-icon></v-btn>
                </v-col>
            </v-row> 
           
        </v-col>
      </v-row>
  </v-container>
</template>

<script>
export default {
    props: ["processId", "fileNames"],
    data() {
        return {
            graphics: [],
            fileName: "",
            selectedGraphic: "",
            newGraphic: "",
            menu: false,
            selectedVariant: "Вариант 1",
            variants: ["Вариант 1"]
        }
    },

    computed: {

        validateNewFileName() {
            let fileName = this.fileName
            if(!fileName)
            {
                return "Введите имя файла"
            }
            if(this.fileNames.some(f => f.name === fileName)) {
                return "Такой файл уже существует"
            }
            return []
        },

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

    methods: {
        async createFileName(name, processId, graphics) {
           await this.$http({
                method: "put",
                url: `/api/filegraphicuploader/filename/create`, 
                data: {name: name , processId: processId, graphicNames: graphics.map(g => ({name: g.name, variant: +g.variant.split(' ')[1]}))}, 
                config: {
                    headers: {
                        'Accept': "application/json",
                        'Content-Type': "application/json"
                    }
                }
            })
            .then(response => { 
                this.$emit("show-snackbar", "Новое имя файла успешно создано", "success") 
                this.$emit("file-created", response.data) 
                this.graphics = []
                this.fileName = "" 
                this.selectedVariant = "Вариант 1",
                this.variants =  ["Вариант 1"]            
            })
            .catch(error => this.$emit("show-snackbar", error.response.data[0].message, "pink"));  
        },

        createGraphic(newGraphic, selectedVariant) {
            let graphic = {
                variant: selectedVariant,
                name: newGraphic
            }
            this.graphics.push(graphic)    
            this.menu = false
            this.newGraphic = ""        
        },

        deleteGraphic(name) {
            this.graphics = this.graphics.filter(g => g.name !== name)
        },

        createVariant() {
            let variantsKeys = this.variants.map(v => +v.split(' ')[1]).sort()
            let newVariant = "Вариант " + (_.last(variantsKeys) + 1)
            this.variants.push(newVariant)
            this.selectedVariant = newVariant
        },

        deleteVariant() {
            if(this.variants.length > 1) {  
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