<template>
<v-container fluid grid-list-lg>
 
    <svg :style="svgRotation" :height="fieldHeight" :width="fieldWidth" :viewBox="fieldViewBox">
      
     <polyline fill="none"  stroke="rgba(63, 81, 181, 0.9)" stroke-width="4" stroke-dasharray="25"
                    :points="cutting" />
              
      <g v-for="(die, key, index) in dies" :key="die.id">
        <rect  :dieIndex="key" :x="die.x" :y="die.y" :width="die.width" :height="die.height" :fill="die.fill" @click="showmenu" @contextmenu="showmenu" />
        
      </g>
    
    </svg>
    <v-menu v-model="menu"
            :position-x="x"
            :position-y="y"
            absolute
            offset-y>
      <v-list>
        <v-list-tile v-for="(item, index) in menuItems"
                     :key="index"
                     @click="sendToWaferMapFull">
          <v-list-tile-title>{{ item.title }}</v-list-tile-title>
        </v-list-tile>
      </v-list>
    </v-menu>

  <v-bottom-nav v-model="showNav"
                :active.sync="currentOrientation"
                color="transparent">
    <v-btn :value="0" flat color="rgba(63, 81, 181, 1)">
      0°
      
    </v-btn>

    <v-btn :value="90" flat color="rgba(63, 81, 181, 1)">
      90°

    </v-btn>

    <v-btn :value="180" flat color="rgba(63, 81, 181, 1)">
      180°
     
    </v-btn>

    <v-btn :value="270" flat color="rgba(63, 81, 181, 1)">
      270°
     
    </v-btn>
  </v-bottom-nav>

</v-container>
</template>

<script>
  import Loading from 'vue-loading-overlay';
  export default {
    props: ['waferId', 'defectiveDiesSearchProps', 'streetSize', 'fieldHeight', 'fieldWidth'],
    components: { Loading },
    data() {
      return {
        dies: [],
        isloading: false,
        selectedDieId: 0,
        activeBtn: 1,
        showNav: false,
        x: 0,
        y: 0,
        initialOrientation: 0,
        currentOrientation: -1,
        defectiveDies: [],
        fieldViewBox: "",
        menuItems: [
         { title: "Показать все дефекты на кристалле" }
        ],
        menu: false
        
      
       
      }
    },

    methods:
    {

      sendToWaferMapFull()
      {
         this.$http.get(`/api/defect/getbydieid?dieId=${this.selectedDieId}`)
          .then((response) => {
            let defects = response.data;
            let request = { defects: defects, dieCode: this.dies.find(x => x.id === this.selectedDieId).code }
            this.$emit('show-footer', request);
          })
          .catch((error) => {

          });
         
      },

      showmenu(e) {
        e.preventDefault()
        if (this.dies[+e.currentTarget.attributes.dieIndex.value].isActive)
        {
          this.showMenu = false
          this.x = e.clientX;
          this.y = e.clientY;
          this.selectedDieId = this.dies[+e.currentTarget.attributes.dieIndex.value].id;
          this.$nextTick(() => {
            this.menu = true
          });
        }
       
      }
    },

    
   
    
    watch:
    {
    

      
      waferId: {

       
        immediate: true,
        handler(newVal, oldVal) {
          this.isloading = true;
          let fieldObject = {};
          fieldObject.waferId = this.waferId;
          fieldObject.fieldHeight = this.fieldHeight;
          fieldObject.fieldWidth = this.fieldWidth;
          fieldObject.streetSize = this.streetSize;
          this.$http({
            method: "post",
            url: `/api/wafermap/getformedwafermap`, data: fieldObject, config: {
              headers: {
                'Accept': "application/json",
                'Content-Type': "application/json"
              }
            }
          })
            .then((response) => {
              if (response.status === 200) {
                this.dies = JSON.parse(response.data.waferMapFormed);
                this.initialOrientation = +response.data.orientation;
                this.currentOrientation = this.initialOrientation;
                this.isloading = false;
                this.showNav = true;
              }



            })
            .catch((error) => {

              if (error.response.status === 400) {
                this.dies = [];
                this.isloading = false;
              }
            });



        }
      },


      defectiveDiesSearchProps: function ()
      {
        if (this.defectiveDiesSearchProps.defectType === "all") {
          this.$http.get(`/api/defectivedie/getbydangerlevel?waferId=${this.waferId}&dangerLevelId=${this.defectiveDiesSearchProps.dangerLevel}`)
            .then((response) => {
              this.defectiveDies = response.data;

            })
            .catch((error) => {
              this.defectiveDies = [];
               
            });
        }
        else {
          this.$http.get(`/api/defectivedie/getbydefecttype?waferId=${this.waferId}&defectTypeId=${this.defectiveDiesSearchProps.defectType}&dangerLevelId=${this.defectiveDiesSearchProps.dangerLevel}`)
            .then((response) => {
              this.defectiveDies = response.data;

            })
            .catch((error) => {
              this.defectiveDies = [];

            });
        }
      },

      defectiveDies: function ()
      {
        if (this.defectiveDiesSearchProps.dangerLevel === 1 && this.defectiveDiesSearchProps.defectType === "all")
        {
          this.dies.forEach((die) => { die.fill = "#1E5938"; die.isActive = false });
        }
        else
        {
          this.dies.forEach((die) => { die.fill = "#4f4c4c"; die.isActive = false });
        }
        
        if (this.dies.length > 0 && this.defectiveDies.length > 0)
        {
       
          for (var i = 0; i < this.defectiveDies.length; i++) {
            //Bad smell
            this.dies.find(d => d.id === this.defectiveDies[i].dieId).fill = this.defectiveDies[i].color;
            this.dies.find(d => d.id === this.defectiveDies[i].dieId).isActive = true;
          }
        }
       
      },

      fieldWidth: {
        immediate: true,
        handler(newVal, oldVal) {
          this.fieldViewBox = `0 0 ${this.fieldHeight} ${this.fieldWidth}`;

        }
      }



    },

    computed:
    {
      cutting()
      {
        //ONLY IF HEIGHT === WIDTH
        let bBorder = this.fieldHeight / 6;
        let tBorder = this.fieldHeight / 6 * 5;
        if (this.initialOrientation === null)
        {
          return `0,0 0,0`
        }
        switch (this.initialOrientation) {
          case 0:
            return `${bBorder},${this.fieldHeight} ${tBorder},${this.fieldHeight}`
            break;
          case 90:
            return `0,${bBorder} 0,${tBorder}`;
            break
          case 180:
            return `${bBorder},0 ${tBorder},0`;
            break;
          case 270:
            return `${this.fieldHeight},${bBorder} ${this.fieldHeight},${tBorder}`;
            break;
          default:
            return `0,0 0,0`;
        }

      },

       svgRotation() {
         return {
           transform: `rotate(${this.currentOrientation - this.initialOrientation}deg)`
         
        }
      }
    }
  }
</script>

<style scoped>
  rect:hover {
    stroke: rgba(63, 81, 181, 0.5);
    stroke-width: 3
  }

 
</style>
