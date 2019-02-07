<template>
  <svg :height="fieldHeight" :width="fieldWidth" :viewBox="fieldViewBox">
    <g v-for="(die, key, index) in dies" :key="die.id">
      <rect :x="die.x" :y="die.y" :width="die.width" :height="die.height" :fill="die.fill"/>
    </g>
  </svg>
</template>

<script>
 
  export default {
    props: ['defectedDies', 'waferId', 'streetSize', 'fieldHeight', 'fieldWidth'],

    data() {
      return {
        dies: [],
        fieldViewBox: "",
        
      
       
       
      }
    },  

   
    computed:
    {
      

    },

    watch:
    {

      defectedDies: function ()
      {
        
        for (var i = 0; i < this.defectedDies.length; i++) {
          this.dies.find(d => d.id === this.defectedDies[i].dieId).fill = this.defectedDies[i].color;
        }
      },

      fieldWidth: {
        immediate: true,
        handler(newVal, oldVal) {
          this.fieldViewBox = `0 0 ${this.fieldHeight} ${this.fieldWidth}`;

        }
      },

      
      waferId: {


        immediate: true,
        handler(newVal, oldVal) {

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
                this.dies = response.data;

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
  }
</script>
