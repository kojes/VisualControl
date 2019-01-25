<template>
  <v-hover>
    <v-card slot-scope="{ hover }" class="card-shadow" :class="`elevation-${hover ? 12 : 0}`">
    
      <lightbox id="mylightbox"
                ref="lightbox"
                :images="photos"
                :directory="photoStorageAddress"
                :timeoutDuration="5000" />




      <v-container grid-list-lg>
        <loading :active.sync="isloading"
                 :can-cancel="false"
                 color="#4b0082"
                 loader="bars"
                 :is-full-page="false">

        </loading>


        <v-layout align-start justify-space-between>
        <span>Кристалл:{{dieCode}}</span>
        <v-tooltip top>

          <v-icon large slot="activator" :color="dangerLevel.color">{{badgeDangerIcon}}</v-icon>

          <span>{{dangerLevel.specification}}</span>
        </v-tooltip>
        </v-layout>

      
        <v-tabs color="indigo"
                dark
                icons-and-text>
      

          <v-tab href="#stage">
            Этап
            <v-icon>table_chart</v-icon>
          </v-tab>

          <v-tab href="#defecttype">
            Тип
            <v-icon>opacity</v-icon>
          </v-tab>

          <v-tab href="#extra">
            Доп.инфо
            <v-icon>watch_later</v-icon>
          </v-tab>
        
          <v-tab-item value="stage">
            <v-card flat>
              <v-layout align-start justify-start row>
                <v-flex lg12>
                  <v-text-field :value="stage.stageName"
                                label="Технологический этап:"
                                readonly>
                  </v-text-field>
                </v-flex>
              </v-layout>
            </v-card>
          </v-tab-item>
          <v-tab-item value="defecttype">
            <v-card flat>
              <v-layout align-start justify-start row>

                <v-flex lg12>
                  <v-text-field :value="defectType.description"
                                label="Тип дефекта:"
                                readonly>
                  </v-text-field>
                </v-flex>

              </v-layout>
            </v-card>
          </v-tab-item>
          <v-tab-item value="extra">
            <v-card flat>
              <v-layout align-start justify-start row>
                <v-flex lg6>
                  <v-text-field :value="operator.name"
                                label="Инженер:"
                                readonly>
                  </v-text-field>
                </v-flex>
                <v-flex lg6>
                  <v-text-field :value="date.split('T')[0]"
                                label="Дата загрузки:"
                                readonly>
                  </v-text-field>
                </v-flex>
              </v-layout>
            </v-card>
          </v-tab-item>

        </v-tabs>





        <v-layout row align-end justify-end>

          <v-flex lg12>
            <v-tabs centered dark
                    color="indigo"
                    show-arrows>
              <v-tabs-slider color="yellow"></v-tabs-slider>

              <v-tab v-for="(photo, index) in photos"
                     :href="'#tab-' + index"
                     :key="index">
                Фото {{ index + 1 }}
              </v-tab>

              <v-tabs-items>
                <v-tab-item v-for="(photo, index) in photos"
                            :value="'tab-' + index"
                            :key="index">
                  <v-flex lg12>
                    <v-card dark>
                      <v-layout align-center justify-center>
                        <v-lazy-image v-on:click.native="showLightbox(photo.name)"
                                      :src="photoStorageAddress + photo.guid + '_MINI.' + photo.name.split('.')[1]" />
                      </v-layout>
                    </v-card>
                  </v-flex>






                </v-tab-item>
              </v-tabs-items>
            </v-tabs>






          </v-flex>

        </v-layout>
        <v-layout row>
          <span>ID:{{defectId}}</span>
        </v-layout>
      </v-container>

    </v-card>
   </v-hover>

</template>

<script>

  import VLazyImage from "v-lazy-image";
  import Loading from 'vue-loading-overlay';
  import 'vue-loading-overlay/dist/vue-loading.css';

  export default {
    props: ['defectId', 'dieCode'],
   
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

    computed:
    {
      badgeDangerIcon()
      {
        switch (this.dangerLevel.danger) {
          case 0:
            return "done"
            break;
          case 1:
            return "error_outline"
            break;
          case 2:
            return "error_outline"
            break;
          default:
            return "error_outline"
        }
      }
    },

    watch:
    {

      defectId: function () {
        this.isloading = true;
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

      defect: async function () {

        let waferId = this.defect.waferId;
        await this.$http.get(`/api/photo/getphotostorageaddress`).then((response) => {
          this.photoStorageAddress = response.data + waferId + "/";
          this.isloading = false;
        });


      }

    },

    

    components: {
      VLazyImage, Loading
    },

    methods:
    {
      showLightbox: function (imageName) {
        this.$refs.lightbox.show(imageName);
      }
    },

      data() {
        return {
          isloading: true,
          defect: {},
          stage: {},
          defectType: {},
          dangerLevel: {},
          date: "",
          photoStorageAddress: "",
          operator: {},
          photos: []
         
          
        }
      }
  }
</script>

<style>

  img {
    width: 320px;
    height: 180px;
    margin: 20px;
    border-radius: 3px;
    cursor: pointer;
    transition: all 0.4s ease;
  }

  .card-shadow {
    --box-shadow-color: palegoldenrod;
    box-shadow: 1px 2px 3px var(--box-shadow-color);
  }

  .v-lazy-image {
    filter: blur(10px);
    transition: filter 0.7s;
  }

  .v-lazy-image-loaded {
    filter: blur(0);
  }
</style>
