using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers.Abstract;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    public class ShortLinkController : Controller
    {
        private readonly IShortLinkProvider _shortLinkProvider;
        public ShortLinkController(IShortLinkProvider shortLinkProvider)
        {
            _shortLinkProvider = shortLinkProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ShortLinkInfoViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        [Route("{shortlink}/element-export")]
        public async Task<IActionResult> GetElementExportDetails([FromRoute] string shortLink)
        {
            var result = await _shortLinkProvider.GetElementExportDetails(shortLink);
            return result.HasErrors ? (IActionResult)NotFound(result.GetErrors()) : (IActionResult)Ok(result.TObject);
        }
    }
}