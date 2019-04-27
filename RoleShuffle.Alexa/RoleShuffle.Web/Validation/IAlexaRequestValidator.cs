using System.Threading.Tasks;
using Alexa.NET.Request;
using Microsoft.AspNetCore.Http;
using RoleShuffle.Base.Aspects;

namespace RoleShuffle.Web.Validation
{
    public interface IAlexaRequestValidator
    {
        [LogResult]
        [LogMethodScope]
        Task<bool> ValidateRequest(HttpRequest request, SkillRequest skillRequest);
    }
}