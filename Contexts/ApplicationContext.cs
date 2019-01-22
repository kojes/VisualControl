using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Models;

namespace VueExample.Contexts
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Models.Point> Point { get; set; }
        public DbSet<Graphic> Graphic { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<Measurement> Measurement { get; set; }
        public DbSet<MeasuredDevice> MeasuredDevice { get; set; }
      
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source = SRV3\SRV3; Initial Catalog = CrossTesting; persist security info = True; user id = vcu; password = straw7pi; MultipleActiveResultSets = True;");
        }
    }
}
