using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.Entities
{
    [Table("FK_DIE_PARAMETERS")]
    public class DieParameterOld
    {
        [Column("id_fk_d_p")]
        public long Id { get; set; }
        [Column("id_die")]
        public long? DieId { get; set; }
        [Column("id_p")]
        public int? Id247 { get; set; }
        [Column("Value")]
        public string Value { get; set; }
        [Column("id_mr")]
        public int? MeasurementRecordingId { get; set; }
    }
}