using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VueExample.Entities
{
    [Table("ShortUrl")]
    public class ShortLinkEntity
    {
        [Key]
        [Column("GeneratedId")]
        public Guid GeneratedId { get; set; }
        [Column("Link")]
        public string Link { get; set; }
        [Column("ShortLink")]
        public string ShortLink { get; set; }
    }
}