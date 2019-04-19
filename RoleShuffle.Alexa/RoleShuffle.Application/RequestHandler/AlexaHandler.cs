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
        private readonly ILogger<AlexaHandler> m_logger;
        private const string LocaleAll = "All";

        public AlexaHandler(
            IIntentHandler intentHandler, 
            ILogger<AlexaHandler> logger)
        {
            m_logger = logger;
            m_intentHandler = intentHandler;
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
                var ssml = await CommonResponseCreator.GetSSMLAsync(MessageKeys.LaunchMessage, request.Request.Locale).ConfigureAwait(false);
                var launchResponse = ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = ssml });
                launchResponse.Response.ShouldEndSession = false;
                return launchResponse;
            }

            if (request.GetRequestType() != typeof(IntentRequest))
            {
                m_logger.LogWarning($"The request-type {request.GetRequestType()} isn't supported.");
                var ssml = await CommonResponseCreator.GetSSMLAsync(MessageKeys.ErrorRequestTypeNotSupported,
                    request.Request.Locale, request.GetRequestType()?.ToString()).ConfigureAwait(false);
                return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = ssml });
            }

            var response = await m_intentHandler.GetResponseAsync(request).ConfigureAwait(false);
            return response;
        }
    }
}