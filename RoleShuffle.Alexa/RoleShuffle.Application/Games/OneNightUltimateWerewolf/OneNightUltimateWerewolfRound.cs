using System;
using RoleShuffle.Application.Abstractions.Model;

namespace RoleShuffle.Application.Games.OneNightUltimateWerewolf
{
    public class OneNightUltimateWerewolfRound
    {
        public OneNightUltimateWerewolfRound(RoleSelection roleSelection)
        {
            RoleSelection = roleSelection ?? throw new ArgumentNullException(nameof(roleSelection));
        }

        public RoleSelection RoleSelection { get; }
    }
}