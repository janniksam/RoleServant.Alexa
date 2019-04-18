using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using RoleShuffle.Application.SSMLResponses;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Intents
{
    public class AmazonCancelIntent : IIntent
    {
        public bool IsResponseFor(string intent)
        {
            return intent == Constants.Intents.Cancel;
        }

        public async Task<SkillResponse> GetResponse(SkillRequest request)
        {
            var ssml = await CommonResponseCreator.GetSSMLAsync(MessageKeys.StopMessage, request.Request.Locale).ConfigureAwait(false);
            return ResponseBuilder.Tell(new SsmlOutputSpeech {Ssml = ssml});
        }
    }
}