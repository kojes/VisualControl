using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Models;

namespace VueExample.Contexts
{
    public class Srv6Context : DbContext
    {
        public DbSet<CodeProduct> CodeProducts { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<Wafer> Wafers { get; set; }
        public DbSet<Die> Dies { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<WaferMap> WaferMaps { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source = SRV6\SRV3; Initial Catalog = db_process; persist security info = True; user id = labuser; password = zxvitr78KK; MultipleActiveResultSets = True;");
        }
    }
}
