using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models
{
    [Table("Device")]
    public class Device
    {
        [Column("id_device")]
        [Key]
        public int DeviceId { get; set; }
        public string Address { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }

        public IEnumerable<AtomicMeasurement> AtomicMeasurement { get; set; }
    }
}
