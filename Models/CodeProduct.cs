using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VueExample.Models.SRV6;

namespace VueExample.Models
{
    [Table("Code_product")]
    public class CodeProduct
    {   [Key()]
        [Column("id_cp")]
        public int IdCp { get; set; }
        [Column("id_processlist")]
        public int ProcessId { get; set; }
        [Column("Cp_Name")]
        public string CodeProductName { get; set; }
        public List<DieTypeCodeProduct> DieTypeCodeProducts { get; set; }
    }
}
