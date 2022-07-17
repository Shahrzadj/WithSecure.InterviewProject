using Microsoft.AspNetCore.Mvc;
using WithSecure.Interview.Api.Dtos.VirusChecker;

namespace WithSecure.Interview.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class VirusCheckerController : ControllerBase
    {
        [HttpPost]
        public ActionResult<VirusCheckerResponseDto> CheckForViruses(IFormCollection file)
        {
            return Ok(new VirusCheckerResponseDto { Result = "clean" });
        }
    }
}