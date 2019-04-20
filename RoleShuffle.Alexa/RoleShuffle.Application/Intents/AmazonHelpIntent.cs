using System.Data.SqlTypes;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.AspNetCore.Http;
using RoleShuffle.Application.SSMLResponses;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Intents
{
    public class AmazonHelpIntent : IIntent
    {
        public bool IsResponseFor(string intent)
        {
            return intent == Constants.Intents.Help;
        }

        public async Task<SkillResponse> GetResponse(SkillRequest request)
        {
            var ssml = await CommonResponseCreator.GetSSMLAsync(MessageKeys.HelpMessage, request.Request.Locale).ConfigureAwait(false); ;
            var response = ResponseBuilder.Tell(new SsmlOutputSpeech {Ssml = ssml});
            response.Response.ShouldEndSession = false;
            return response;
        }
    }
}