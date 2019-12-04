using System;
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
        [Column("Date_measurement")]
        public DateTime MeasurementDateTime { get; set; }
        [Column("Type")]      
        public int Type { get; set; }
        [Column("id_stage")]
        public int? StageId { get; set; }
        [Column("id_bmr")]
        public int? BigMeasurementRecordingId { get; set; }
        public ICollection<MeasurementRecordingElement> MeasurementRecordingElements { get; set; }
    }
}
