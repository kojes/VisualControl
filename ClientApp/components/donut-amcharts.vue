<template>
  <div class="donut" ref="chartdiv">
  </div>
</template>

<script>
  import * as am4core from "@amcharts/amcharts4/core";
  import * as am4charts from "@amcharts/amcharts4/charts";
  import am4themes_animated from "@amcharts/amcharts4/themes/animated";
  am4core.useTheme(am4themes_animated);

  export default {
    props: ['routeApi', 'parametersApi', 'waferId'],
    data() {
      return {
        chart: ""
      }
    },
    mounted() {
      let chart = am4core.create(this.$refs.chartdiv, am4charts.PieChart);
      chart.hiddenState.properties.opacity = 0; 
      chart.legend = new am4charts.Legend();
      chart.innerRadius = am4core.percent(40);
      var series = chart.series.push(new am4charts.PieSeries());
      series.dataFields.value = "v";
      series.dataFields.category = "ctg";      
      series.legendSettings.labelText = "[#fff]{category}[/]";
      series.legendSettings.valueText = "[bold #fff]{value}[/]";
      series.slices.template.stroke = am4core.color("#fc0");
      series.slices.template.strokeWidth = 2;
      series.slices.template.strokeOpacity = 1;
      series.slices.template.configField = "amChartConfig";
      series.ticks.template.stroke = am4core.color("#fc0")
      series.ticks.template.strokeWidth = 2;
      series.labels.template.fill = am4core.color("#fff");
     
      this.chart = chart;
    },

    watch:
    {
      waferId()
      {
        this.parametersApi["waferId"] = this.waferId;
        let query = "?";
        for (var key in this.parametersApi) {
          query = query + key + "=" + this.parametersApi[key] + "&";
        }

        if (this.waferId)
        {
          this.$http.get(`${this.routeApi}${query.slice(0, -1)}`).then((response) => {
            let chartProps = response.data;
            this.chart.data = chartProps.data;
            this.chart.title = chartProps.title;
          });
        }
       

      }
    },


    beforeDestroy() {
      if (this.chart) {
        this.chart.dispose();
      }
    }

  }

</script>

<style>
  .donut {
   
    height: 250px;
  }
</style>
