using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models
{
    [Table("Graphic")]
    
    public class Graphic
    {
        [Column("id_graphic")]
        public int GraphicId { get; set; }
        public string Specification { get; set; }
        public string Unit { get; set; }
        public string RussianName { get; set; }
        public string Type { get; set; }
        public IEnumerable<AtomicMeasurement> AtomicMeasurement { get; set; }
    }
}
