<template>
  <div class="row mt-4">
    <div class="card w-75 rounded border" style="box-shadow : 2px 0px 9px 0">
      <div class="card-header">
        <h3 class="card-title" style="color:#343a40">{{'Дефект ID: ' + defect.defectId}}</h3>
      </div>
      <div class="container card-body">

        <div class="row">
          <div class="col-4">
            <v-text-field :value="stage.stageName"
                          label="Технологический этап:"
                          color="success"
                          readonly>
            </v-text-field>
          </div>
          <div class="col-4">
            <v-text-field :value="defectType.description"
                          label="Тип дефекта:"
                          readonly>
            </v-text-field>
          </div>
          <div class="col-4">
            <v-text-field :value="dangerLevel.specification"
                          label="Опасность дефекта:"
                          readonly>
            </v-text-field>
          </div>
        </div>
        <div class="row">
          <div class="col-4">
            <v-text-field :value="operator.name"
                          label="Инженер:"
                          readonly>
            </v-text-field>
          </div>
          <div class="col-4">
            <v-text-field :value="date"
                          label="Дата загрузки дефекта:"
                          readonly>
            </v-text-field>
          </div>
        </div>
        <div class="row">
          <div class="col-12 border rounded shadow-sm">
            <h4 style="color:#343a40">Фотографии дефектов</h4>
            <transition-group name="thumbnailfade" tag="div">
              <v-lazy-image v-for="(photo, index) in photos"
                            :key="index"
                            v-on:click.native="showLightbox(photo.name)"
                            :src="photoStorageAddress + photo.name" />
            </transition-group>

            <lightbox id="mylightbox"
                      ref="lightbox"
                      :images="photos"
                      :directory="photoStorageAddress"
                      :timeoutDuration="5000" />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>

  import VLazyImage from "v-lazy-image";
  export default {
    props: ['defectId'],





    mounted() {

     
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
        this.operator = { name: this.defect.operator }




      });

      this.$http.get(`/api/photo/getphotosbydefectid?defectId=${defectId}`).then((response) => {
        this.photos = response.data;
      });




    },

    watch:
    {

      defect: async function () {

        let waferId = this.defect.waferId;
        await this.$http.get(`/api/photo/getphotostorageaddress`).then((response) => {
          this.photoStorageAddress = response.data + waferId + "/";
        });

        
      },

      defectType: function ()
      {
         this.cardStyle.color = this.dangerLevel.color;
      }

    },

    components: {
      VLazyImage
    },

    methods:
    {
      showLightbox: function (imageName) {
        this.$refs.lightbox.show(imageName);
      }
    },

      data() {
        return {
          defect: {},
          stage: {},
          defectType: {},
          dangerLevel: {},
          date: "",
          photoStorageAddress: "",
          operator: {},
          photos: [],
          cardStyle: {
            color: ''
           }
          
        }
      }
  }
</script>

<style>

  img {
    width: 270px;
    height: 180px;
    margin: 20px;
    border-radius: 3px;
    cursor: pointer;
    transition: all 0.4s ease;
  }

  .thumbnailfade-leave-active,
  .thumbnailfade-enter-active {
    transition: all 0.4s ease;
  }

  .thumbnailfade-enter-active {
    transition-delay: 0.4s;
  }

  .thumbnailfade-enter,
  .thumbnailfade-leave-to {
    opacity: 0;
  }

  .v-lazy-image {
    filter: blur(10px);
    transition: filter 0.7s;
  }

  .v-lazy-image-loaded {
    filter: blur(0);
  }
</style>
