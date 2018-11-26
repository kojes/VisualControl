using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
