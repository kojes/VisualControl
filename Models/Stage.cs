using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.Models
{
    [Table("Stage")]
    public class Stage
    {
        [Column("id_stage")]
        public int StageId { get; set; }
        [Column("StageName")]
        public string StageName { get; set; }
        [Column("id_process")]
        public int ProcessId { get; set; }
        [Column("id_cp")]
        public int? CodeProductId { get; set; }
    }
}
