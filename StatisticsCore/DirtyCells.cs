using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace VueExample.StatisticsCore
{
    public class DirtyCells
    {
        public List<long?> StatList { get; set; }
        public List<long?> FixedList { get; set; }
        public string StatPercentage {get; set;}
        public string FixedPercentage {get; set;}

        public DirtyCells()
        {
            StatList = new List<long?>();
            FixedList = new List<long?>();
        }

        public DirtyCells Distinct()
        {
            this.StatList = this.StatList.Distinct().ToList();
            this.FixedList = this.FixedList.Distinct().ToList();
            return this;
        }
        
        public DirtyCells CalculatePercentage(int dieCount)
        {
            this.StatPercentage = Convert.ToString(this.StatList.Count / (dieCount + 0.0), CultureInfo.InvariantCulture);
            this.FixedPercentage = Convert.ToString(this.FixedList.Count / (dieCount + 0.0), CultureInfo.InvariantCulture);
            return this;
        }

    }
}