using System.Collections.Generic;

namespace VueExample.Models.SRV6.Uploader
{
    public class UploadingFile
    {
        public int? MeasurementRecordingId { get; set; }
        public string OperationName { get; set; }
        public string BigMeasurementName { get; set; }
        public int? StageId { get; set; }
        public int ElementId { get; set; }
        public int CodeProductId { get; set; }
        public string WaferId { get; set; }
        public string UserName { get; set; }
        public string Map { get; set; }
        public string Comment { get; set; }
        public string Path { get; set; }
        public bool IsNewMeasurement { get; set; } = true;
        public Dictionary<string, UploadingFileData> Data { get; set; }
        public List<Graphic> Graphics { get; set; } = new List<Graphic>();
        public List<string> GraphicNames { get; set; }

    }
}