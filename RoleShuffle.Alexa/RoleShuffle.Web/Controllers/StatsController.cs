using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RoleShuffle.Application.Abstractions.Games;
using RoleShuffle.Application.Abstractions.Services;
using RoleShuffle.Web.Models;

namespace RoleShuffle.Web.Controllers
{
    [Route("api/[controller]")]
    public class StatsController : Controller
    {
        private readonly IEnumerable<IGame> m_availableGames;
        private readonly IConfigurationManager m_configurationManager;

        public StatsController(IEnumerable<IGame> availableGames, IConfigurationManager configurationManager)
        {
            m_availableGames = availableGames;
            m_configurationManager = configurationManager;
        }

        // GET api/portfolio
        [HttpGet]
        public IActionResult Get()
        {
            var gameRounds = new Dictionary<IGame, IEnumerable<IGameRound>>();
            foreach (var game in m_availableGames)
            {
                gameRounds.Add(game, game.GetOpenGames());
            }

            var gameRoundCount = gameRounds.Sum(p => p.Value.Count());
            var totalRoundsStarted = gameRounds.Sum(p => p.Key.GetCurrentStats().TotalGamesCreated);
            var latestRoundOfTotal = m_availableGames.SelectMany(p => p.GetOpenGames()).Max(p => p.LastUsed);

            return View("Index",
                new Statistics
                {
                    Games = m_availableGames,
                    BackupDate = m_configurationManager.GetLastBackupDate(),
                    GameRoundsPerGame = gameRounds,
                    TotalGameRoundsUpAndRunning = gameRoundCount,
                    LatestRoundOfTotal = latestRoundOfTotal,
                    TotalRoundsStarted = totalRoundsStarted,
                });
        }
    }
}