using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace RoleShuffle.Application.Intents
{
    public interface IIntent
    {
        bool IsResponseFor(string intent);

        Task<SkillResponse> GetResponse(SkillRequest skillRequest);
    }
}