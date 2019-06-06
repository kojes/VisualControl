using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models.SRV6
{
    [Table("StatParameterForStage")]
    public class StatParameterForStage
    {
        [Column("StatID")]
        public int Id { get; set; }
        [Column("id_stage")]
        public int? StageId { get; set; }
        [Column("id_parameter")]
        public int? StatisticParameterId { get; set; }
        public string MinAverage { get; set; }
        public string MaxAverage { get; set; }
        public string TypeAverage { get; set; }
    }
}