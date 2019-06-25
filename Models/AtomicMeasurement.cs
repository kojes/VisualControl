using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models
{
    [Table("AtomicMeasurement")]
    public class AtomicMeasurement
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("id_measurement")]
        public int MeasurementId { get; set; }
        [Column("id_graphic")]
        public int GraphicId { get; set; }
        [Column("id_device")]
        public int DeviceId { get; set; }
        [Column("Port")]
        public int PortNumber { get; set; }

        public IEnumerable<MeasurementSetAtomicMeasurement> MeasurementSetAtomicMeasurement { get; set; } = new List<MeasurementSetAtomicMeasurement>();
        public Measurement Measurement { get; set; }
        public Device Device { get; set; }
        public Graphic Graphic { get; set; }
    }
}