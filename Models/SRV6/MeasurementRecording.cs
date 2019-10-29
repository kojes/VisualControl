using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models.SRV6
{
    [Table("Measurement_Recording")]
    public class MeasurementRecording
    {
        [Column("id_mr")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("id_stage")]
        public int? StageId { get; set; }
        public ICollection<MeasurementRecordingElement> MeasurementRecordingElements { get; set; }
    }
}
