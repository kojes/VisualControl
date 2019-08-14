<template>
  <div class="hello" :style="{ backgroundColor: settings.colors.backgroundColor}" ref="chartdiv">
  </div>
</template>

<script>
  import * as am4core from "@amcharts/amcharts4/core";
  import * as am4charts from "@amcharts/amcharts4/charts";
  import am4themes_animated from "@amcharts/amcharts4/themes/animated";
  import smooth from 'array-smooth'
  
  export default {
     
    props: ['points', 'graphic', 'devices', 'settings'],

   
    mounted() {
      let chart = am4core.create(this.$refs.chartdiv, am4charts.XYChart);
      this.tempPoints = JSON.parse(JSON.stringify(this.points));
      var gridColor = this.settings.colors.gridColor;
      var textColor = this.settings.colors.textColor;
      function am4themes_myTheme(target) {
      if (target instanceof am4core.InterfaceColorSet) {
        target.setFor("grid", am4core.color(gridColor));
        target.setFor("text", am4core.color(textColor));
       
      }

      if (target instanceof am4core.ColorSet) {
    target.list = [
      am4core.color("#E64A39"),
      am4core.color("#E97439"),
      am4core.color("#EDD157"),
      am4core.color("#65ED99"),
      am4core.color("#ce978d"),
      am4core.color("#bbbb9a"),
      am4core.color("#C15D85"),
      am4core.color("#94D869")
    ];
  }
    }

    am4core.useTheme(am4themes_myTheme);

     
      let dateAxis = chart.xAxes.push(new am4charts.DurationAxis());
      dateAxis.baseUnit = "millisecond";
      dateAxis.title.text = "Время испытаний (чч:мм:cc)";
      

      chart.cursor = new am4charts.XYCursor();

      chart.cursor.behavior = "zoomXY";

      chart.durationFormatter.durationFormat = "hh':'mm':'ss";


      let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
      valueAxis.title.text = this.graphic.russianName;  
     
      for (var prop in this.points) {

        let data = [];
        let getter = (item) => item.value;
        let setter = (item, itemSmoothed) => ({ duration: item.duration, value: itemSmoothed })
        let durationSpaces = [];
        
        var entryDatepoint = Date.parse(this.points[prop].pointsList[0].time);
        var max = 0;
        var adequateSpace = Date.parse(this.points[prop].pointsList[1].time) - Date.parse(this.points[prop].pointsList[0].time);
        for (let i = 0; i < this.points[prop].pointsList.length; i++) {

           data.push({ "duration": Date.parse(this.points[prop].pointsList[i].time) - entryDatepoint, "value": +this.points[prop].pointsList[i].value});        
          
          //max = Date.parse(this.points[prop].pointsList[this.points[prop].pointsList.length - 1].time) - entryDatepoint;
        }
        let series = chart.series.push(new am4charts.LineSeries());
      
        if(this.settings.smoothing.require)
        { 
           series.data = smooth(data, this.settings.smoothing.power, getter, setter);
        }
        else
        {
           series.data = data;
        }

        
        series.dataFields.valueX = "duration";
        series.dataFields.valueY = "value";
        series.strokeWidth = 2;
        series.tensionX = 0.7;
        series.tensionY = 0.7;
        let device = this.devices.find(x => x.deviceId === this.points[prop].pointsList[0].deviceId);
        series.name =  "Измерение: " + this.points[prop].measurementName + " Устройство: " + device.name + " Порт: " + this.points[prop].pointsList[0].portNumber;
      }

      chart.legend = new am4charts.Legend();
      chart.legend.useDefaultMarker = true;
      var markerTemplate = chart.legend.markers.template;
      markerTemplate.width = 40;
      markerTemplate.height = 40;

      valueAxis.strictMinMax = false;      
      if(this.settings.axisY.strictMinMax)
      {
          valueAxis.strictMinMax = true;
          valueAxis.min = this.settings.axisY.min;
          valueAxis.max = this.settings.axisY.max;
      }
       
      dateAxis.strictMinMax = true;
      dateAxis.min = 0;
     

      chart.colors.step = 2;
      this.chart = chart;
     
    },

    watch: 
    {
       'settings.colors': function()
       {
          var gridColor = this.settings.colors.gridColor;
          var textColor = this.settings.colors.textColor;
       }
    },

    beforeDestroy() {
      if (this.chart) {
        this.chart.dispose();
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
  .hello {
    width: 90%;
    height: 700px;
}
</style>
