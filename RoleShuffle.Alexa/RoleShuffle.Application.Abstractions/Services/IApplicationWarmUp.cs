using System.Threading.Tasks;

namespace RoleShuffle.Application.Abstractions.Services
{
    public interface IApplicationWarmUp
    {
        Task WarmUp();
    }
}