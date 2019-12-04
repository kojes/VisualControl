using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Entities
{
    [Table("FK_MR_Graphics")]
    public class FkMrGraphic
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("id_graphics")]
        public int? GraphicId { get; set; }
        [Column("id_mr")]
        public int? MeasurementRecordingId { get; set; }
        [Column("Comment")]
        public string Comment { get; set; }
        [Column("Operator")]
        public string Operator { get; set; }
        [Column("DateFile")]
        public DateTime DateFile { get; set; }
        [Column("DateImport")]
        public DateTime DateImport { get; set; }
    }
}