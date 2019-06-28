using System;
using System.Collections.Generic;
using RoleShuffle.Application.Abstractions.Games;

namespace RoleShuffle.Web.Models
{
    public class Statistics
    {
        public Dictionary<IGame, IEnumerable<IGameRound>> GameRoundsPerGame { get; set; }

        public int TotalGameRoundsUpAndRunning { get; set; }

        public long TotalRoundsStarted { get; set; }

        public DateTime? LatestRoundOfTotal { get; set; }

        public string LatestRoundOfTotalString => LatestRoundOfTotal.HasValue ? LatestRoundOfTotal.Value.ToString("g") : "Never";

        public IEnumerable<IGame> Games { get; set; }

        public DateTime BackupDate { get; set; }
    }
}