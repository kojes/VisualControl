using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers.ChipVerification.Abstract;

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

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var facilityList = await _facilityProvider.GetAllAsync();
            return facilityList.Count() > 0 ? (ActionResult)Ok(facilityList) : (ActionResult)NoContent();
        }
    }
}