using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace VueExample.Models.SRV6
{
    [Table("DieType")]
    public class DieType
    {
        [Column("id")]
        public int DieTypeId { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        public ICollection<DieTypeElement> DieTypeElements { get; set; }
        public List<DieTypeCodeProduct> DieTypeCodeProducts { get; set; }
    }
}