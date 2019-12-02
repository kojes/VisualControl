using System;
namespace VueExample.ViewModels
{
    public class SimpleOperationUploaderViewModel
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public int? StageId { get; set; }
        public ElementUploading Element { get; set; }
        public FileNameUploaderUViewModel FileName { get; set; }
        public string MapType { get; set; } = String.Empty;
        public string Comment { get; set; } = String.Empty;
    }
    
    public class ElementUploading 
    {
        public int? ElementId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }

    }


}