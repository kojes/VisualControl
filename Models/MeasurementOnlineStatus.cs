using System;
using System.ComponentModel.DataAnnotations;

namespace VueExample.Models
{
    public class MeasurementOnlineStatus
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime LastTime { get; set;  }
        public bool IsOnline { get; set; }
        public int FullTimeInSeconds { get; set; }
      
    }
}