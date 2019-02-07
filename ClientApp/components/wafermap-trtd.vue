<template>
  <svg :height="fieldHeight" :width="fieldWidth" :viewBox="fieldViewBox">
    <g v-for="(die, key, index) in dies" :key="die.id">
      <rect :x="die.x" :y="die.y" :width="die.width" :height="die.height" :fill="die.fill"/>
    </g>
  </svg>
</template>

<script>
  //props: ['diesJSON', 'waferId']
  export default {
    data() {
      return {
        dies: [],
        streetSize: 7,
        fieldHeight: 840,
        fieldWidth: 840,
        waferId: "",
        diesJSON: ""
      }
    },  

    created() {
      this.waferId = "E258";
     
     

    },

    computed:
    {
      fieldViewBox() {
        return `0 0 ${this.fieldHeight} ${this.fieldWidth}`
      }

    },

    watch:
    {

      diesJSON: function ()
      {
        let selectedWafer = this.waferId;
        this.$http.get(`/api/defectivedie/getbydangerlevel?waferId=${selectedWafer}&dangerLevelId=${1}`)
          .then((response) => {
            let corruptedDies = response.data;
            for (var i = 0; i < corruptedDies.length; i++) {
              this.dies.find(d => d.id === corruptedDies[i].dieId).fill = corruptedDies[i].color;
            }
          })
          .catch((error) => {


          });
      },

      
      waferId: function()
      {
        let fieldObject = {};
        fieldObject.waferId = this.waferId;
        fieldObject.fieldHeight = this.fieldHeight;
        fieldObject.fieldWidth = this.fieldWidth;
        fieldObject.streetSize = this.streetSize;

        this.$http({method: "post",
            url: `/api/wafermap/getformedwafermap`, data: fieldObject, config: {
            headers: {
              'Accept': "application/json",
              'Content-Type': "application/json"
            }
          }
        })
          .then((response) => {
            if (response.status === 200) {
              this.dies = response.data;
              this.diesJSON = "1";
            }



          })
          .catch((error) => {

            if (error.response.status === 400) {
              this.dies = [];
            }
          });



      }

     
     
    }
  }
</script>
