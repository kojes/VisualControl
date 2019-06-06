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
        [Column("id_die")]
        public long DieId { get; set; }
        [Column("Wafer_id")]
        public string WaferId { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("x")]
        public short XCoordinate { get; set; }
        [Column("y")]
        public short YCoordinate { get; set; }
        [Column("id_dt")]
        public int? DieTypeId { get; set; }
    }
}
