using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.Models.SRV6
{
    [Table("StatisticParameter")]
    public class StatisticParameter
    {
        
        [Column("id_parameter")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("HTMLName")]
        public string HTMLName { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("Unit")]
        public string Unit { get; set; }
        [Column("Millimeter")]
        public string Millimeter { get; set; }
        [Column("MillimeterUnit")]
        public string MillimeterUnit { get; set; }
        [Column("UserForMaps")]
        public string UsedForMaps { get; set; }
    }
}