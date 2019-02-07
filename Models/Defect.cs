using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace VueExample.Models
{
    [Table("Defect")]
    public class Defect
    {
        [Column("id_defect")]
        public int DefectId { get; set; }
        [Column("id_die")]
        public long DieId { get; set; }
        [Column("id_stage")]
        public int StageId { get; set; }

        
        [Column("id_defecttype")]
        public int DefectTypeId { get; set; }
        
        [Column("id_dangerlevel")]
        public int DangerLevelId { get; set; }

        public string Operator { get; set; }
        public DateTime Date { get; set; }
        [Column("wafer_id")]
        public string WaferId { get; set; }

        [JsonIgnore]
        public virtual ICollection<DefectDefectComment> DefectDefectComments { get; set; }
        [JsonIgnore]
        public DefectType DefectType { get; set; }
        [JsonIgnore]
        public DangerLevel DangerLevel { get; set; }

    }
}
