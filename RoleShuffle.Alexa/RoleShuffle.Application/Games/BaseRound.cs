using System;
using RoleShuffle.Application.Abstractions.Games;

namespace RoleShuffle.Application.Games
{
    public abstract class BaseRound : IGameRound
    {
        protected BaseRound()
        {
            CreationTime = DateTime.UtcNow;
        }

        public string UserId { get; set; }

        public string CreationLocale { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastUsed { get; set; }

        public void UpdateLastUsed()
        {
            LastUsed = DateTime.UtcNow;
        }
    }
}