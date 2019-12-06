using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models.SRV6
{
    [Table("BigMeasurementRecording")]
    public class BigMeasurementRecording
    {
        [Key]
        [Column("id_bmr")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("WaferId")]
        public string WaferId { get; set; }
    }
}