using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models.SRV6 
{
    [Table ("CodeProduct_DieType")]
    public class DieTypeCodeProduct 
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_cp")]
        public int CodeProductId { get; set; }
        [Column("id_dt")]
        public int DieTypeId { get; set; }
        public CodeProduct CodeProduct { get; set; }
        public DieType DieType { get; set; }
    }
}