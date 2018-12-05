using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PhotoController : Controller
    {
        private IPhotoProvider _photoProvider;

        public PhotoController(IPhotoProvider photoProvider)
        {
            _photoProvider = photoProvider;
        }

        [HttpGet]
        public IActionResult GetPhotoStorageAddress()
        {
            return Ok(ExtraConfiguration.PhotoStorageAddress);
        }

        [HttpGet]
        public IActionResult GetPhotosByDefectId([FromQuery]int defectId)
        {
            return Ok(_photoProvider.GetPhotosByDefectId(defectId));
        }
    }
}
