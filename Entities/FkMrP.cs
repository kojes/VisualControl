using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace VueExample.Entities {
    [Table ("FK_MR_P")]
    public class FkMrP {

        [Column ("id_mr_p")]
        public int Id { get; set; }

        [Column ("id_mr")]
        public int? MeasurementRecordingId { get; set; }

        [Column ("id_p")]
        public short? Id247 { get; set; }

        [Column ("Wafer_id")]
        public string WaferId { get; set; }

    }
}