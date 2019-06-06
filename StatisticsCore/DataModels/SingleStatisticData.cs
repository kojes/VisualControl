namespace VueExample.StatisticsCore.DataModels
{
    public class SingleStatisticData
    {
        public string ExpectedValue { get; set; }
        public string Maximum { get; set; }
        public string Minimum { get; set; }
        public string StandartDeviation { get; set; }
        public string StatisticsName { get; set; }
        public string Unit { get; set; }
        public string Median { get; set; }
        public int ParameterID { get; set; }
        public string LowBorderStat { get; set; }
        public string TopBorderStat { get; set; }
        public string LowBorderFixed { get; set; }
        public string TopBorderFixed{ get; set; }
        public string AverageFixed { get; set; }
        public DirtyCells DirtyCells{get; set; }

    }
}