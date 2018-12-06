<template>
  <div class="container w-100">
    <div class="row">
      <div class="col-12">
        <v-autocomplete v-model="selectedWafer"
                        :items="wafers"
                        :label="`Выберите пластину для просмотра дефектов`"
                        persistent-hint
                        ></v-autocomplete></div>
    </div>
    <div class="row">
      <div class="col-3">
        <v-autocomplete v-model="model"
                        :hint="!isEditing ? 'Click the icon to edit' : 'Click the icon to save'"
                        :items="states"
                        :readonly="!isEditing"
                        :label="`State — ${isEditing ? 'Editable' : 'Readonly'}`"
                        persistent-hint
                        prepend-icon="mdi-city"></v-autocomplete></div>
      <div class="col-3">
        <v-select :items="states"
                  v-model="e7"
                  label="Select"
                  multiple
                  chips
                  hint="What are the target regions"
                  persistent-hint></v-select>
      </div>
      <div class="col-3">
        <v-select :items="states"
                  v-model="e7"
                  label="Select"
                  multiple
                  chips
                  hint="What are the target regions"
                  persistent-hint></v-select>
      </div>
      <div class="col-3">
        <v-select :items="states"
                  v-model="e7"
                  label="Select"
                  multiple
                  chips
                  hint="What are the target regions"
                  persistent-hint></v-select>
      </div>
    </div>
   
    <defect-card :defectId="29"></defect-card>

    <defect-card :defectId="30"></defect-card>

    </div>
       
</template>

<script>
  import DefectCard from './defect-card.vue'
  
  export default {
    components: {
      'defect-card': DefectCard
    },

    created()
    {
      this.$http.get(`/api/wafer/getall`).then((response) => {
        this.wafers = response.data;
      });
    },

    watch:
    {
      selectedWafer: function ()
      {
          let selectedWafer = this.selectedWafer; 
          this.$http.get(`/api/defect/getbywaferid?waferId=${selectedWafer}`).then((response) => {
          this.defects = response.data;
          });
        
      },

      defects: function ()
      {

        let defects = this.defects;
        this.$http.get('/api/defectfilter/GetDefectFilter', {
          params: {
           defectList: defects
          }
        })
          .then(function (response) {
            this.defectFilter = response.data;
          });
      }

    },

    computed: {
      now: function () {
        return Date.now()
      }
    },

    data() {
      return {
        selectedWafer: "",
        wafers: [],
        defects: [],
        defectFilter: {}
      }
    }
  }
</script>
