using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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

    }
}
