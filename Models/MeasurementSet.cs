using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models
{
    [Table("MeasurementSet")]
    public class MeasurementSet
    {
        [Column("id")]
        public int MeasurementSetId { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        public IEnumerable<MeasurementSetAtomicMeasurement> MeasurementSetAtomicMeasurement { get; set; } = new List<MeasurementSetAtomicMeasurement>();
    }
}