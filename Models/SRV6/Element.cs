using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models.SRV6
{
    [Table("ElementType")]
    public class Element
    {
        [Key]
        [Column("id")]
        public int ElementId {get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Comment")]
        public string Comment { get; set; }
        [Column("id_et")]
        public int? TypeId { get; set; }
        [Column("PhotoPath")]
        public string PhotoPath { get; set; }
        [Column("DocName")]
        public string DocName { get; set; }
        public ICollection<MeasurementRecordingElement> MeasurementRecordingElements { get; set; }
    }
}