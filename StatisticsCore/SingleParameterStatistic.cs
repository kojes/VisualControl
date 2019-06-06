using System.Linq;
using System.Collections.Generic;
using System;
using VueExample.Models.SRV6;
using System.Globalization;

namespace VueExample.StatisticsCore
{
    public class SingleParameterStatistic
    {
        public string Name { get; set; }
        public List<long?> dieList {get; }
        public List<double> valueList {get; }
        public string LowBorderStat { get; set; }
        public string TopBorderStat { get; set; }
        public double LowBorderFixed { get; set; }
        public double TopBorderFixed{ get; set; }
        public double AverageFixed { get; set; }
        public bool IsHasFixed { get; set; } = false;
        public DirtyCells DirtyCells{get; set; } 

        public SingleParameterStatistic(string name, List<long?> dieList, List<double> valueList)
        {
             this.dieList = new List<long?>(dieList);
             this.Name = name;
             this.valueList = new List<double>(valueList);
             this.DirtyCells = new DirtyCells();
             CalculateDirtyCellsStat();
            
             
        }

        public SingleParameterStatistic(string name, List<long?> dieList, List<double> valueList, DirtyCells originDirtyCells)
        {
             this.dieList = new List<long?>(dieList);
             this.Name = name;
             this.valueList = new List<double>(valueList);
             this.DirtyCells = new DirtyCells();
             this.DirtyCells.StatList = originDirtyCells.StatList.Intersect(dieList).ToList();
             this.DirtyCells.FixedList = originDirtyCells.FixedList.Intersect(dieList).ToList();
             
        }
    

        private void CalculateDirtyCellsStat()
        {
            var dds = new DataDescriptiveStatistics(valueList);
            for (int i = 0; i < valueList.Count; i++)
            {
                if((dds.Quartile3Double + 1.5*dds.IQRDouble < valueList[i] || dds.Quartile1Double - dds.IQRDouble*1.5 > valueList[i] || double.IsNaN(valueList[i])))
                {
                     this.DirtyCells.StatList.Add(dieList[i]);
                }
            }

            this.LowBorderStat = GetFormat(dds.Quartile1Double - dds.IQRDouble * 1.5);
            this.TopBorderStat = GetFormat(dds.Quartile3Double + 1.5 * dds.IQRDouble);


            
        }

        public SingleParameterStatistic CalculateDirtyCellsFixed(StatParameterForStage statParameterForStage)
        {
            if(statParameterForStage != null)
            {
                this.LowBorderFixed = !String.IsNullOrEmpty(statParameterForStage.MinAverage) ? double.Parse(statParameterForStage.MinAverage, CultureInfo.InvariantCulture) : 1E-23;
                this.TopBorderFixed = !String.IsNullOrEmpty(statParameterForStage.MaxAverage) ? double.Parse(statParameterForStage.MaxAverage, CultureInfo.InvariantCulture) : 1E23;
                CalculateFixedList();
            }

            return this;
            
        }

        private void CalculateFixedList()
        {
            for (int i = 0; i < valueList.Count; i++)
            {
                if(this.LowBorderFixed > valueList[i] || this.TopBorderFixed < valueList[i] || double.IsNaN(valueList[i]))
                {
                     this.DirtyCells.FixedList.Add(dieList[i]);
                }
            }
            IsHasFixed = true;
        }

        private string GetFormat(double number)
        {
            if (Math.Abs(number) < 1E-22 || Math.Abs(number) > 1E22)
            {
                return String.Empty;
            }
            if ((Math.Abs(number) >= 10000 || Math.Abs(number) < 1E-2) && Math.Abs(number - 0) > 1E-20)
            {
                return number.ToString("0.00E0");
            }
            return number.ToString("0.000");
        }
    }
}