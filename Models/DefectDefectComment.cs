using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VueExample.Models
{
    public class DefectDefectComment
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("id_defect")]
        public int DefectId { get; set; }
        [Column("id_defecttype")]
        public int DefectTypeId { get; set; }
        [JsonIgnore]
        public virtual Defect Defect { get; set; }
        [JsonIgnore]
        public virtual DefectComment DefectComment { get; set; }
    }
}
