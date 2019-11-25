using System.Collections.Generic;
using VueExample.Models.SRV6.Uploader;

namespace VueExample.ViewModels
{
    public class FileNameUploaderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProcessId { get; set; }
        public List<GraphicNameUploaderViewModel> GraphicNames { get; set; }
        
    }
}