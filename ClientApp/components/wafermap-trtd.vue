<template>
  <svg :height="fieldHeight" :width="fieldWidth" :viewBox="fieldViewBox">
    <g v-for="(die, key, index) in dies" :key="die.id">
      <rect :x="die.x" :y="die.y" :width="die.width" :height="die.height" :fill="die.fill"/>
    </g>
  </svg>
</template>

<script>
 
  export default {
    props: ['waferId', 'defectiveDiesSearchProps', 'streetSize', 'fieldHeight', 'fieldWidth'],

    data() {
      return {
        dies: [],
        defectiveDies: [],
        fieldViewBox: ""
                  
       
      }
    },

    
   
    
    watch:
    {
    

      
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
          this.dies.forEach((die) => (die.fill = "#1E5938"));
        }
        else
        {
          this.dies.forEach((die) => (die.fill = "#b6b6b6"));
        }
        
        if (this.dies.length > 0 && this.defectiveDies.length > 0)
        {
       
          for (var i = 0; i < this.defectiveDies.length; i++) {
            
             this.dies.find(d => d.id === this.defectiveDies[i].dieId).fill = this.defectiveDies[i].color;
          }
        }
       
      },

      fieldWidth: {
        immediate: true,
        handler(newVal, oldVal) {
          this.fieldViewBox = `0 0 ${this.fieldHeight} ${this.fieldWidth}`;

        }
      }



    }
  }
</script>
