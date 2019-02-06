<template>
  <svg :height="fieldHeight" :width="fieldWidth" :viewBox="fieldViewBox">
    <g v-for="(die, key, index) in w11" :key="die.id">
      <rect :x="die.x" :y="die.y" :width="die.width" :height="die.height" :fill="die.fill"/>
    </g>
  </svg>
</template>

<script>
  //props: ['diesJSON', 'waferId']
  export default {
    data() {
      return {
        w11: [],
        streetSize: 7,
        fieldHeight: 840,
        fieldWidth: 840,
        waferId: ""
      }
    },  

    created()
    {
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
              this.w11 = response.data;
              this.w11.find(file => file.id === 195268).fill = "#006600"
            }



          })
          .catch((error) => {

            if (error.response.status === 400) {
              this.w11 = [];
            }
          });



      }

     
     
    }
  }
</script>
