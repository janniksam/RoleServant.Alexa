using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using RoleShuffle.Application.Extensions;
using RoleShuffle.Application.Games;
using RoleShuffle.Application.ResponseMessages;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Intents
{
    public class StartNewGameIntent : IIntent
    {
        private readonly IEnumerable<IGame> m_availableGames;
        private readonly IMessages m_messages;

        public StartNewGameIntent(
            IEnumerable<IGame> availableGames,
            IMessages messages)
        {
            m_availableGames = availableGames;
            m_messages = messages;
        }

        public bool IsResponseFor(string intent)
        {
            return intent == Constants.Intents.StartNewGame;
        }

        public async Task<SkillResponse> GetResponse(SkillRequest skillRequest)
        {
            var request = (IntentRequest) skillRequest.Request;
            var gameNumberRaw = request.Intent.GetSlot(Constants.Slots.GameNumber);
            if (!short.TryParse(gameNumberRaw, out var gameNumber) ||
                m_availableGames.All(p => p.GameNumber != gameNumber))
            {
                var outputText = "<speak><p>Folgende Spiele sind zurzeit verfügbar:</p><p>";
                foreach (var game in m_availableGames.OrderBy(p => p.GameNumber))
                {
                    outputText += $"{game.GameNumber}:. {game.GameName}. ";
                }

                outputText += "</p><p>Bitte suchen Sie ein Spiel aus, indem Sie die dazugehörige Zahl auswählen.</p></speak>";

                return ResponseBuilder.DialogElicitSlot(
                    new SsmlOutputSpeech
                    {
                        Ssml = outputText
                    },
                    Constants.Slots.GameNumber,
                    request.Intent);
            }

            var usersGame = m_availableGames.First(p => p.GameNumber == gameNumber);
            return usersGame.StartGameRequested(skillRequest);
        }
    }
}