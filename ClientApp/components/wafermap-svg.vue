<template>
<v-container fluid grid-list-lg>
  
    <svg :style="svgRotation" :height="fieldHeight" :width="fieldWidth" :viewBox="fieldViewBox">
      
     <polyline fill="none"  stroke="#fc0" stroke-width="4" stroke-dasharray="25"
                    :points="cutting" />
              
      <g v-for="(die, key) in dies" :key="die.id">
        <rect  :dieIndex="key" :x="die.x" :y="die.y" :width="die.width" :height="die.height" :fill="die.fill" @click="selectDie" @contextmenu="showmenu" />
        
      </g>
    
    </svg>
    <v-menu v-model="menu"
            :position-x="x"
            :position-y="y"
            absolute
            offset-y>
      <v-list>
        <v-list-tile v-for="(item, index) in menuItems"
                     :key="index">
                    
          <v-list-tile-title>{{ item.title }}</v-list-tile-title>
        </v-list-tile>
      </v-list>
    </v-menu>

  <!-- <v-bottom-nav :value="showNav"
                :active.sync="currentOrientation"
                color="transparent">
    <v-btn :value="0" flat color="#fc0">
      0°
      
    </v-btn>

    <v-btn :value="90" flat color="#fc0">
      90°

    </v-btn>

    <v-btn :value="180" flat color="#fc0">
      180°
     
    </v-btn>

    <v-btn :value="270" flat color="#fc0">
      270°
     
    </v-btn>
    
  </v-bottom-nav> -->

</v-container>
</template>

<script>
  import Loading from 'vue-loading-overlay';
  export default {
    props: ['waferId', 'avbSelectedDies', 'streetSize', 'fieldHeight', 'fieldWidth'],
    components: { Loading },
    data() {
      return {
        dies: [],
        isloading: false,
        activeBtn: 1,
        showNav: false,
        x: 0,
        y: 0,
        initialOrientation: -1,
        currentOrientation: -1,
        fieldViewBox: "",
        menuItems: [
         { title: "Mocking" }
        ],
        menu: false
        
      
       
      }
    },

    methods:
    {
      
      selectDie(e)
      {
        e.preventDefault()
        var dieId = this.dies[+e.currentTarget.attributes.dieIndex.value].id;
      
        if (this.dies[+e.currentTarget.attributes.dieIndex.value].isActive)
        {
            
             let position =  this.selectedDies.indexOf(dieId);
             if ( ~position ) 
             {
                 this.selectedDies.splice(position, 1);
                 this.$store.commit("wafermeas/updateSelectedDies", this.selectedDies);
             }
             else
             {
                 this.selectedDies.push(dieId);
                 this.$store.commit("wafermeas/updateSelectedDies", this.selectedDies);
             }
        }
      
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
                this.showNav = false;
                this.dies.forEach(function(cell) {
                    cell.fill = "#A1887F";
                    cell.isActive = false;
                });
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

    
    

      fieldWidth: {
        immediate: true,
        handler(newVal, oldVal) {
          this.fieldViewBox = `0 0 ${this.fieldHeight} ${this.fieldWidth}`;
        }
      },


      selectedDies: function()
      {
        this.dies.forEach(function(cell) {
                    cell.fill = "#A1887F";
                    cell.isActive = false;
                });
        if (this.avbSelectedDies.length > 0 && this.selectedDies.length > 0)
        {

          for (var i = 0; i < this.avbSelectedDies.length; i++) 
          {
            //Bad smell
            this.dies.find(d => d.id === this.avbSelectedDies[i]).fill = "#8C9EFF";
            this.dies.find(d => d.id === this.avbSelectedDies[i]).isActive = true;
          }  
       
          for (var i = 0; i < this.selectedDies.length; i++) 
          {
            
            this.dies.find(d => d.id === this.selectedDies[i]).fill = "#3D5AFE";
           
          }    
          
         
        }

       }
    },

    computed:
    {
      selectedDies()
      {
            return this.$store.state.wafermeas.selectedDies
      },
      cutting()
      {
        //ONLY IF HEIGHT === WIDTH
        let bBorder = this.fieldHeight / 6;
        let tBorder = this.fieldHeight / 6 * 5;
        if (this.initialOrientation === -1)
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
    stroke: #fc0;
    stroke-width: 3;
    stroke-opacity: 0.6
  }

 
</style>
