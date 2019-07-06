using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Games.ActOfTreason
{
    public class ActOfTreasonGame : BaseSSMLGame<ActOfTreasonRound>
    {
        public ActOfTreasonGame()
            : base("ActOfTreason", Constants.GameNumbers.ActOfTreason)
        {
        }

        public override IEnumerable<string> GetRequiredSSMLViews()
        {
            return base.GetRequiredSSMLViews()
                .Concat(new[]
                {
                    DistributeRolesView
                });
        }

        public override Task<SkillResponse> DistributeRoles(SkillRequest request)
        {
            return PerformDefaultDistributionPhase(request);
        }

        public override Task<SkillResponse> StartGameRequested(SkillRequest request)
        {
            var userId = request.Context.System.User.UserId;
            var newRound = new ActOfTreasonRound()
            {
                UserId = userId,
                CreationLocale = request.Request.Locale
            };

            CreateRound(newRound);

            return PerformDefaultStartGamePhaseWithDistributionPhaseContinuation(request);
        }
    }
}