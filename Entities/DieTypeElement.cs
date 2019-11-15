using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VueExample.Models.SRV6;

namespace VueExample.Entities
{
    [Table("DieType_ElementType")]
    public class DieTypeElement
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_et")]
        public int ElementId { get; set; }
        [Column("id_dt")]
        public int DieTypeId { get; set; }
        public Element Element { get; set; }
        public DieType DieType { get; set; }
    }
}