using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.Models
{
    [Table("DefectType")]
    public class DefectType
    {
        [Column("id_defecttype")]
        public int DefectTypeId { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
    }
}
