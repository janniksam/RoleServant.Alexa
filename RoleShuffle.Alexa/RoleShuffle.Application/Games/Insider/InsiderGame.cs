using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Games.Insider
{
    public class InsiderGame : BaseSSMLGame<InsiderRound>
    {
        public InsiderGame()
            : base("Insider", Constants.GameNumbers.Insider)
        {
        }

        public override IEnumerable<string> GetRequiredSSMLViews()
        {
            return base.GetRequiredSSMLViews()
                .Concat(new[] {NightPhaseView});
        }

        public override Task<SkillResponse> PerformNightPhase(SkillRequest request)
        {
            return PerformDefaultNightPhase(request);
        }

        public override Task<SkillResponse> StartGameRequested(SkillRequest request)
        {
            var userId = request.Context.System.User.UserId;
            var newRound = new InsiderRound
            {
                UserId = userId,
                CreationLocale = request.Request.Locale
            };

            CreateRound(newRound);

            return PerformDefaultStartGamePhaseWithNightPhaseContinuation(request);
        }
    }
}