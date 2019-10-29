using Microsoft.EntityFrameworkCore;
using VueExample.Entities;
using VueExample.Models;
using VueExample.Models.SRV6;
using Graphic = VueExample.Models.SRV6.Graphic;

namespace VueExample.Contexts
{
    public class Srv6Context : DbContext {
        public DbSet<CodeProduct> CodeProducts { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<Wafer> Wafers { get; set; }
        public DbSet<Die> Dies { get; set; }
        public DbSet<Element> Elements { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<WaferMap> WaferMaps { get; set; }
        public DbSet<Models.SRV6.DieType> DieTypes { get; set; }
        public DbSet<DieGraphics> DieGraphics { get; set; }
        public DbSet<MeasurementRecording> MeasurementRecordings { get; set; }
        public DbSet<Graphic> Graphics { get; set; }
        public DbSet<DieParameterOld> DiesParameterOld { get; set; }
        public DbSet<FkMrP> FkMrPs { get; set; }
        public DbSet<ShortLinkEntity> ShortLinkEntities { get; set; }
        public DbSet<Divider> Dividers { get; set; }
        public DbSet<StatParameterForStage> StatParametersForStage { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            modelBuilder.Entity<DieTypeElement>()
                .HasKey(t => new { t.DieTypeId, t.ElementId });
            
            modelBuilder.Entity<DieTypeCodeProduct>()
                .HasKey(t => new { t.DieTypeId, t.CodeProductId});

            modelBuilder.Entity<MeasurementRecordingElement>()
                .HasKey(t => new { t.ElementId, t.MeasurementRecordingId});

            modelBuilder.Entity<DieTypeElement>()
                .HasOne(pt => pt.DieType)
                .WithMany(p => p.DieTypeElements)
                .HasForeignKey(pt => pt.DieTypeId);

            modelBuilder.Entity<MeasurementRecordingElement>()
                .HasOne(pt => pt.Element)
                .WithMany(p => p.MeasurementRecordingElements)
                .HasForeignKey(pt => pt.ElementId);
            
            modelBuilder.Entity<MeasurementRecordingElement> ()
                .HasOne(pt => pt.MeasurementRecording)
                .WithMany(p => p.MeasurementRecordingElements)
                .HasForeignKey(pt => pt.MeasurementRecordingId);             

            modelBuilder.Entity<DieTypeCodeProduct>()
                .HasOne(pt => pt.CodeProduct)
                .WithMany(p => p.DieTypeCodeProducts)
                .HasForeignKey(pt => pt.CodeProductId);
                
            modelBuilder.Entity<DieTypeCodeProduct>()
                .HasOne(pt => pt.DieType)
                .WithMany(p => p.DieTypeCodeProducts)
                .HasForeignKey(pt => pt.DieTypeId);
        }
        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer (@"data source = SRV6\SRV3; Initial Catalog = db_process; persist security info = True; user id = labuser; password = zxvitr78KK; MultipleActiveResultSets = True;");
        }
    }
}