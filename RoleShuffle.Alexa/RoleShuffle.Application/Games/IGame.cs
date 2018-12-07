using Alexa.NET.Request;
using Alexa.NET.Response;

namespace RoleShuffle.Application.Games
{
    public interface IGame
    {
        SkillResponse StartGameRequested(SkillRequest skillRequest);

        SkillResponse DistributeRoles(SkillRequest request);

        SkillResponse PerformNightPhase(SkillRequest request);

        bool IsPlaying(string userId);

        short GameNumber { get; }

        string GameName { get; }
    }
}