using System.ComponentModel.DataAnnotations.Schema;
namespace VueExample.Models.SRV6
{
    [Table("TypeElement1")]
    public class ElementType
    {
        [Column("id_te")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
    }
}