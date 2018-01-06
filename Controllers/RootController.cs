using Microsoft.AspNetCore.Mvc;

namespace BitFlagship.Controllers
{
    [Produces("application/json")]
    [Route("/")]
    public class RootController : Controller
    {
        [HttpGet("{endPoint?}")]
        public IActionResult GetEcho(string endPoint)
            => Ok(Json(endPoint ?? "Hello from BitFlagship!"));
    }
}