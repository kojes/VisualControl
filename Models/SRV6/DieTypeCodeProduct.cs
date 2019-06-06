using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Models.SRV6 {
    [Table ("CodeProduct_DieType")]
    public class DieTypeCodeProduct {
        public int CodeProductId { get; set; }
        public int DieTypeId { get; set; }

        public CodeProduct CodeProduct { get; set; }
        public DieType DieType { get; set; }
    }
}