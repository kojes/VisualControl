using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models.SRV6
{
    [Table("ElementType")]
    public class Element
    {
        [Column("id")]
        public int ElementId {get; set; }
        [Column("Name")]
        public string Name { get; set; }
        public ICollection<DieTypeElement> DieTypeElements { get; set; }
        public ICollection<MeasurementRecordingElement> MeasurementRecordingElements { get; set; }
    }
}