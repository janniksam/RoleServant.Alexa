using System;

namespace RoleShuffle.Application.Abstractions.Games
{
    public interface IGameRound
    {
        string CreationLocale { get; set; }

        DateTime CreationTime { get; }

        string UserId { get; set;  }
    }
}