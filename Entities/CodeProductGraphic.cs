using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VueExample.Models;

namespace VueExample.Entities
{
    [Table("FK_CodeProduct_Graphics")]
    public class CodeProductGraphic
    {
        [Key]
        [Column("id_fk")]
        public int Id { get; set; }
        [Column("id_cp")]
        public int? CodeProductId { get; set; }
        [Column("id_graphics")]
        public int? GraphicId { get; set; }       
    
        public CodeProduct CodeProduct { get; set; }
        public Models.SRV6.Graphic Graphic { get; set; }
    }
}