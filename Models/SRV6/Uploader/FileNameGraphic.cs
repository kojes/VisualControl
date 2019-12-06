using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models.SRV6.Uploader
{
    [Table("UploaderGraphicFile")]
    public class FileNameGraphic
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_f")]
        public int FileNameId { get; set; }
        [Column("id_g")]
        public int GraphicNameId{ get; set; }
        [Column("Variant")]
        public int Variant { get; set; }
        public FileName FileName { get; set; }
        public GraphicName GraphicName { get; set; }
    }
}