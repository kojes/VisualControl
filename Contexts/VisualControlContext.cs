using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Models;

namespace VueExample.Contexts
{
    public class VisualControlContext : DbContext
    {
        public DbSet<DefectType> DefectTypes { get; set; }
        public DbSet<DangerLevel> DangerLevels { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source = SRV6\SRV3; Initial Catalog = VisualControl; persist security info = True; user id = labuser; password = zxvitr78KK; MultipleActiveResultSets = True;");
        }
    }
}
