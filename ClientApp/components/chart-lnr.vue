<template>
<v-card>
     
          <v-container  fluid>
              <v-layout class="charts" ref="chartdiv">
              </v-layout>
              
          </v-container>
   
    </v-card>
</template>

<script>
 import * as am4core from "@amcharts/amcharts4/core";
 import * as am4charts from "@amcharts/amcharts4/charts";
 import am4themes_animated from "@amcharts/amcharts4/themes/animated";


 export default {

    props: ["keyGraphicState", "measurementId", "divider"],
    data() {
    return {
       chartData: {}
    };
  },

    mounted()
    {
       

    },

    beforeDestroy() {
      if (this.chart) {
        this.chart.dispose();
      }
      },

    computed:
    {
        selectedDies()
        {
            return this.$store.state.wafermeas.selectedDies;
        }
    },

    watch:
    {
        selectedDies: function()
        {
            this.getChartData();
        },

        divider: function()
        {
            this.getChartData();
        }
    },

    methods:
    {
         getChartData()
         {
                var singlestatModel = {};
                singlestatModel.divider = this.divider;
                singlestatModel.keyGraphicState = this.keyGraphicState;
                singlestatModel.measurementId = this.measurementId;
                singlestatModel.dieIdList = this.selectedDies;
                this.$http
                    .get(`api/amchart/GetLinearForMeasurement?statisticSingleGraphicViewModelJSON=${JSON.stringify(singlestatModel)}`)
                    .then(response => {
                    this.chartData = response.data;
                  
                    let chart = am4core.create(this.$refs.chartdiv, am4charts.XYChart);
                   
                    let categoryAxis = this.chart.xAxes.push(new am4charts.ValueAxis());
                   
                    let valueAxis = this.chart.yAxes.push(new am4charts.ValueAxis());     
                
                    for (let j = 0; j < this.chartData.data.length; j++) {
             
                        let data = [];
                        for (let i = 0; i < this.chartData.data[j].pointList.length; i++)
                        {
                            data.push({category: this.chartData.data[j].pointList[i].ctg, value: this.chartData.data[j].pointList[i].value});
                            
                        }
                        let series = this.chart.series.push(new am4charts.LineSeries());
                        series.data = data;
                        series.dataFields.valueX = "category";
                        series.dataFields.valueY = "value";
                        series.strokeWidth = 2;
                               
            
                    }
                    this.chart = chart;
                  
                })
                .catch(error => {});
         }
    }
   
 }
</script>

<style scoped>
  .charts {
    width: 90%;
    height: 90%;
  }
</style>
