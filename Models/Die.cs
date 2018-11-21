using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.Models
{
    [Table("Die")]
    public class Die
    {
        [Column("die_id")]
        public long DieId { get; set; }
        [Column("Wafer_id")]
        public string WaferId { get; set; }
        [Column("Code")]
        public string Code { get; set; }
    }
}
