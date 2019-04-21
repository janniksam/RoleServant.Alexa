using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using RoleShuffle.Application.Abstractions.RequestHandler;
using RoleShuffle.Application.Intents;
using RoleShuffle.Application.SSMLResponses;

namespace RoleShuffle.Application.RequestHandler
{
    public class AlexaHandler : IAlexaHandler
    {
        private readonly IIntentHandler m_intentHandler;
        private readonly ILaunchRequestHandler m_launchRequestHandler;
        private const string LocaleAll = "All";

        public AlexaHandler(
            IIntentHandler intentHandler,
            ILaunchRequestHandler launchRequestHandler)
        {
            m_intentHandler = intentHandler;
            m_launchRequestHandler = launchRequestHandler;
        }

        public Task<SkillResponse> HandleAync(SkillRequest request)
        {
            var response = GetResponse(request);
            return response;
        }

        private async Task<SkillResponse> GetResponse(SkillRequest request)
        {
            if (request == null)
            {
                var ssml = await CommonResponseCreator.GetSSMLAsync(MessageKeys.AllLanguages.ErrorMalformedRequest, LocaleAll).ConfigureAwait(false);
                return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = ssml });
            }

            if (request.GetRequestType() == typeof(LaunchRequest))
            {
                return await m_launchRequestHandler.GetResponseAsync(request)
                    .ConfigureAwait(false);
            }
            
            if (request.GetRequestType() != typeof(IntentRequest))
            {
                var ssml = await CommonResponseCreator.GetSSMLAsync(MessageKeys.ErrorRequestTypeNotSupported,
                    request.Request.Locale, request.GetRequestType()?.ToString()).ConfigureAwait(false);
                return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = ssml });
            }

            var response = await m_intentHandler.GetResponseAsync(request).ConfigureAwait(false);
            return response;
        }

      
    }
}