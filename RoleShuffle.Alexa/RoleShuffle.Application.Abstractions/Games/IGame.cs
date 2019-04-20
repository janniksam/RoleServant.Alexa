using System.Collections.Generic;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace RoleShuffle.Application.Abstractions.Games
{
    public interface IGame
    {
        Task<SkillResponse> StartGameRequested(SkillRequest skillRequest);

        Task<SkillResponse> DistributeRoles(SkillRequest request);

        Task<SkillResponse> PerformNightPhase(SkillRequest request);

        IEnumerable<string> GetRequiredSSMLViews();
        
        bool IsPlaying(string userId);

        Task<string> GetLocalizedGamename(string locale);

        short GameNumber { get; }

        string SSMLViewFolder { get; }
    }
}