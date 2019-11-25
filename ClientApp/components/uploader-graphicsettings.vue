<template>
  <v-container>
      <v-row>
        <v-col lg="6">
            <v-text-field v-model="fileName" label="Название файла"></v-text-field>
        </v-col>
         <v-col lg="5" offset-lg="1">
            <v-btn color="success" block  @click="createFileName()">Создать новый файл</v-btn> 
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
                                                    <v-text-field v-model="newGraphic" label="Название графика"></v-text-field>
                                                </v-col>
                                            </v-row>
                                            <v-row>
                                                <v-col lg="6" offset-lg="5" class="pe-8">
                                                    <v-btn color="indigo" block @click="createGraphic(newGraphic, selectedVariant)">Создать график</v-btn> 
                                                </v-col>
                                            </v-row>
                                        </v-card>
                                    </v-menu>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-list-item-group v-model="selectedGraphic" color="primary">                               
                                    <v-list-item v-for="graphic in graphics" :key="graphic.id">
                                        <!-- <v-list-item-icon>
                                        
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            <v-list-item-title v-text="item.text"></v-list-item-title>
                                        </v-list-item-content> -->
                                    </v-list-item>
                                </v-list-item-group>
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
                    <v-btn color="primary" block outlined @click="addVariant()">Добавить вариант</v-btn> 
                </v-col>
                <v-col lg="2">
                    <v-btn icon color="pink" @click="deleteVariant()"><v-icon>delete_outline</v-icon></v-btn>
                </v-col>
            </v-row> 
           
        </v-col>
      </v-row>
  </v-container>
</template>

<script>
export default {
    props: ["mode", "fileName", "graphics"],
    data() {
        return {
            graphics: this.graphics,
            fileName: this.fileName,
            selectedGraphic: "",
            newGraphic: "",
            menu: false,
            selectedVariant: "Вариант 1",
            variants: ["Вариант 1", "Вариант 2"]
        }
    },

    methods: {
        async createFileName() {
            console.log("created")
        },
        createGraphic(newGraphic, selectedVariant) {
            if(this.mode === "creating") {
                let graphic = {
                    variant: selectedVariant.split(" ")[1],
                    name: newGraphic
                }
                this.graphics.push(graphic)
            }
        }
    },

    async mounted() {

    }

}
</script>

<style>

</style>