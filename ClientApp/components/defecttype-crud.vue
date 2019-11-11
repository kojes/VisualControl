<template>
<v-container>
  <v-layout row>
    <v-flex lg6 offset-lg2>
      <v-card>
       
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

          <v-toolbar-title>Типы дефектов</v-toolbar-title>

         
        </v-toolbar>
        <v-list>
          <v-divider></v-divider>

          <v-list-item v-for="defect in defecttypes"
                       :key="defect.defectId"
                       >
          

            <v-list-item-content>
              <v-list-item-title v-text="defect.description"></v-list-item-title>
            </v-list-item-content>
            <v-list-item-avatar>
              <avatar :username="defect.description"
                      :background-color="defect.color"
                      :size ="30">
              </avatar>
            </v-list-item-avatar>
            <v-list-item-action>
              <v-btn icon ripple @click="openDeleteDialog(defect)">
                <v-icon color="grey">delete</v-icon>
              </v-btn>
            </v-list-item-action>

          </v-list-item>
          <v-divider></v-divider>

        </v-list>
      </v-card>
    </v-flex>
  </v-layout>
  <v-layout row justify-center>
    <v-dialog v-model="dialog" persistent max-width="600px">
      
      <v-card>
        <v-card-title>
          <span class="headline">Добавление нового типа дефекта</span>
        </v-card-title>
        <v-card-text>
          <v-container grid-list-md>
            <v-layout wrap>
              <v-flex lg12>
                <v-text-field v-model="newDefectType" :error-messages="defectTypeErrors" label="Название типа дефекта"></v-text-field>
              </v-flex>
              <v-flex lg9>
                <v-text-field value="Выберите цвет типа дефекта"
                              append-outer-icon="arrow_right_alt"
                              readonly></v-text-field>
              </v-flex>
              <v-flex lg3>
                <verte v-model="colorAdd" picker="wheel" model="hex" menuPosition="center" :recentColors=null :enableAlpha="false" value="#96c15d">
                  <svg xmlns="http://www.w3.org/2000/svg" width="48" height="48" viewBox="0 0 24 24"><path d="M18 4V3c0-.55-.45-1-1-1H5c-.55 0-1 .45-1 1v4c0 .55.45 1 1 1h12c.55 0 1-.45 1-1V6h1v4H9v11c0 .55.45 1 1 1h2c.55 0 1-.45 1-1v-9h8V4h-3z" /><path d="M0 0h48v48H0z" fill="none" /></svg>

                </verte>
              </v-flex>
            </v-layout>
          </v-container>
          </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" text @click="dialog = false">Отмена</v-btn>
          <v-btn color="green" text @click="addDefect">Сохранить</v-btn>
        </v-card-actions>
      
      </v-card>
    </v-dialog>
  </v-layout>
  <v-layout row justify-center>
    <v-dialog v-model="deleteDialog" persistent max-width="600px">

      <v-card>
        <v-card-title>
          <span class="headline">Удаление</span>
        </v-card-title>
        <v-card-text>
          Вы действительно хотите удалить "{{deletingDefectType.description}}"?
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" text @click="deleteDialog = false">Отмена</v-btn>
          <v-btn color="red" text @click="deleteDefectType">Удалить</v-btn>
        </v-card-actions>

      </v-card>
    </v-dialog>
  </v-layout>
  <v-snackbar v-model="snackbarVisibility"
              :color="snackbarColor"
              right
              top>
    {{ snackbarText }}
    <v-btn color="pink"
           text
           @click="snackbarVisibility = false">
      Закрыть
    </v-btn>
  </v-snackbar>
</v-container>
</template>


<script>
  import Verte from 'verte';
  import 'verte/dist/verte.css';
  import Avatar from 'vue-avatar'
  import { required, minLength } from 'vuelidate/lib/validators'
  export default {

    data() {
      return {
        defecttypes: [],
        dialog: false,
        deleteDialog: false,
        deletingDefectType: "",
        colorAdd: "",
        newDefectType: "",
        snackbarVisibility: false,
        snackbarText: "",
        snackbarColor: "success"
      }
    },

    components:
    {
       Verte, Avatar
    },

    mounted()
    {
     
      this.$http.get(`/api/defecttype/getall`).then((response) => {
        this.defecttypes = response.data;
      });

     
    },

    methods:
    {
      openAddDialog()
      {
        this.dialog = true;
        this.colorAdd = "#" + ('00000' + (Math.random() * (1 << 24) | 0).toString(16)).slice(-6);
        
      },

      openDeleteDialog(defectType) {
      
        this.deleteDialog = true;
        this.deletingDefectType = defectType;

      },

      deleteDefectType()
      {
        let defecttype = this.deletingDefectType;
        this.$http.post(`/api/defecttype/deletedefecttype`, defecttype)
          .then((response) => {
            let responseObj = response.data;
            if (response.status === 200) {
              this.snackbarText = `Тип дефекта <<${responseObj.description}>> успешно удален`;
              this.snackbarColor = "teal darken-4";
              this.snackbarVisibility = true;
              this.defecttypes = this.defecttypes.filter(x => x.defectTypeId != defecttype.defectTypeId);
              this.deletingDefectType = "";
            }



          })
          .catch((error) => {

            if (error.response.status === 400) {
              this.snackbarText = error.response.data[0].message;
              this.snackbarColor = "pink darken-4";
              this.snackbarVisibility = true;
              this.deletingDefectType = "";
            }
          });
          this.deleteDialog = false;
      },

      addDefect()
      {
        this.$v.$touch();
        if (this.$v.newDefectType.$error)
        {
          this.dialog = true;
        }
        else
        {
          this.dialog = false;
          var defecttype = {
            description: this.newDefectType,
            color: this.colorAdd
          };

          this.$http.post(`/api/defecttype/addnewdefecttype`, defecttype)
            .then((response) => {
              let responseObj = response.data;
              if (response.status === 200)
              {
                this.snackbarText = `Тип дефекта <<${responseObj.description}>> успешно добавлен`;
                this.snackbarColor = "teal darken-4";
                this.snackbarVisibility = true; 
                this.defecttypes.push(responseObj);
                this.newDefectType = "";
              }

             
               
            })
            .catch((error) => {
              
              if (error.response.status === 400) {
                this.snackbarText = error.response.data[0].message;
                this.snackbarColor = "pink darken-4";
                this.snackbarVisibility = true;
              }
            });
        }
       
       
      }
    },

    computed:
    {


      defectTypeErrors()
      {
        const errors = []
        if (!this.$v.newDefectType.$error) return errors
        !this.$v.newDefectType.required && errors.push('Введите тип дефекта')
        !this.$v.newDefectType.minLength && errors.push('Описание дефекта должно быть более 4 символов')
        return errors
      }
    },

    validations: {
      newDefectType: {
        required,
        minLength: minLength(4)
      }
    }
 }

</script>

<style>
  .verte__menu
  {
     background-color: dimgrey !important
  }
</style>
