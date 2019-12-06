using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models
{
    [Table("GrowWafer")]
    public class Wafer
    {
        [Column("Wafer_id")]
        public string WaferId { get; set; }
        [Column("id_cp")]
        public int? CodeProductId { get; set; }
        
    }
}
