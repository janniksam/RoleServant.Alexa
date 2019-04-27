using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using RoleShuffle.Base.Aspects;

namespace RoleShuffle.Application.Intents
{
    public interface IIntent
    {
        bool IsResponseFor(string intent);

        [LogMethodScope]
        Task<SkillResponse> GetResponse(SkillRequest request);
    }
}