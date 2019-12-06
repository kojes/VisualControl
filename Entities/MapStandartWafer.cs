using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Entities
{
    [Table("FK_Map_StandartWafer")]
    public class MapStandartWafer
    {
        [Key]
        [Column("id_msf")]
        public int Id { get; set; }
        [Column("id_fk")]
        public int? Idfk { get; set; }
        [Column("NewCode")]
        public string NewCode { get; set; }
        [Column("MapName")]
        public string MapName { get; set; }
    }
}