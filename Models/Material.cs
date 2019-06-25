using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models
{
    [Table("Material")]
    public class Material
    {
        [Column("id")]
        public int MaterialId { get; set; }
        [Column("Name")]
        public string Name { get; set; }
    }
}