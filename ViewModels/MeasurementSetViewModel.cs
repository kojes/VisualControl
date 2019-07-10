using System;
namespace VueExample.ViewModels
{
    public class MeasurementSetViewModel
    {       
        public Guid MeasurementSetId { get; set; }
        public string Name { get; set; }
        public bool IsGenerated { get; set; }
        public string Route { get; set; }
    }
}