using Microsoft.EntityFrameworkCore;
using VueExample.Entities;
using VueExample.Models;
using VueExample.Models.SRV6;
using VueExample.Models.SRV6.Uploader;
using Graphic = VueExample.Models.SRV6.Graphic;
using MeasurementRecordingElement = VueExample.Models.SRV6.MeasurementRecordingElement;

namespace VueExample.Contexts
{
    public class Srv6Context : DbContext {
        public DbSet<CodeProduct> CodeProducts { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<Wafer> Wafers { get; set; }
        public DbSet<Die> Dies { get; set; }
        public DbSet<Element> Elements { get; set; }
        public DbSet<ElementType> ElementTypes { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<WaferMap> WaferMaps { get; set; }
        public DbSet<Models.SRV6.DieType> DieTypes { get; set; }
        public DbSet<DieGraphics> DieGraphics { get; set; }
        public DbSet<MeasurementRecording> MeasurementRecordings { get; set; }
        public DbSet<BigMeasurementRecording> BigMeasurementRecordings { get; set; }
        public DbSet<Graphic> Graphics { get; set; }
        public DbSet<FileName> FileNames { get; set; }
        public DbSet<GraphicName> GraphicNames { get; set; }
        public DbSet<FileNameGraphic> FileNameGraphics { get; set; }
        public DbSet<DieParameterOld> DiesParameterOld { get; set; }
        public DbSet<Entities.DieTypeElement> DieTypeElements{ get; set; }
        public DbSet<DieTypeCodeProduct> DieTypeCodeProducts { get; set; }
        public DbSet<CodeProductGraphic> CodeProductGraphic { get; set; }
        public DbSet<CodeProductStandartWafer> CodeProductStandartWafers { get; set; }
        public DbSet<MapStandartWafer> MapStandartWafers { get; set; }
        public DbSet<MeasurementRecordingElement> MeasurementRecordingElements {get; set;}
        public DbSet<FkMrP> FkMrPs { get; set; }
        public DbSet<FkMrGraphic> FkMrGraphics { get; set; }
        public DbSet<ShortLinkEntity> ShortLinkEntities { get; set; }
        public DbSet<Divider> Dividers { get; set; }
        public DbSet<StatParameterForStage> StatParametersForStage { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
           
           
        }
        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer (@"data source = SRV6\SRV3; Initial Catalog = db_process; persist security info = True; user id = labuser; password = zxvitr78KK; MultipleActiveResultSets = True;");
        }
    }
}