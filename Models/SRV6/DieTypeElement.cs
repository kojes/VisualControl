namespace VueExample.Models.SRV6
{
    public class DieTypeElement
    {
        public int DieTypeId { get; set; }
        public int ElementId { get; set; }
        public DieType DieType { get; set; }
        public Element Element { get; set; }
    }
}