using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models
{
    [Table("MeasurementSet_AtomicMeasurement")]
    public class MeasurementSetAtomicMeasurement
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_am")]
        public int AtomicMeasurementId { get; set; }
        [Column("id_ms")]
        public int MeasurementSetId { get; set; }

        public AtomicMeasurement AtomicMeasurement {get; set;}
        public MeasurementSet MeasurementSet { get; set; }
    }
}