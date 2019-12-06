using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models.SRV6.Uploader
{
    [Table("UploaderFileName")]
    public class FileName
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("id_process")]
        public int ProcessId { get; set; }
    }
}