using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Models;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PhotoUploadingController : Controller
    {

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult SaveImage()
        {
            var pathFolder = Guid.NewGuid().ToString("N");
            var tempFolderPath = Path.GetTempPath();
            Directory.CreateDirectory($"{tempFolderPath}\\{pathFolder}");
            if (HttpContext.Request.Form.Files[0] != null)
            {
                var file = HttpContext.Request.Form.Files[0];
                
                using (FileStream fs = new FileStream($"{tempFolderPath}\\{pathFolder}\\{file.FileName}", FileMode.CreateNew, FileAccess.Write, FileShare.Write))
                {
                    file.CopyTo(fs);
                }
                return Content(pathFolder);
            }

            return StatusCode(500);

        }


        [HttpDelete]
        public IActionResult RevertImage()
        {
            var tempFolderPath = Path.GetTempPath();
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    var pathFolder = reader.ReadToEnd();
                    Directory.Delete($"{tempFolderPath}\\{pathFolder}", true);
                }
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }

            return StatusCode(500);


        }
    }
}
