using System.Collections.Generic;

namespace VueExample.Models.SRV6.Uploader
{
    public class UploadingFileData
    {
        public List<string> AbscissList { get; set; } = new List<string>();
        public Dictionary<string, List<string>> ValueLists { get; set; } = new Dictionary<string, List<string>>();
    }
}