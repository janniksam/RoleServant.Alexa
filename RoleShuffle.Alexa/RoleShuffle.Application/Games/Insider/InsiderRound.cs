using System;
using RoleShuffle.Application.Abstractions.Games;

namespace RoleShuffle.Application.Games.Insider
{
    public class InsiderRound : IGameRound
    {
        public InsiderRound()
        {
            CreationTime = DateTime.UtcNow;
        }

        public string CreationLocale { get; set; }

        public DateTime CreationTime { get; }
    }
}