using System.Collections.Generic;

namespace RoleShuffle.Application.Abstractions.Model
{
    public class RoleServantConfiguration
    {
        public List<SavedGame> SavedGames { get; set; } = new List<SavedGame>();
    }
}