using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using RoleShuffle.Application.SSMLResponses;

namespace RoleShuffle.Application.Intents
{
    public class IntentHandler : IIntentHandler
    {
        private readonly IEnumerable<IIntent> m_intents;
        private readonly ILogger<IntentHandler> m_logger;

        public IntentHandler(
            IEnumerable<IIntent> intents,
            ILogger<IntentHandler> logger)
        {            
            m_intents = intents;
            m_logger = logger;
        }

        public async Task<SkillResponse> GetResponseAsync(SkillRequest request)
        {
            var intentRequest = (IntentRequest) request.Request;

            var intent = m_intents.FirstOrDefault(p => p.IsResponseFor(intentRequest.Intent.Name));
            if (intent != null)
            {
                return await intent.GetResponse(request).ConfigureAwait(false);
            }

            m_logger.LogWarning($"An handler for intend {intentRequest.Intent.Name} hasn't been found.");
            var ssml = await CommonResponseCreator.GetSSMLAsync(MessageKeys.ErrorIntentNotFound, request.Request.Locale).ConfigureAwait(false);
            return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = ssml });
        }
    }
}