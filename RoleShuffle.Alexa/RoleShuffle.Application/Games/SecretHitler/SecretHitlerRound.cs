using System;
using RoleShuffle.Application.Abstractions.Games;

namespace RoleShuffle.Application.Games.SecretHitler
{
    public class SecretHitlerRound : IGameRound
    {
        public SecretHitlerRound(short playerAmount)
        {
            CreationTime = DateTime.UtcNow;
            PlayerAmount = playerAmount;
        }

        public short PlayerAmount { get; }

        public string CreationLocale { get; set; }

        public DateTime CreationTime { get; }
    }
}