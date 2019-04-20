using System.Collections.Generic;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Games.Insider
{
    public class InsiderGame : BaseGame<InsiderRound>
    {
        public InsiderGame()
            : base("Insider", "Insider", Constants.GameNumbers.Insider)
        {
        }

        public override IEnumerable<string> GetRequiredSSMLViews()
        {
            return new[] { NightPhaseView };
        }

        public override Task<SkillResponse> PerformNightPhase(SkillRequest request)
        {
            if (!RunningRounds.TryGetValue(request.Context.System.User.UserId, out var round) || round == null)
            {
                return NoActiveGameOpen(request);
            }

            return PerformDefaultNightPhase(request);
        }

        public override Task<SkillResponse> StartGameRequested(SkillRequest request)
        {
            var userId = request.Context.System.User.UserId;
            var newRound = new InsiderRound();
            RunningRounds.AddOrUpdate(userId, newRound, (k, v) => newRound);

            return PerformDefaultStartGamePhaseWithNightPhaseContinuation(request);
        }
    }
}