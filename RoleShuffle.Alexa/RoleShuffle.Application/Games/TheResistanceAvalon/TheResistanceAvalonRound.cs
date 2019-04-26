using System;
using RoleShuffle.Application.Abstractions.Games;

namespace RoleShuffle.Application.Games.TheResistanceAvalon
{
    public class TheResistanceAvalonRound : IGameRound
    {
        public TheResistanceAvalonRound()
        {
            CreationTime = DateTime.UtcNow;
        }

        public string UserId { get; set; }

        public bool Percival { get; set; }

        public bool Morgana { get; set; }

        public bool Mordred { get; set; }

        public bool Oberon { get; set; }

        public string CreationLocale { get; set; }

        public DateTime CreationTime { get; set; }
    }
}