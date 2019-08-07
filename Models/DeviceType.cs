using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models
{
    [Table("DeviceType")]
    public class DeviceType
    {
        [Column("Model")]
        public string Model { get; set; }
        [Column("Manufacturer")]
        public string Manufacturer { get; set; }
        [Column("PicturePath")]
        public string PathToPicture { get; set; }
        [Column("PortQuantity")]
        public int PortQuantity { get; set; }
    }
}