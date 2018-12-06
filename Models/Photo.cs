using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.Models
{
    [Table("Photo")]
    public class Photo
    {
        [Column("id_photo")]
        public int PhotoId { get; set; }
        [Column("guid")]
        public string Guid { get; set; }
        [Column("id_defect")]
        public int DefectId { get; set; }
        
        [NotMapped]
        public string Name
        {
            get { return Guid + ".jpg"; }
        }
               
    }
}
