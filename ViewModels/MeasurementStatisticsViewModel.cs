using System.Globalization;
using System;
using Newtonsoft.Json;

namespace VueExample.ViewModels
{
    public class MeasurementStatisticsViewModel
    {
        
        public int MeasurementId { get; set; }
        public int GraphicId { get; set; }
        public int DeviceId { get; set; }        
        public int PortNumber { get; set; }
        public string MeasurementName { get; set; }      
        public string GraphicUnit {get; set; }
        public string DeviceName {get; set; }         
        public string Minimum { get; set; }
        public string Maximum { get; set; }       
        [JsonIgnore]
        public string FirstPointValue { get; set; }   
        [JsonIgnore]  
        public string After300Value { get; set; }    

        public string LastValue { get; set; }

        public double CommonDifference
        {
            get => String.IsNullOrEmpty(After300Value) ? Convert.ToDouble(LastValue, CultureInfo.InvariantCulture) - Convert.ToDouble(FirstPointValue, CultureInfo.InvariantCulture) : Convert.ToDouble(LastValue, CultureInfo.InvariantCulture) - Convert.ToDouble(After300Value, CultureInfo.InvariantCulture);         
              
        }
        public double CommonDifferencePercentage
        {
            get => String.IsNullOrEmpty(After300Value) ? (CommonDifference / Convert.ToDouble(FirstPointValue, CultureInfo.InvariantCulture)) * 100 : (CommonDifference / Convert.ToDouble(After300Value, CultureInfo.InvariantCulture)) * 100;         
              
        }
        public string FirstValue
        {
            get => String.IsNullOrEmpty(After300Value) ? FirstPointValue : After300Value;
        }

    }
}