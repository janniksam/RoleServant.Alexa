using System;
using RoleShuffle.Application.Abstractions.Games;
using RoleShuffle.Application.Abstractions.Model;

namespace RoleShuffle.Application.Games.OneNightUltimateWerewolf
{
    public class OneNightUltimateWerewolfRound : IGameRound
    {
        public OneNightUltimateWerewolfRound()
        {
            CreationTime = DateTime.UtcNow;
        }

        public string UserId { get; set; }

        public RoleSelection RoleSelection { get; set; }

        public string CreationLocale { get; set; }

        public DateTime CreationTime { get; set; }
    }
}