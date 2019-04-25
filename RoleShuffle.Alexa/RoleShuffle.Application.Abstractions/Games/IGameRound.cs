using System;

namespace RoleShuffle.Application.Abstractions.Games
{
    public interface IGameRound
    {
        string CreationLocale { get; }
        DateTime CreationTime { get; }
    }
}