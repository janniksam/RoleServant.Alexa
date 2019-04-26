using System.Collections.Generic;
using RoleShuffle.Application.Abstractions.Games;

namespace RoleShuffle.Application.Abstractions.Model
{
    public class SavedGame
    {
        public string Id { get; set; }

        public GameStats GameStats { get; set; }

        public List<IGameRound> GameRounds { get; set; }
    }
}