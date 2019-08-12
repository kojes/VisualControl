using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers.ChipVerification.Abstract;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    public class FacilityController : Controller
    {
        public readonly IFacilityProvider _facilityProvider;
        public FacilityController(IFacilityProvider facilityProvider)
        {
            _facilityProvider = facilityProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<FacilityViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _facilityProvider.GetAllAsync();
            return result.HasErrors ? (ActionResult)NotFound(result.GetErrors()) : (ActionResult)Ok(result.TObject);
        }
    }
}