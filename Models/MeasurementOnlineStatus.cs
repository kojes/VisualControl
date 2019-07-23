using System;
namespace VueExample.Models
{
    public class MeasurementOnlineStatus
    {
        public DateTime StartTime { get;  }
        public DateTime LastTime { get;  }
        public bool IsOnline { get; }
        public double FullTimeInSeconds { get; }

        public MeasurementOnlineStatus(int? measurementInterval, int pointsCount, DateTime StartTime, DateTime LastTime)
        {
            this.StartTime = StartTime;
            this.LastTime = LastTime;
            this.FullTimeInSeconds =  measurementInterval.HasValue ? pointsCount * Convert.ToInt32(measurementInterval) : 0.0;            
            this.IsOnline = (DateTime.Now.AddMinutes(-5.0) - LastTime).TotalSeconds < 2 * measurementInterval;
          
        }
    }
}