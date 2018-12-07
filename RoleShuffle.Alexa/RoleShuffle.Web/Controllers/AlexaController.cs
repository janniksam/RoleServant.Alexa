using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.AspNetCore.Mvc;
using RoleShuffle.Application.Abstractions.RequestHandler;

namespace RoleShuffle.Web.Controllers
{
    [Route("api/[controller]")]
    public class AlexaController : Controller
    {
        private readonly IAlexaHandler m_handler;

        public AlexaController(IAlexaHandler handler)
        {
            m_handler = handler;
        }

        // GET api/portfolio
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("RoleShuffle Alexa API Endpoint.");
        }

        [HttpPost]
        public async Task<SkillResponse> Post([FromBody]SkillRequest request)
        {
            var result = await m_handler.HandleAync(request);
            return result;
        }
    }
}