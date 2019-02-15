using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.Models
{
    [Table("Wafer_Map_Parameters")]
    public class WaferMap
    {
        [Column("id_wmp")]
        public int Id { get; set; }
        [Column("Wafer_id")]
        public string WaferId { get; set; }
        [Column("Orientation")]
        public string Orientation { get; set; }
    }
}
