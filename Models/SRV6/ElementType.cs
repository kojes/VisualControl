using System.ComponentModel.DataAnnotations.Schema;
namespace VueExample.Models.SRV6
{
    [Table("TypeElement")]
    public class ElementType
    {
        [Column("id_tp")]
        public int Id { get; set; }
        [Column("Name")]
        public int Name { get; set; }
    }
}