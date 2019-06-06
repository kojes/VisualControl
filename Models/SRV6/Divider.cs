using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models.SRV6
{
    [Table("Divider")]
    public class Divider
    {
        [Column("id_divider")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("DividerK")]
        public string DividerK { get; set; }
    }
}