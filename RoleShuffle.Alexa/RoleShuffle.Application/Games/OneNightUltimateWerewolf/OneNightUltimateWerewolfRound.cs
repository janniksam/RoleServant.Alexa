using System;
using RoleShuffle.Application.Abstractions.Games;
using RoleShuffle.Application.Abstractions.Model;

namespace RoleShuffle.Application.Games.OneNightUltimateWerewolf
{
    public class OneNightUltimateWerewolfRound : IGameRound
    {
        public OneNightUltimateWerewolfRound(RoleSelection roleSelection)
        {
            CreationTime = DateTime.UtcNow;
            RoleSelection = roleSelection ?? throw new ArgumentNullException(nameof(roleSelection));
        }

        public RoleSelection RoleSelection { get; }

        public string CreationLocale { get; set; }

        public DateTime CreationTime { get; }
    }
}