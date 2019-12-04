using System.Collections.Generic;
using VueExample.Models.SRV6.Uploader;

namespace VueExample.ViewModels
{
    public class FileNameUploaderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int ProcessId { get; set; }
        public List<GraphicNameUploaderViewModel> GraphicNames { get; set; }
        
    }

    public class FileNameUploaderUViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProcessId { get; set; }
        public string SelectedGraphicNames { get; set; }
        public List<string> GraphicNames { get; set; }
        
    }
}