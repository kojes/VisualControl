namespace VueExample.ChartModels.AmChart.Linear
{
    public class Point
    {
        public string Ctg { get; set; }
        public string Value { get; set; }

        public Point(string category, string value)
        {
            this.Ctg = category;
            this.Value = value;
        }
    }
}