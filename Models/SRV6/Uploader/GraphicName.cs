using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models.SRV6.Uploader
{
    [Table("UploaderGraphic")]
    public class GraphicName
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
    }
}