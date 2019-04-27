using System;

namespace RoleShuffle.Application.Abstractions.Games
{
    public interface IGameRound
    {
        DateTime CreationTime { get; set; }

        DateTime? LastUsed { get; set; }

        string CreationLocale { get; set; }

        string UserId { get; set;  }

        void UpdateLastUsed();
    }
}