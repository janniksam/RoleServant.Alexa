using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RoleShuffle.Application.Abstractions.Games;

namespace RoleShuffle.Web.Controllers
{
    [Route("api/[controller]")]
    public class StatsController : Controller
    {
        private readonly IEnumerable<IGame> m_availableGames;

        public StatsController(IEnumerable<IGame> availableGames)
        {
            m_availableGames = availableGames;
        }

        // GET api/portfolio
        [HttpGet]
        public IActionResult Get()
        {
            return View("Index", m_availableGames);
        }
    }
}