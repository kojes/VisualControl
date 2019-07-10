using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.Models
{
    [Table("Point")]
    public class Point
    {
        [Column("id_point")]
        public Int64 PointId { get; set; }
        public int PortNumber { get; set; }
        [Column("id_graphic")]
        public int GraphicId { get; set; }
        [Column("id_device")]
        public int DeviceId { get; set; }
        [Column("id_measurement")]
        public int MeasurementId { get; set; }
        public DateTime Time { get; set; }
        public string Value { get; set; }
        public Measurement Measurement { get; set; }
        
    }
}
