using Microsoft.AspNetCore.Http;

namespace VueExample.Models
{
    public class FileInputModel
    {
        public string Name { get; set; }
        public IFormFile FileToUpload { get; set; }
    }
}
