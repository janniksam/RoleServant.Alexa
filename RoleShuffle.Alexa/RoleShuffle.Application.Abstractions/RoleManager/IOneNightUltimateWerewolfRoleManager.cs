using RoleShuffle.Application.Abstractions.Model;

namespace RoleShuffle.Application.Abstractions.RoleManager
{
    public interface IOneNightUltimateWerewolfRoleManager
    {
        int AddRoleSelection(RoleSelection roleSelection);
        RoleSelection LoadRoles(string deckSelection);
        RoleSelection GetRoleSelection(int deckId);
    }
}