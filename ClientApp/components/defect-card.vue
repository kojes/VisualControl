<template>
  <div class="container">
    <div class="row">
      <div class="col-3">
        <v-text-field :value="stage.stageName"
                      label="Технологический этап:"
                      color="success"
                      readonly>

        </v-text-field>
      </div>
      <div class="col-3">
        <v-text-field :value="defectType.description"
                      label="Тип дефекта:"
                      readonly>
        </v-text-field>
      </div>
      <div class="col-3">
        <v-text-field :value="dangerLevel.specification"
                      label="Опасность дефекта:"
                      readonly>
        </v-text-field>
      </div>
    </div>
    <div class="row">
      <div class="col-3">
        <v-text-field :value="operator.name"
                      label="Инженер:"
                      readonly>
        </v-text-field>
      </div>
      <div class="col-3">
        <v-text-field :value="date"
                      label="Дата загрузки дефекта:"
                      readonly>
        </v-text-field>
      </div>
    </div>
    <div class="row">
      <div class="col-9">
        <v-carousel cycle="false" height="undefined" interval="99999999" hide-delimiters="true">
          <v-carousel-item v-for="(item,i) in items"
                           :key="i"
                           :src="item.src"></v-carousel-item>
        </v-carousel>
      </div>
    </div>
   </div>
</template>

<script>
 

  export default {
    props: ['defectId'],

   



     mounted() {

        this.defectId = 19;
        let defectId = this.defectId;
          this.$http.get(`/api/defect/getbyid?defectId=${defectId}`).then((response) => {
          this.defect = response.data;
          let stageId = this.defect.stageId;

          this.$http.get(`/api/stage/getbyid?stageId=${stageId}`).then((response) => {
            this.stage = response.data;
          });
          let dangerLevelId = this.defect.dangerLevelId;
            this.$http.get(`/api/dangerlevel/getbyid?dangerlevelId=${dangerLevelId}`).then((response) => {
            this.dangerLevel = response.data;
          });

          let defectTypeId = this.defect.defectTypeId;
            this.$http.get(`/api/defecttype/getbyid?defecttypeId=${defectTypeId}`).then((response) => {
            this.defectType = response.data;
            });

            this.date = this.defect.date;
            this.operator = {name: this.defect.operator}
           
        });
        

           

      

    },
    data() {
      return {
        defect: {},
        stage: {},
        defectType: {},
        dangerLevel: {},
        date: "",
        operator: {},
        photos: [],
        items: [
          {
            src: 'http://192.168.11.10/photostorage/096_1/3b473e8e1e734140b513a928106deea9.jpg'
          },
          {
            src: 'https://cdn.vuetifyjs.com/images/carousel/sky.jpg'
          },
          {
            src: 'https://cdn.vuetifyjs.com/images/carousel/bird.jpg'
          },
          {
            src: 'https://cdn.vuetifyjs.com/images/carousel/planet.jpg'
          }
        ]
              
      }
    }
  }
</script>
