using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Entities
{
    [Table("FK_CodeProduct_StandartWafer")]
    public class CodeProductStandartWafer
    {
        [Column("id_fk")]
        public int Id { get; set; }
        [Column("x")]
        public Nullable<Int16> X { get; set; }
        [Column("y")]
        public Nullable<Int16> Y { get; set; }
        [Column("Flag")]
        public Nullable<Int16> Flag { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("id_cp")]
        public int? CodeProductId { get; set; }
    }
}