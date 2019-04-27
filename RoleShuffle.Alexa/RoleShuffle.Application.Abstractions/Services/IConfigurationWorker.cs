using System.Threading;
using System.Threading.Tasks;
using RoleShuffle.Base.Aspects;

namespace RoleShuffle.Application.Abstractions.Services
{
    [LogMethodScope]
    public interface IConfigurationWorker
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}