using System.Threading;
using System.Threading.Tasks;

namespace RoleShuffle.Application.Abstractions.Services
{
    public interface IConfigurationWorker
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}