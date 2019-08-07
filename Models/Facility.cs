using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models
{
    [Table("Facility")]
    public class Facility
    {
        [Column("id_facility")]
        public int FacilityId { get; set; }
        public string Name { get; set; }
    }
}