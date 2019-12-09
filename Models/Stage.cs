using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models
{
    [Table("Stage")]
    public class Stage
    {
        [Key]
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
