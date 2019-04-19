using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using RoleShuffle.Application.Abstractions.Games;
using RoleShuffle.Application.Extensions;
using RoleShuffle.Application.SSMLResponses;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Intents
{
    public class StartNewGameIntent : IIntent
    {
        private readonly IEnumerable<IGame> m_availableGames;

        public StartNewGameIntent(IEnumerable<IGame> availableGames)
        {
            m_availableGames = availableGames;
        }

        public bool IsResponseFor(string intent)
        {
            return intent == Constants.Intents.StartNewGame;
        }

        public async Task<SkillResponse> GetResponse(SkillRequest request)
        {
            var intentRequest = (IntentRequest) request.Request;
            var gameNumberRaw = intentRequest.Intent.GetSlot(Constants.Slots.GameNumber);
            if (!short.TryParse(gameNumberRaw, out var gameNumber) ||
                m_availableGames.All(p => p.GameNumber != gameNumber))
            {
                var ssml = await CommonResponseCreator.GetSSMLAsync(
                    MessageKeys.ChooseGame, 
                    request.Request.Locale,
                    m_availableGames).ConfigureAwait(false);

                return ResponseBuilder.DialogElicitSlot(
                    new SsmlOutputSpeech
                    {
                        Ssml = ssml
                    },
                    Constants.Slots.GameNumber,
                    intentRequest.Intent);
            }

            var usersGame = m_availableGames.First(p => p.GameNumber == gameNumber);
            return await usersGame.StartGameRequested(request).ConfigureAwait(false);
        }
    }
}