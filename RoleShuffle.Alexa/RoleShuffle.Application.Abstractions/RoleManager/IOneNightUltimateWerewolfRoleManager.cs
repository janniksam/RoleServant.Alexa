using RoleShuffle.Application.Abstractions.Model;
using RoleShuffle.Base.Aspects;

namespace RoleShuffle.Application.Abstractions.RoleManager
{
    [LogMethodScope]
    public interface IOneNightUltimateWerewolfRoleManager
    {
        int AddRoleSelection(RoleSelection roleSelection);
        RoleSelection LoadRoles(string deckSelection);
        RoleSelection GetRoleSelection(int deckId);
    }
}