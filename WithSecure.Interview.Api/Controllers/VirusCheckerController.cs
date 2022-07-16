using Microsoft.AspNetCore.Mvc;
using WithSecure.Interview.Api.Dtos.VirusChecker;

namespace WithSecure.Interview.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VirusCheckerController : ControllerBase
    {
        [HttpPost]
        public IActionResult CheckForViruses([FromBody] VirusCheckerRequestDto file)
        {
            return Ok(new VirusCheckerResponseDto { Result = "clean" });
        }
    }
}