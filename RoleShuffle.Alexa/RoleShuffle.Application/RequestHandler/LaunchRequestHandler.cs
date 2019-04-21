using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using RoleShuffle.Application.Abstractions.Games;
using RoleShuffle.Application.Abstractions.RequestHandler;
using RoleShuffle.Application.SSMLResponses;

namespace RoleShuffle.Application.RequestHandler
{
    public class LaunchRequestHandler : ILaunchRequestHandler
    {
        private readonly IEnumerable<IGame> m_availableGames;

        public LaunchRequestHandler(IEnumerable<IGame> availableGames)
        {
            m_availableGames = availableGames;
        }

        public async Task<SkillResponse> GetResponseAsync(SkillRequest request)
        {
            var currentlyOpenGame =
                m_availableGames.FirstOrDefault(p => p.IsPlaying(request.Context.System.User.UserId));

            var ssml = await CommonResponseCreator
                .GetSSMLAsync(MessageKeys.LaunchMessage, request.Request.Locale, currentlyOpenGame)
                .ConfigureAwait(false);

            var launchResponse = ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = ssml });
            launchResponse.Response.ShouldEndSession = false;
            return launchResponse;
        }
    }
}