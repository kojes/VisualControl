using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.Models
{
    [Table("Measurement")]
    public class Measurement
    {
        [Column("id_measurement")]
        public int MeasurementId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? StopTime { get; set; }

        [Column("IntervalInSeconds")]
        public int? IntervalInSeconds { get; set; }
        
        public string Name { get; set; }

        [Column("id_measureddevice")]
        public int? MeasuredDeviceId { get; set; }

        [Column("id_material")]
        public int? MaterialId { get; set; }
        
        [Column("id_facility")]
        public int FacilityId  { get; set; }

        public IEnumerable<AtomicMeasurement> AtomicMeasurement { get; set; }
        public IEnumerable<Point> Points { get; set; }
        public Material Material { get; set; }


    }
}
