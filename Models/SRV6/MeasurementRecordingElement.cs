using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models.SRV6
{
    [Table("FK_MR_EL")]
    public class MeasurementRecordingElement
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_mr")]
        public int MeasurementRecordingId { get; set; }
        [Column("id_el")]
        public int ElementId { get; set; }
        public MeasurementRecording MeasurementRecording { get; set; }
        public Element Element { get; set; }
    }
}