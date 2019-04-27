using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoleShuffle.Base.Aspects;

namespace RoleShuffle.Application.Abstractions.Services
{
    [LogMethodScope(LogLevel.Information)]
    public interface IApplicationWarmUp
    {
        Task WarmUp();
    }
}