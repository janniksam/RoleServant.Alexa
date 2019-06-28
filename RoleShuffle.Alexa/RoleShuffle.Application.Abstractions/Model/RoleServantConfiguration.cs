using System;
using System.Collections.Generic;

namespace RoleShuffle.Application.Abstractions.Model
{
    public class RoleServantConfiguration
    {
        public DateTime LastBackupDate { get; set; }

        public List<SavedGame> SavedGames { get; set; } = new List<SavedGame>();
    }
}