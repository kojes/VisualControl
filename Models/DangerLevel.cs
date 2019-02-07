using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace VueExample.Models
{
    [Table("Danger")]
    public class DangerLevel
    {
        [Column("id_dangerlevel")]
        public int DangerLevelId { get; set; }
        [Column("DangerLevelInteger")]
        public int? Danger { get; set; }
        [Column("Specification")]
        public string Specification { get; set; }
        public string Color { get; set; }
        [JsonIgnore]
        public ICollection<Defect> Defects { get; set; }
    }
}
