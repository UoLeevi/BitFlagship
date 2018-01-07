using Microsoft.AspNetCore.Mvc;

namespace BitFlagship.Controllers
{
    [Route("/")]
    public class RootController : Controller
    {
        [HttpGet]
        public IActionResult Get()
            => Ok("Try out the href API: bitflagship.com/href");
    }
}