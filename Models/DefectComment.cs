using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VueExample.Models
{
    [Table("DefectComment")]
    public class DefectComment
    {
        [Column("id_defectcomment")]
        public int DefectCommentId { get; set; }
        public string Message { get; set; }
        public string OriginalPoster { get; set; }
        [JsonIgnore]
        public virtual ICollection<DefectDefectComment> DefectDefectComments { get; set; }
    }
}
