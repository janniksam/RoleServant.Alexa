using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.AspNetCore.Mvc;
using RoleShuffle.Application.Abstractions.RequestHandler;
using RoleShuffle.Web.Validation;

namespace RoleShuffle.Web.Controllers
{
    [Route("api/[controller]")]
    public class AlexaController : Controller
    {
        private readonly IAlexaHandler m_handler;
        private readonly IAlexaRequestValidator m_requestValidator;

        public AlexaController(
            IAlexaHandler handler,
            IAlexaRequestValidator requestValidator)
        {
            m_handler = handler;
            m_requestValidator = requestValidator;
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
#if !DEBUG
            var isValid = await m_requestValidator.ValidateRequest(HttpContext.Request, request);
            if (!isValid)
            {
                Response.StatusCode = 400;
                return null;
            }
#endif
            
            var result = await m_handler.HandleAync(request);
            return result;
        }
    }
}