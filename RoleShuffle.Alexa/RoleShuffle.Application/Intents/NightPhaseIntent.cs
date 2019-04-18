using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using RoleShuffle.Application.Abstractions.Games;
using RoleShuffle.Application.Games;
using RoleShuffle.Application.SSMLResponses;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Intents
{
    public class NightPhaseIntent : IIntent
    {
        private readonly IEnumerable<IGame> m_availableGames;

        public NightPhaseIntent(
            IEnumerable<IGame> availableGames)
        {
            m_availableGames = availableGames;
        }

        public bool IsResponseFor(string intent)
        {
            return intent == Constants.Intents.NightPhase;
        }

        public async Task<SkillResponse> GetResponse(SkillRequest request)
        {
            var usersGame =
                m_availableGames.FirstOrDefault(p => p.IsPlaying(request.Context.System.User.UserId));
            if (usersGame == null)
            {
                var ssml = await CommonResponseCreator.GetSSMLAsync(MessageKeys.ErrorNoOpenGame, request.Request.Locale).ConfigureAwait(false);
                return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = ssml });
            }

            return await usersGame.PerformNightPhase(request).ConfigureAwait(false);
        }
    }
}