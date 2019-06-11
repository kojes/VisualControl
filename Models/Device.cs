using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.Models
{
    [Table("Device")]
    public class Device
    {
        [Column("id_device")]
        public int DeviceId { get; set; }
        public string Address { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
    }
}
