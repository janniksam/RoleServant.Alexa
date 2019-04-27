using System.Collections.Generic;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using RoleShuffle.Application.Abstractions.Model;
using RoleShuffle.Base.Aspects;

namespace RoleShuffle.Application.Abstractions.Games
{
    public interface IGame
    {
        [LogMethodScope]
        Task<SkillResponse> StartGameRequested(SkillRequest skillRequest);

        [LogMethodScope]
        Task<SkillResponse> DistributeRoles(SkillRequest request);

        [LogMethodScope]
        Task<SkillResponse> PerformNightPhase(SkillRequest request);

        [LogMethodScope]
        bool IsPlaying(string userId);

        [LogMethodScope]
        void StopPlaying(SkillRequest request);

        [LogMethodScope]
        IEnumerable<IGameRound> GetOpenGames();

        [LogMethodScope]
        void InitFromConfiguration(SavedGame game);

        [LogMethodScope]
        GameStats GetCurrentStats();

        [LogMethodScope]
        IEnumerable<string> GetRequiredSSMLViews();

        Task<string> GetLocalizedGamename(string locale);

        short GameNumber { get; }

        string GameId { get; }
    }
}