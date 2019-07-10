using System;
namespace VueExample.Models
{
    public class MeasurementOnlineStatus
    {
        public DateTime StartTime { get;  }
        public DateTime LastTime { get;  }
        public bool IsOnline { get; }
        public double FullTimeInSeconds { get; }

        public MeasurementOnlineStatus(int? measurementInterval, DateTime StartTime, DateTime LastTime)
        {
            this.StartTime = StartTime;
            this.LastTime = LastTime;
            this.FullTimeInSeconds = (LastTime - StartTime).TotalSeconds;
            this.IsOnline = (DateTime.Now.AddHours(-1.0) - LastTime).TotalSeconds < 2 * measurementInterval;
          
        }
    }
}