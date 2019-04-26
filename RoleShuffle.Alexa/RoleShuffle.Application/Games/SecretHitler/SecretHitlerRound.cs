using System;
using RoleShuffle.Application.Abstractions.Games;

namespace RoleShuffle.Application.Games.SecretHitler
{
    public class SecretHitlerRound : IGameRound
    {
        public SecretHitlerRound()
        {
            CreationTime = DateTime.UtcNow;
        }

        public string UserId { get; set; }

        public short PlayerAmount { get; set;  }

        public string CreationLocale { get; set; }

        public DateTime CreationTime { get; set; }
    }
}