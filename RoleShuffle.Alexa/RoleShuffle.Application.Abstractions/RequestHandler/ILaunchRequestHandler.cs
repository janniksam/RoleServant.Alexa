using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace RoleShuffle.Application.Abstractions.RequestHandler
{
    public interface ILaunchRequestHandler
    {
        Task<SkillResponse> GetResponseAsync(SkillRequest request);
    }
}